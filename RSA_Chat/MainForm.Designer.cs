﻿namespace RSA_Chat
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
            this.listBox_users = new System.Windows.Forms.ListBox();
            this.button_send = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.richTextBox_mess = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_message
            // 
            this.textBox_message.Location = new System.Drawing.Point(218, 529);
            this.textBox_message.Name = "textBox_message";
            this.textBox_message.Size = new System.Drawing.Size(547, 20);
            this.textBox_message.TabIndex = 1;
            // 
            // listBox_users
            // 
            this.listBox_users.FormattingEnabled = true;
            this.listBox_users.Location = new System.Drawing.Point(12, 12);
            this.listBox_users.Name = "listBox_users";
            this.listBox_users.Size = new System.Drawing.Size(200, 537);
            this.listBox_users.TabIndex = 2;
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
            this.panel1.Location = new System.Drawing.Point(218, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(628, 511);
            this.panel1.TabIndex = 4;
            // 
            // richTextBox_mess
            // 
            this.richTextBox_mess.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox_mess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_mess.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_mess.Name = "richTextBox_mess";
            this.richTextBox_mess.ReadOnly = true;
            this.richTextBox_mess.Size = new System.Drawing.Size(626, 509);
            this.richTextBox_mess.TabIndex = 0;
            this.richTextBox_mess.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 563);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.listBox_users);
            this.Controls.Add(this.textBox_message);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_message;
        private System.Windows.Forms.ListBox listBox_users;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.RichTextBox richTextBox_mess;
    }
}