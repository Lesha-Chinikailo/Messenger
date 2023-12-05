using System;
using System.Collections.Generic;
using System.ComponentModel;
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

using Google.Cloud.Firestore;

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
        User thisUser;
        User companion;
        ChatMessages ourMessage;
        string FromTo;
        string thisConnectionId;
        int indexSelectedUser;
        int messageDepth;

        HubConnection connection;

        FirestoreDb database;
        public Chat(User thisUser)
        {
            InitializeComponent();
            this.thisUser = thisUser;
            thisUser.IsOnline = true;
            indexSelectedUser = -1;
            messageDepth = 50;
            this.Text += $" ({thisUser.Name})";

            connection = new HubConnectionBuilder()
            .WithUrl("http://192.168.56.1:5153/chat")
            .Build();
        }
        private void listUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listUsers.SelectedIndex == indexSelectedUser)
                return;
            indexSelectedUser = listUsers.SelectedIndex;
            companion = users[indexSelectedUser];
            DisplayUserMessages(users[indexSelectedUser]);
            labLastTime.Visible = true;
            labUserName.Visible = true;
            labUserName.Text = users[indexSelectedUser].Name;
            labLastTime.Text = users[indexSelectedUser].IsOnline ? "online" : users[indexSelectedUser].DateTime.ToString(); 
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
            //database = FirestoreDb.Create();
        }

        private async void InitializeSignalR()
        {
            connection.On<int, string>("Receive", (senderUserId, message) =>
            {
                Invoke((Action)(() =>
                {
                    if (senderUserId != -1)
                        MessageBox.Show($"{senderUserId}: {message}");
                    else
                    {
                        int receiverID = int.Parse(message);
                        List<User> user = users.Where(u => u.Id == receiverID).ToList();
                        if(user.Count != 0)
                        {
                            user[0].IsOnline = true;
                            if (user[0].Name == labUserName.Text)
                                labLastTime.Text = "online";
                        }
                        
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
                    if(response.Body != "null")
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
                //usersId.Add($"{thisUser.Name}:{thisUser.Id}", connectionId);
                //await _client.SetAsync("WhoIsOnline/", usersId);
            }
            else
            {
                MessageBox.Show("what don't work");
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
            //splitContainer1.Panel2.Controls.Clear();
            FirebaseResponse response = _client.Get("Chat/");
            if (response.Body == "null")
            {
                MessageBox.Show("nobody don't use this Chat");
                return;
            }

            Dictionary<string, ChatMessages> chatMessages = response.ResultAs<Dictionary<string, ChatMessages>>();

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

        private void createLabelMessage(int fromId,  string message)
        {
            Label labelMessage = new Label
            {
                Text = fromId == thisUser.Id ? $"I:{message}" : $"{companion.Name}:{message}",
                Size = new Size(225, (message.Length < 15) ? 30 : (message.Length < 55 ? message.Length / 15 * 30 : (message.Length < 100 ? message.Length / 20 * 30 : message.Length / 25 * 30))),
                BorderStyle = BorderStyle.Fixed3D,
                //AutoSize = true,
                //Dock = from == thisUser.Id ? DockStyle.Left : DockStyle.Right,
                //Location = new Point(splitContainer1.Panel2.Width - Width, messageDepth)
            };
            if (fromId == thisUser.Id)
                labelMessage.Location = new Point(splitContainer1.Panel2.Width - labelMessage.Width - 100, messageDepth);
            else
                labelMessage.Location = new Point(10, messageDepth);
            messageDepth += (labelMessage.Height + 10);
            panelMessages.Controls.Add(labelMessage);
            //Location = new Point(splitContainer1.Panel2.Width - Width, messageDepth)
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

            Dictionary<string, ChatMessages> chat = new Dictionary<string, ChatMessages>
            {
                { $"{ourMessage.user1ID}:{ourMessage.user2ID}", ourMessage }
            };
            await _client.SetAsync("Chat/", chat);
            txbMessage.Text = string.Empty;
        }

        async private void SendMessageToOne(int receiverId, string message)
        {
            FirebaseResponse response = _client.Get($"Users/{receiverId}");
            User user = response.ResultAs<User>();
            string userConnectionId = _client.Get($"WhoIsOnline/{user.Name}:{user.Id}").ResultAs<string>();
            await connection.InvokeAsync("SendToUser", thisUser.Id, userConnectionId, message);
        }

        private void Chat_FormClosed(object sender, FormClosedEventArgs e)
        {
            FirebaseResponse response = _client.Get("Users/");
            users = response.ResultAs<List<User>>();
            //users.IndexOf(thisUser);
            users[users.IndexOf(thisUser)].IsOnline = false;
            users[users.IndexOf(thisUser)].DateTime = DateTime.Now;
            _client.Set("Users/", users);

            DeleteConnectionId();
        }

        async private void DeleteConnectionId()
        {
            Dictionary<string, string> usersId;
            FirebaseResponse response = _client.Get("WhoIsOnline/");

            usersId = response.ResultAs<Dictionary<string, string>>();
            usersId[$"{thisUser.Name}:{thisUser.Id}"] = "null";
            await _client.UpdateAsync("WhoIsOnline/", usersId);

            //await _client.UpdateAsync($"WhoIsOnline/{thisUser.Name}:{thisUser.Id}", "null");

        }
    }
}
