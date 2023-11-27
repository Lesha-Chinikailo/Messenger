namespace SignIn
{
    partial class Registration
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labName = new Label();
            txbName = new TextBox();
            btnSignUp = new Button();
            linkHaveAccount = new LinkLabel();
            txbEmail = new TextBox();
            labEmail = new Label();
            txbPassword = new TextBox();
            labPassword = new Label();
            SuspendLayout();
            // 
            // labName
            // 
            labName.AutoSize = true;
            labName.Location = new Point(194, 118);
            labName.Name = "labName";
            labName.Size = new Size(56, 25);
            labName.TabIndex = 0;
            labName.Text = "name";
            // 
            // txbName
            // 
            txbName.Location = new Point(289, 118);
            txbName.Name = "txbName";
            txbName.Size = new Size(210, 31);
            txbName.TabIndex = 1;
            // 
            // btnSignUp
            // 
            btnSignUp.Location = new Point(318, 363);
            btnSignUp.Name = "btnSignUp";
            btnSignUp.Size = new Size(112, 34);
            btnSignUp.TabIndex = 2;
            btnSignUp.Text = "Sign up";
            btnSignUp.UseVisualStyleBackColor = true;
            btnSignUp.Click += btnSignUp_Click;
            // 
            // linkHaveAccount
            // 
            linkHaveAccount.AutoSize = true;
            linkHaveAccount.Location = new Point(289, 442);
            linkHaveAccount.Name = "linkHaveAccount";
            linkHaveAccount.Size = new Size(188, 25);
            linkHaveAccount.TabIndex = 3;
            linkHaveAccount.TabStop = true;
            linkHaveAccount.Text = "I already have account";
            linkHaveAccount.LinkClicked += linkHaveAccount_LinkClicked;
            // 
            // txbEmail
            // 
            txbEmail.Location = new Point(289, 172);
            txbEmail.Name = "txbEmail";
            txbEmail.Size = new Size(210, 31);
            txbEmail.TabIndex = 5;
            // 
            // labEmail
            // 
            labEmail.AutoSize = true;
            labEmail.Location = new Point(194, 172);
            labEmail.Name = "labEmail";
            labEmail.Size = new Size(54, 25);
            labEmail.TabIndex = 4;
            labEmail.Text = "email";
            // 
            // txbPassword
            // 
            txbPassword.Location = new Point(289, 226);
            txbPassword.Name = "txbPassword";
            txbPassword.Size = new Size(210, 31);
            txbPassword.TabIndex = 7;
            // 
            // labPassword
            // 
            labPassword.AutoSize = true;
            labPassword.Location = new Point(194, 229);
            labPassword.Name = "labPassword";
            labPassword.Size = new Size(89, 25);
            labPassword.TabIndex = 6;
            labPassword.Text = "password";
            // 
            // Registration
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(736, 476);
            Controls.Add(txbPassword);
            Controls.Add(labPassword);
            Controls.Add(txbEmail);
            Controls.Add(labEmail);
            Controls.Add(linkHaveAccount);
            Controls.Add(btnSignUp);
            Controls.Add(txbName);
            Controls.Add(labName);
            Name = "Registration";
            Text = "Registration";
            FormClosed += Registration_FormClosed;
            Load += Registration_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labName;
        private TextBox txbName;
        private Button btnSignUp;
        private LinkLabel linkHaveAccount;
        private TextBox txbEmail;
        private Label labEmail;
        private TextBox txbPassword;
        private Label labPassword;
    }
}