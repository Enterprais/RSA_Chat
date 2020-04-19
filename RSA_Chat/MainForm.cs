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

        public delegate void DUpdateUser();
        public delegate void DUpdateSession(string idx);
        public DUpdateUser UpdateUsers;
        public DUpdateSession UpdateSession;
        public int CurrentSession { private set; get; }


        public MainForm(string text)
        {
            ChatControl = new ChatControl(this);
            InitializeComponent();
            ChatControl.MyName = text;
            CurrentSession = -1;
            ChatControl.SendEnterAlert();

            UpdateUsers = UpdateUserList;
            UpdateSession = ChangeUserSession;
        }

        public void ClearSession()
        {
            CurrentSession = -1;
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            if (textBox_message.Text == "" || CurrentSession == -1)
                return;
            ChatControl.SendMessage(textBox_message.Text, CurrentSession);
            ChangeUserSession(CurrentSession.ToString());
            textBox_message.Clear();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ChatControl.SendExitAlert();
            Application.Exit();
        }

        private void listView_users_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_users.SelectedItems.Count > 0)              
                ChangeUserSession(listView_users.SelectedItems[0].SubItems[1].Text);
        }

        public void ChangeUserSession(string index)
        {
            int idx = int.Parse(index);
            CurrentSession = idx;
            richTextBox_mess.Clear();
            foreach (Session.MesField mes in ChatControl.UsersList[idx].Session.Messages)
            {
                richTextBox_mess.Text += mes.user + ": ";
                richTextBox_mess.Text += mes.mes + "\n";
            }
            label_user.Text = ChatControl.UsersList[idx].Name;
            ChatControl.UsersList[idx].Session.ReadMessage();
            UpdateUserList();
        }

        public void UpdateUserList() //обновление списка пользователей на экране
        {
            listView_users.Clear();

            foreach (User item in ChatControl.UsersList)
            {
                ListViewItem view = new ListViewItem();

                view.Text = item.Name + "(" + item.Session.UnreadMes + ")";
                view.SubItems.Add(ChatControl.UsersList.IndexOf(item).ToString());
                listView_users.Items.Add(view);
            }
        }

        private void textBox_message_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Return))
                button_send_Click(new object(), new EventArgs());
        }

        private void richTextBox_mess_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
