using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.VisualBasic.ApplicationServices;


namespace SignIn
{
    public partial class Chat : Form
    {
        private IFirebaseConfig _config = new FirebaseConfig()
        {
            AuthSecret = "yfvpDIIYfiM64RZYxlaTr7sQ0shK88Sm8Qany2EZ",
            BasePath = "https://messenger-11e03-default-rtdb.firebaseio.com/",
        };
        private IFirebaseClient _client;
        List<User> users;
        List<ChatGroup> chatGroups;
        Dictionary<string, ChatMessages> chatMessages;
        User thisUser;
        User companion;
        ChatMessages ourMessage;
        string FromTo;
        string thisConnectionId;
        int indexSelectedUser;
        int indexSelectedGroup;
        int messageDepth;
        const string ONLINE = "online";

        HubConnection connection;

        public Chat(User thisUser)
        {
            InitializeComponent();
            this.thisUser = thisUser;
            thisUser.IsOnline = true;
            indexSelectedUser = -1;
            messageDepth = 50;
            this.Text += $" ({thisUser.Name})";

            connection = new HubConnectionBuilder()
            .WithUrl($"http://{ConfigurationManager.AppSettings.Get("serverIp")}:{ConfigurationManager.AppSettings.Get("serverPort")}/chat")
            .Build();
        }
        private void listUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listUsers.SelectedIndex == indexSelectedUser)
                return;
            indexSelectedUser = listUsers.SelectedIndex;
            if (indexSelectedUser < users.Count)
            {
                ourMessage = null;
                companion = users[indexSelectedUser];
                DisplayUserMessages(users[indexSelectedUser]);
                labLastTime.Visible = true;
                labUserName.Visible = true;
                labUserName.Text = users[indexSelectedUser].Name;
                DateTime dateTimeUser = users[indexSelectedUser].DateTime;
                if (users[indexSelectedUser].IsOnline)
                    labLastTime.Text = ONLINE;
                else if(DateTime.Now.Day == users[indexSelectedUser].DateTime.Day)
                    labLastTime.Text = $"last seen at {dateTimeUser.Hour}:{dateTimeUser.Minute}";
                else if(DateTime.Now.Day - 1 == users[indexSelectedUser].DateTime.Day)
                    labLastTime.Text = $"last seen yesterday at {dateTimeUser.Hour}:{dateTimeUser.Minute}";
                else
                    labLastTime.Text = $"last seen {dateTimeUser.Day}.{dateTimeUser.Month}.{dateTimeUser.Year} {dateTimeUser.Hour}:{dateTimeUser.Minute}";
            }
            else
            {
                indexSelectedGroup = indexSelectedUser - users.Count;
                DisplayGroupMessages(indexSelectedGroup);
                string nameGroup = chatGroups.FirstOrDefault(g => g.Id == indexSelectedGroup).Name;
                labUserName.Text = nameGroup;
                labUserName.Visible = true;
                labLastTime.Visible = false;
            }
            
        }
        private void Chat_Load(object sender, EventArgs e)
        {
            try
            {
                _client = new FireSharp.FirebaseClient(_config);

                if (_client != null)
                {
                    this.CenterToScreen();
                    //this.Size = Screen.PrimaryScreen.WorkingArea.Size;
                    //this.WindowState = FormWindowState.Normal;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Connection Fail.");
                return;
            }
            InitializeSignalR();
            DisplayUsers();
            DisplayGroup();
        }

        private void DisplayGroup()
        {
            FirebaseResponse response = _client.Get("Group/");
            if (response.Body == "null")
            {
                MessageBox.Show("don't have group");
                return;
            }
            chatGroups = response.ResultAs<List<ChatGroup>>();

            foreach (var group in chatGroups)
            {
                listUsers.Items.Add(group.Name + " (Group)");
            }
        }

        private async void InitializeSignalR()
        {
            connection.On<int, string>("Receive", (senderUserId, message) =>
            {
                Invoke((Action)(() =>
                {
                    if (senderUserId != -1)
                    {
                        int receiverID = int.Parse(message);
                        List<User> user = users.Where(u => u.Id == receiverID).ToList();
                        if (user.Count != 0)
                        {
                            user[0].IsOnline = false;
                            if (user[0].Name == labUserName.Text)
                                labLastTime.Text = "last seen recently";
                        }
                    }
                    else
                    {
                        int receiverID = int.Parse(message);
                        List<User> user = users.Where(u => u.Id == receiverID).ToList();
                        if (user.Count != 0)
                        {
                            user[0].IsOnline = true;
                            if (user[0].Name == labUserName.Text)
                                labLastTime.Text = "online";
                        }

                    }
                }));
            });

            connection.On<int, string, int>("ReceiveToGroup", (senderUserId, message, groupId) =>
            {
                Invoke((Action)(() =>
                {
                    if(labUserName.Text == chatGroups.FirstOrDefault(g => g.Id == groupId).Name)
                    {
                        createLabelMessage(senderUserId, message);
                    }
                }));
            });

            connection.On<int, string, string>("ReceiveToUser", (senderUserId, message, receiverId) =>
            {
                Invoke((Action)(() =>
                {
                    var user = users.Where(u => u.Id == senderUserId).ToList()[0];
                    createLabelMessage(senderUserId, message);
                }));
            });


            try
            {
                // подключемся к хабу
                await connection.StartAsync();

                await connection.InvokeAsync("Send", -1, $"{thisUser.Id}");

                PushId(connection.ConnectionId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        async private void PushId(string connectionId)
        {
            Dictionary<string, string> usersId = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(connectionId))
            {
                FirebaseResponse response = _client.Get($"WhoIsOnline/{thisUser.Name}:{thisUser.Id}");
                if (response.Body == "null")
                {
                    response = _client.Get("WhoIsOnline/");
                    if (response.Body != "null")
                    {
                        usersId = response.ResultAs<Dictionary<string, string>>();
                    }
                    else
                    {
                        usersId = new Dictionary<string, string>();
                    }
                    usersId.Add($"{thisUser.Name}:{thisUser.Id}", connectionId);
                    thisConnectionId = connectionId;
                    _client.Set("WhoIsOnline/", usersId);
                }
                else
                {
                    string str = response.ResultAs<string>();
                    thisConnectionId = connectionId;
                    usersId[$"{thisUser.Name}:{thisUser.Id}"] = thisConnectionId;
                    await _client.UpdateAsync("WhoIsOnline/", usersId);
                }
            }
            else
            {
                MessageBox.Show("something doesn't work");
                return;
            }
        }

        private void DisplayUsers()
        {
            FirebaseResponse response = _client.Get("Users/");
            if (response.Body == "null")
            {
                MessageBox.Show("don't have users");
                return;
            }
            users = response.ResultAs<List<User>>();
            users[users.IndexOf(thisUser)].IsOnline = true;
            _client.Set("Users/", users);
            response = _client.Get("Users/");

            int indexDelete = users.IndexOf(thisUser);
            users.RemoveAt(indexDelete);
            foreach (var user in users)
            {
                listUsers.Items.Add(user.Name);
            }

        }
        private void DisplayUserMessages(User user)
        {
            panelMessages.Controls.Clear();
            messageDepth = 0;
            FirebaseResponse response = _client.Get("Chat/");
            if (response.Body == "null")
            {
                MessageBox.Show("nobody don't use this Chat");
                return;
            }

            chatMessages = response.ResultAs<Dictionary<string, ChatMessages>>();

            foreach (var message in chatMessages)
            {
                List<int> idUsers = message.Key.Split(':').ToList().Select(int.Parse).ToList();
                if ((idUsers.First() == thisUser.Id && idUsers.Last() == user.Id) || (idUsers.First() == user.Id && idUsers.Last() == thisUser.Id))
                {
                    FromTo = $"{idUsers.First()}:{idUsers.Last()}";
                    ourMessage = message.Value;

                }

            }
            if (ourMessage == null)
            {
                MessageBox.Show("you don't send anyone message");
                return;
            }
            foreach (var fromMessage in ourMessage.messages)
            {
                List<string> strings = fromMessage.Split(":", 2).ToList();
                int from = int.Parse(strings[0]);
                string message = strings[1];

                createLabelMessage(from, message);
            }
            

        }

        private void createLabelMessage(int fromId, string message)
        {
            string nameAndMessage;
            int messageAndNameLenght;
            if (fromId == thisUser.Id)
            {
                nameAndMessage = $"I:{message}";
                messageAndNameLenght = message.Length + 2;
            }
            else
            {
                nameAndMessage = $"{users.FirstOrDefault(u => u.Id == fromId).Name}:{message}";
                messageAndNameLenght = message.Length + users.FirstOrDefault(u => u.Id == fromId).Name.Length;
            }
            Label labelMessage = new Label
            {
                Text = nameAndMessage,
                Size = new Size(250, (messageAndNameLenght < 15) ? 30 : (messageAndNameLenght < 55 ? messageAndNameLenght / 15 * 30 : (messageAndNameLenght < 100 ? messageAndNameLenght / 20 * 30 : messageAndNameLenght / 25 * 30))),
                BorderStyle = BorderStyle.Fixed3D,
            };
            if (fromId == thisUser.Id)
                labelMessage.Location = new Point(splitContainer1.Panel2.Width - labelMessage.Width - 100, messageDepth);
            else
                labelMessage.Location = new Point(10, messageDepth);
            messageDepth += (labelMessage.Height + 10);
            panelMessages.Controls.Add(labelMessage);
        }

        async private void btnSend_Click(object sender, EventArgs e)
        {
            if (listUsers.SelectedIndex == -1)
            {
                MessageBox.Show("select user");
                return;
            }
            if (string.IsNullOrEmpty(txbMessage.Text))
            {
                MessageBox.Show("enter your message");
                return;
            }
            if(indexSelectedUser >= users.Count)
            {
                SendInGroup(indexSelectedGroup, txbMessage.Text);
            }
            else
            {
                FirebaseResponse response = _client.Get("Chat/");
                if (response.Body == "null")
                {
                    MessageBox.Show("no one don't use chat");
                    goto sendMessage;
                    //return;
                }
                if (users[indexSelectedUser].IsOnline)
                    SendMessageToOne(users[indexSelectedUser].Id, txbMessage.Text);
                createLabelMessage(thisUser.Id, txbMessage.Text);
            //await connection.InvokeAsync("Send", thisUser.Id, txbMessage.Text);
            sendMessage:
                if (ourMessage == null)
                {
                    ourMessage = new ChatMessages()
                    {
                        user1ID = thisUser.Id,
                        user2ID = users[indexSelectedUser].Id,
                        messages = new List<string>()
                    };
                }
                ourMessage.messages.Add($"{thisUser.Id}:{txbMessage.Text}");
                if (chatMessages.ContainsKey($"{ourMessage.user1ID}:{ourMessage.user2ID}"))
                    chatMessages[$"{ourMessage.user1ID}:{ourMessage.user2ID}"].messages = ourMessage.messages;
                else
                    chatMessages.Add($"{ourMessage.user1ID}:{ourMessage.user2ID}", ourMessage);
                await _client.UpdateAsync("Chat/", chatMessages);
            }
            
            txbMessage.Text = string.Empty;
        }

        async private void SendInGroup(int selectedIndex, string message)
        {
            chatGroups[selectedIndex].Messages.Add($"{thisUser.Id}:{message}");
            await _client.UpdateAsync($"Group/{selectedIndex}", chatGroups[selectedIndex]);
            await connection.InvokeAsync("SendToGroup", thisUser.Id, message, 0);
        }

        async private void SendMessageToOne(int receiverId, string message)
        {
            FirebaseResponse response = _client.Get($"Users/{receiverId}");
            User user = response.ResultAs<User>();
            string userConnectionId = _client.Get($"WhoIsOnline/{user.Name}:{user.Id}").ResultAs<string>();
            await connection.InvokeAsync("SendToUser", thisUser.Id, userConnectionId, message);
        }

        async private void Chat_FormClosed(object sender, FormClosedEventArgs e)
        {
            FirebaseResponse response = _client.Get("Users/");
            users = response.ResultAs<List<User>>();
            //users.IndexOf(thisUser);
            users[users.IndexOf(thisUser)].IsOnline = false;
            users[users.IndexOf(thisUser)].DateTime = DateTime.Now;
            _client.Set("Users/", users);

            DeleteConnectionId();
            await connection.InvokeAsync("Send", 0, $"{thisUser.Id}");
        }

        async private void DeleteConnectionId()
        {
            Dictionary<string, string> usersId;
            FirebaseResponse response = _client.Get("WhoIsOnline/");

            usersId = response.ResultAs<Dictionary<string, string>>();
            usersId[$"{thisUser.Name}:{thisUser.Id}"] = "null";
            await _client.UpdateAsync("WhoIsOnline/", usersId);

        }
        private void DisplayGroupMessages(int idGroup)
        {
            panelMessages.Controls.Clear();
            messageDepth = 0;
            FirebaseResponse response = _client.Get($"Group/");
            if (response.Body == "null")
            {
                MessageBox.Show("nobody don't use this group");
                return;
            }
            chatGroups = response.ResultAs<List<ChatGroup>>();

            ChatGroup group = chatGroups.Where(c => c.Id == idGroup).ToList()[0];
            if (group.Messages.Count == 0)
                MessageBox.Show("no one sent messages");
            foreach (var messages in group.Messages)
            {
                List<string> strings = messages.Split(":", 2).ToList();
                int from = int.Parse(strings[0]);
                string message = strings[1];

                createLabelMessage(from, message);
            }

        }
       
    }
}
