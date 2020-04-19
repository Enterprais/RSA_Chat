namespace RSA_Chat
{
    partial class MainForm
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
            this.textBox_message = new System.Windows.Forms.TextBox();
            this.button_send = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.listView_users = new System.Windows.Forms.ListView();
            this.richTextBox_mess = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label_user = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_message
            // 
            this.textBox_message.Location = new System.Drawing.Point(218, 531);
            this.textBox_message.MaxLength = 150;
            this.textBox_message.Name = "textBox_message";
            this.textBox_message.Size = new System.Drawing.Size(547, 20);
            this.textBox_message.TabIndex = 1;
            this.textBox_message.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_message_KeyPress);
            // 
            // button_send
            // 
            this.button_send.Location = new System.Drawing.Point(771, 528);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(75, 23);
            this.button_send.TabIndex = 3;
            this.button_send.Text = "Отправить";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.richTextBox_mess);
            this.panel1.Location = new System.Drawing.Point(218, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(628, 490);
            this.panel1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(62, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Пользователи";
            // 
            // listView_users
            // 
            this.listView_users.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listView_users.HideSelection = false;
            this.listView_users.Location = new System.Drawing.Point(12, 33);
            this.listView_users.MultiSelect = false;
            this.listView_users.Name = "listView_users";
            this.listView_users.Size = new System.Drawing.Size(200, 518);
            this.listView_users.TabIndex = 6;
            this.listView_users.UseCompatibleStateImageBehavior = false;
            this.listView_users.View = System.Windows.Forms.View.List;
            this.listView_users.SelectedIndexChanged += new System.EventHandler(this.listView_users_SelectedIndexChanged);
            // 
            // richTextBox_mess
            // 
            this.richTextBox_mess.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox_mess.Location = new System.Drawing.Point(-1, -1);
            this.richTextBox_mess.Name = "richTextBox_mess";
            this.richTextBox_mess.Size = new System.Drawing.Size(628, 492);
            this.richTextBox_mess.TabIndex = 0;
            this.richTextBox_mess.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(423, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Чат с:";
            // 
            // label_user
            // 
            this.label_user.AutoSize = true;
            this.label_user.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_user.Location = new System.Drawing.Point(477, 13);
            this.label_user.Name = "label_user";
            this.label_user.Size = new System.Drawing.Size(0, 17);
            this.label_user.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 563);
            this.Controls.Add(this.listView_users);
            this.Controls.Add(this.label_user);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.textBox_message);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximumSize = new System.Drawing.Size(874, 602);
            this.Name = "MainForm";
            this.Text = "RSA-ЧАТ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_message;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ListView listView_users;
        private System.Windows.Forms.RichTextBox richTextBox_mess;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_user;
    }
}