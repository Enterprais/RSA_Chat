using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace RSA_Chat
{
    public partial class MainForm : Form
    {
        ChatControl ChatControl;

        public MainForm(string text)
        {
            InitializeComponent();
            ChatControl = new ChatControl(this);
            ChatControl.MyName = text;
            richTextBox_mess.Text = ChatControl.MyIp.ToString();
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            ChatControl.SendEnterAlert();
            if (textBox_message.Text == "")
                return;

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Owner.Close();
        }
    }
}
