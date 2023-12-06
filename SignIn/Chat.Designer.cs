namespace SignIn
{
    partial class Chat
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
            splitContainer1 = new SplitContainer();
            listUsers = new ListBox();
            panelMessages = new Panel();
            labLastTime = new Label();
            labUserName = new Label();
            btnSend = new Button();
            txbMessage = new TextBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(listUsers);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panelMessages);
            splitContainer1.Panel2.Controls.Add(labLastTime);
            splitContainer1.Panel2.Controls.Add(labUserName);
            splitContainer1.Panel2.Controls.Add(btnSend);
            splitContainer1.Panel2.Controls.Add(txbMessage);
            splitContainer1.Size = new Size(891, 537);
            splitContainer1.SplitterDistance = 203;
            splitContainer1.TabIndex = 0;
            // 
            // listUsers
            // 
            listUsers.Dock = DockStyle.Top;
            listUsers.FormattingEnabled = true;
            listUsers.ItemHeight = 25;
            listUsers.Location = new Point(0, 0);
            listUsers.Name = "listUsers";
            listUsers.Size = new Size(203, 379);
            listUsers.TabIndex = 0;
            listUsers.SelectedIndexChanged += listUsers_SelectedIndexChanged;
            // 
            // panelMessages
            // 
            panelMessages.AutoScroll = true;
            panelMessages.Location = new Point(12, 54);
            panelMessages.Name = "panelMessages";
            panelMessages.Size = new Size(660, 423);
            panelMessages.TabIndex = 5;
            // 
            // labLastTime
            // 
            labLastTime.AutoSize = true;
            labLastTime.Font = new Font("Segoe UI", 6F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labLastTime.Location = new Point(6, 25);
            labLastTime.Name = "labLastTime";
            labLastTime.Size = new Size(57, 15);
            labLastTime.TabIndex = 4;
            labLastTime.Text = "Last Time";
            labLastTime.Visible = false;
            // 
            // labUserName
            // 
            labUserName.AutoSize = true;
            labUserName.Location = new Point(3, 3);
            labUserName.Name = "labUserName";
            labUserName.Size = new Size(94, 25);
            labUserName.TabIndex = 3;
            labUserName.Text = "UserName";
            labUserName.Visible = false;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(560, 483);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(112, 34);
            btnSend.TabIndex = 1;
            btnSend.Text = "send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // txbMessage
            // 
            txbMessage.Location = new Point(12, 485);
            txbMessage.Name = "txbMessage";
            txbMessage.Size = new Size(536, 31);
            txbMessage.TabIndex = 0;
            // 
            // Chat
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(891, 537);
            Controls.Add(splitContainer1);
            Name = "Chat";
            Text = "Chat";
            FormClosed += Chat_FormClosed;
            Load += Chat_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private ListBox listUsers;
        private Button btnSend;
        private TextBox txbMessage;
        private Label labLastTime;
        private Label labUserName;
        private Panel panelMessages;
    }
}