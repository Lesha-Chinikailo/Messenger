namespace SignIn
{
    partial class SignIn
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labEmail = new Label();
            txbEmail = new TextBox();
            btnSignIn = new Button();
            linkSignUp = new LinkLabel();
            txbPassword = new TextBox();
            labPassword = new Label();
            SuspendLayout();
            // 
            // labEmail
            // 
            labEmail.AutoSize = true;
            labEmail.Location = new Point(171, 130);
            labEmail.Name = "labEmail";
            labEmail.Size = new Size(54, 25);
            labEmail.TabIndex = 0;
            labEmail.Text = "Email";
            // 
            // txbEmail
            // 
            txbEmail.Location = new Point(264, 127);
            txbEmail.Name = "txbEmail";
            txbEmail.Size = new Size(261, 31);
            txbEmail.TabIndex = 1;
            // 
            // btnSignIn
            // 
            btnSignIn.Location = new Point(324, 351);
            btnSignIn.Name = "btnSignIn";
            btnSignIn.Size = new Size(112, 34);
            btnSignIn.TabIndex = 2;
            btnSignIn.Text = "Sign in";
            btnSignIn.UseVisualStyleBackColor = true;
            btnSignIn.Click += btnSignIn_Click;
            // 
            // linkSignUp
            // 
            linkSignUp.AutoSize = true;
            linkSignUp.Location = new Point(284, 431);
            linkSignUp.Name = "linkSignUp";
            linkSignUp.Size = new Size(197, 25);
            linkSignUp.TabIndex = 3;
            linkSignUp.TabStop = true;
            linkSignUp.Text = "Don't have an account?";
            linkSignUp.LinkClicked += linkSignUp_LinkClicked;
            // 
            // txbPassword
            // 
            txbPassword.Location = new Point(264, 204);
            txbPassword.Name = "txbPassword";
            txbPassword.Size = new Size(261, 31);
            txbPassword.TabIndex = 5;
            // 
            // labPassword
            // 
            labPassword.AutoSize = true;
            labPassword.Location = new Point(171, 207);
            labPassword.Name = "labPassword";
            labPassword.Size = new Size(87, 25);
            labPassword.TabIndex = 4;
            labPassword.Text = "Password";
            // 
            // SignIn
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(736, 476);
            Controls.Add(txbPassword);
            Controls.Add(labPassword);
            Controls.Add(linkSignUp);
            Controls.Add(btnSignIn);
            Controls.Add(txbEmail);
            Controls.Add(labEmail);
            Name = "SignIn";
            Text = "SignIn";
            FormClosed += SignIn_FormClosed;
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labEmail;
        private TextBox txbEmail;
        private Button btnSignIn;
        private LinkLabel linkSignUp;
        private TextBox txbPassword;
        private Label labPassword;
    }
}
