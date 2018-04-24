using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class AddTurist : Form
    {
        int brony_id;
        public AddTurist(int brony_id)
        {
            this.brony_id = brony_id;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string insertStatement = $"INSERT INTO Бронь ([Код брони],[Фамилия],[Имя],[Отчество],[Номер телефона],[Загранный паспорт код],[Виза номер],[Адрес эл почты])" +
                                                 $" VALUES ({brony_id},'{textBox2.Text}','{textBox3.Text}',{textBox4.Text},'{textBox5.Text}','{textBox6.Text}','{textBox7.Text}','{textBox8.Text}')";

            OleDbCommand insertCommand = new OleDbCommand(insertStatement, GlobalVariables.connection);
            GlobalVariables.connection.Open();
            try
            {
                insertCommand.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                GlobalVariables.connection.Close();
            }
        }
    }
}
