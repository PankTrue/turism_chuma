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
            string insertStatement = $"INSERT INTO Туристы ([Код брони],[Фамилия],[Имя],[Отчество],[Номер телефона],[Загранный паспорт код],[Виза номер],[Адрес эл почты],[Пол],[Дата рождения])" +
                                                 $" VALUES ({brony_id},'{textBox2.Text}','{textBox3.Text}','{textBox4.Text}','{textBox5.Text}','{textBox6.Text}','{textBox7.Text}','{textBox8.Text}','{comboBox1.Text}','{dateTimePicker2.Value.ToString("yyyy-MM-dd hh:mm:ss")}')";

            OleDbCommand insertCommand = new OleDbCommand(insertStatement, GlobalVariables.connection);
            GlobalVariables.connection.Open();
            try
            {
                insertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                GlobalVariables.connection.Close();
                foreach (Control x in this.Controls)
                {
                    if (x is TextBox)
                    {
                        ((TextBox)x).Text = String.Empty;
                    }
                }
                datagrid_with_turists_update();
            }
        }



        void datagrid_with_turists_update()
        {
            string strSql = $"SELECT * FROM Туристы WHERE [Код брони]={brony_id}";
            OleDbCommand cmd = new OleDbCommand(strSql, GlobalVariables.connection);
            GlobalVariables.connection.Open();
                cmd.CommandType = CommandType.Text;
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataTable scores = new DataTable();
                da.Fill(scores);
                dataGridView1.DataSource = scores;
            GlobalVariables.connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var form = new FinnalyReservation(this.brony_id);
            form.Show();
        }
    }
}
