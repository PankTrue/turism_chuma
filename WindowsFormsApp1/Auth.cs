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
            Form panel = new Form();

            if (reader.Read())
            {
                GlobalVariables.current_user = Convert.ToInt32(reader["Код сотрудника"]);
                switch (reader["Группа"].ToString())
                {
                    case "Менеджер" : panel = new ManagerPanel(); break;  
                    case "Администратор" : panel = new AdminPanel(); break;
                    default: MessageBox.Show("Группа не найдена"); GlobalVariables.connection.Close(); return;
                }

             MessageBox.Show("Добро пожаловать");
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
            GlobalVariables.connection.Close();
            panel.Show();
        }
    }
}
