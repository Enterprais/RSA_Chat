using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RSA_Chat
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button_Enter_Click(object sender, EventArgs e) //нажатие кнопки входа
        {
            if (textBox_Name.Text == "")
                return;
            MainForm mainForm = new MainForm(textBox_Name.Text); //создать окно чата
            mainForm.Owner = this;
            mainForm.Show();
            this.Hide();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            textBox_Name.Focus();
        }

        private void textBox_Name_KeyPress(object sender, KeyPressEventArgs e) //нажатие клавиши при фокусе на поле ввода логина
        {
            if (e.KeyChar == Convert.ToChar(Keys.Return))
                button_Enter_Click(new object(), new EventArgs());
        }
    }
}
