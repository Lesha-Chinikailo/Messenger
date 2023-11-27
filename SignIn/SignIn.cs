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
    public partial class SignIn : Form
    {
        private IFirebaseConfig _config = new FirebaseConfig()
        {
            AuthSecret = "yfvpDIIYfiM64RZYxlaTr7sQ0shK88Sm8Qany2EZ",
            BasePath = "https://messenger-11e03-default-rtdb.firebaseio.com/",
        };
        private IFirebaseClient _client;
        public SignIn()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
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

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbEmail.Text) && string.IsNullOrEmpty(txbPassword.Text))
            {
                MessageBox.Show("enter all date");
                return;
            }
            FirebaseResponse response = _client.Get("Users/");
            if (response.Body == "null")
                goto end;

            List<User> users = response.ResultAs<List<User>>();

            foreach (var user in users)
            {
                if (txbEmail.Text == user.Email)
                {
                    if (txbPassword.Text == user.Password)
                    {
                        MessageBox.Show("Welcome " + user.Name);

                        return;
                    }
                }
            }
        end:
            MessageBox.Show("you don't have an account");
        }

        private void linkSignUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Thread(() => Application.Run(new Registration())).Start();
            this.Close();

            //Registration registration = new Registration();
            //this.Hide();
            //registration.ShowDialog();
        }

        private void SignIn_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
