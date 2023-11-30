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
        public Chat(User thisUser)
        {
            InitializeComponent();
            this.thisUser = thisUser;
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

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
            int indexDelete = users.IndexOf(thisUser);
            users.RemoveAt(indexDelete);
            foreach (var user in users)
            {
                listUsers.Items.Add(user.Name);
            }
        }
    }
}
