using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace WindowsFormsApp1
{
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                GlobalVariables.connection.Open();
                GlobalVariables.connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error  " + ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GlobalVariables.connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = GlobalVariables.connection;
                command.CommandText = $"SELECT * FROM Сотрудники WHERE Логин='{textBox1.Text}' and Пароль='{textBox2.Text}'";
                OleDbDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                GlobalVariables.current_user = Convert.ToInt32(reader["Код сотрудника"]);
                switch (reader["Группа"].ToString())
                {
                    case "Менеджер" : GlobalVariables.mainForm = new ManagerPanel(); break;  
                    case "Администратор" : GlobalVariables.mainForm = new AdminPanel(); break;
                    default: MessageBox.Show("Группа не найдена"); GlobalVariables.connection.Close(); return;
                }
                MessageBox.Show("Добро пожаловать");
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
            GlobalVariables.connection.Close();
        }
    }
}
