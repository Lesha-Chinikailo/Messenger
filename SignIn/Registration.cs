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
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }

        private IFirebaseConfig _config = new FirebaseConfig()
        {
            AuthSecret = "yfvpDIIYfiM64RZYxlaTr7sQ0shK88Sm8Qany2EZ",
            BasePath = "https://messenger-11e03-default-rtdb.firebaseio.com/",
        };
        private IFirebaseClient _client;
        List<User> users;

        private void Registration_Load(object sender, EventArgs e)
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
            }
        }

        async private void btnSignUp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbEmail.Text) && string.IsNullOrEmpty(txbPassword.Text) && string.IsNullOrEmpty(txbName.Text))
            {
                MessageBox.Show("enter all date");
                return;
            }

            FirebaseResponse response = _client.Get("Users/");
            int lastIdUser = 0;
            if (response.Body == "null")
            {
                users = new List<User>();
                lastIdUser = -1;
                goto createUser;
            }
            users = response.ResultAs<List<User>>();

            foreach (var user in users)
            {
                if (txbEmail.Text == user.Email)
                {
                    MessageBox.Show("You already have an account");
                    return;
                }
            }
            lastIdUser = users.Last().Id;
        createUser:

            User newUser = new User()
            {
                Id = lastIdUser + 1,
                Name = txbName.Text,
                Email = txbEmail.Text,
                Password = txbPassword.Text
            };
            users.Add(newUser);
            await _client.SetAsync("Users/", users);

            txbEmail.Text = string.Empty;
            txbPassword.Text = string.Empty;
            txbName.Text = string.Empty;
        }


        private void linkHaveAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //new Thread(() => Application.Run(new SignIn())).Start();
            this.Close();
        }

        private void Registration_FormClosed(object sender, FormClosedEventArgs e)
        {
            SignIn signIn = new SignIn();
            this.Hide();
            signIn.ShowDialog();
        }
    }
}
