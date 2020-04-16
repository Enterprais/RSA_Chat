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

        private void button_Enter_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm(textBox_Name.Text);
            mainForm.Owner = this;
            mainForm.Show();
            this.Hide();
        }
    }
}
