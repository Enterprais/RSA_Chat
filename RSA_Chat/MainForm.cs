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
        ChatControl ChatControl; //объект управления чатом

        public delegate void DUpdateUser(); //делегат обновления списка пользователей
        public delegate void DUpdateSession(string idx); //делегат обновления окна чата
        public DUpdateUser UpdateUsers; //объект делегата обновления списка пользователей
        public DUpdateSession UpdateSession; //объект делегата обовления окна чата
        public int CurrentSession { private set; get; } //индекс выбранного пользователя 

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

        public void ClearSession() //сброс индекса выбранного пользователя
        {
            CurrentSession = -1;
        }

        private void button_send_Click(object sender, EventArgs e) //нажатие клавиши отправить сообщение 
        {
            if (textBox_message.Text == "" || CurrentSession == -1) //если сообщениене пустое или не выбран пользователь
                return;
            ChatControl.SendMessage(textBox_message.Text, CurrentSession); //отправить сообщение
            ChangeUserSession(CurrentSession.ToString()); //обновить окно чата
            textBox_message.Clear();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) //закрытие окна чата
        {
            ChatControl.SendExitAlert(); //отправить оповещение о выходе
            Application.Exit();
        }

        private void listView_users_SelectedIndexChanged(object sender, EventArgs e) //смена чата пользователя
        {
            if (listView_users.SelectedItems.Count > 0)              
                ChangeUserSession(listView_users.SelectedItems[0].SubItems[1].Text);
        }

        public void ChangeUserSession(string index) //обновление окна с сообщениями
        {
            int idx = int.Parse(index);
            CurrentSession = idx;
            richTextBox_mess.Clear();
            foreach (Session.MesField mes in ChatControl.UsersList[idx].Session.Messages) //проход по всем сообщениям пользвоателя
            {
                richTextBox_mess.Text += mes.user + ": ";
                richTextBox_mess.Text += mes.mes + "\n";
            }
            label_user.Text = ChatControl.UsersList[idx].Name;
            ChatControl.UsersList[idx].Session.ReadMessage(); //сбросить счетчик непрочитанных сообщений
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

        private void textBox_message_KeyPress(object sender, KeyPressEventArgs e) //нажатие клавиши при фокусе на строкесообщения
        {
            if (e.KeyChar == Convert.ToChar(Keys.Return)) //если нажат "enter"
                button_send_Click(new object(), new EventArgs()); //симулировать нажатие кнопки "отправить"
        }

        private void richTextBox_mess_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
