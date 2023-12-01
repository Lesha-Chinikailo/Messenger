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
        int indexSelectedUser;
        int messageDepth;
        public Chat(User thisUser)
        {
            InitializeComponent();
            this.thisUser = thisUser;
            thisUser.IsOnline = true;
            indexSelectedUser = -1;
            messageDepth = 50;
            this.Text += $" ({thisUser.Name})";
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
            DisplayUsers();
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
            rtbMessages.Text = "";
            foreach (var fromMessage in ourMessage.messages)
            {
                List<string> strings = fromMessage.Split(":", 2).ToList();
                int from = int.Parse(strings[0]);
                string message = strings[1];

                Label labelMessage = new Label
                {
                    Text = from == thisUser.Id ? $"I:{message}" : $"{companion.Name}:{message}",
                    //Size = new Size(150, message.Length * 3),
                    BorderStyle = BorderStyle.Fixed3D,
                    AutoSize = true,
                    //Dock = from == thisUser.Id ? DockStyle.Left : DockStyle.Right,
                    //Location = new Point(splitContainer1.Panel2.Width - Width, messageDepth)
                };
                if (from == thisUser.Id)
                    labelMessage.Location = new Point(splitContainer1.Panel2.Width - labelMessage.Width - 50, messageDepth);
                else
                    labelMessage.Location = new Point(10, messageDepth);
                messageDepth += (labelMessage.Height + 10);
                splitContainer1.Panel2.Controls.Add(labelMessage);
                //Location = new Point(splitContainer1.Panel2.Width - Width, messageDepth)
                rtbMessages.Text += message;
            }

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

        private void Chat_FormClosed(object sender, FormClosedEventArgs e)
        {
            FirebaseResponse response = _client.Get("Users/");
            users = response.ResultAs<List<User>>();
            //users.IndexOf(thisUser);
            users[users.IndexOf(thisUser)].IsOnline = false;
            users[users.IndexOf(thisUser)].DateTime = DateTime.Now;
            _client.Set("Users/", users);
        }
    }
}
