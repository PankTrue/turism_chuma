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
        int brony_id, marshrut_id;
        public AddTurist(int brony_id,int marshrut)
        {
            this.marshrut_id = marshrut;
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

        private void button3_Click(object sender, EventArgs e)
        {
            printDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            GlobalVariables.connection.Open();
            OleDbCommand command = new OleDbCommand();
            command.Connection = GlobalVariables.connection;
            command.CommandText = $"SELECT * FROM Туристы WHERE [Код брони]={brony_id}";
            OleDbDataReader reader_turists = command.ExecuteReader();


            OleDbCommand command_1 = new OleDbCommand();
            command_1.Connection = GlobalVariables.connection;
            command_1.CommandText = $"SELECT * FROM Маршрут WHERE [Код маршрута]={marshrut_id}";
            OleDbDataReader reader_marshur = command_1.ExecuteReader();



            OleDbCommand command_3 = new OleDbCommand();
            command_3.Connection = GlobalVariables.connection;
            command_3.CommandText = $"SELECT * FROM Бронь WHERE [Код брони]={brony_id}";
            OleDbDataReader reader_bronya = command_3.ExecuteReader();

            string kod_kurorta="";
            if (reader_bronya.Read())
            {
                kod_kurorta = reader_bronya["Код курорта"].ToString();
            }


            OleDbCommand command_2 = new OleDbCommand();
            command_2.Connection = GlobalVariables.connection;
            command_2.CommandText = $"SELECT * FROM МаршрутКурорты WHERE [Код курорта]={kod_kurorta}";
            OleDbDataReader reader_marshrut_kurort = command_2.ExecuteReader();

            if(!reader_marshur.Read() || !reader_marshrut_kurort.Read())
            {
                return;
            }


            int child_count = 0, people_count = 0;

            TimeSpan current_age;


            while (reader_turists.Read())
            {
                current_age = DateTime.Now - DateTime.Parse(reader_turists["Дата рождения"].ToString());
                if ((current_age.TotalDays / 365) < 18)
                {
                    child_count++;
                }
                people_count++;
            }

            int value_chield = Convert.ToInt32(reader_marshrut_kurort["Стоимость детского билета"]), value_grown = Convert.ToInt32(reader_marshrut_kurort["Стоимость на  1 человека"]);

            e.Graphics.DrawString($"Кол-во людей: {people_count}\n" +
                                  $"Количество детей: {child_count}\n" +
                                  $"Сумма: {((child_count * value_chield ) + ((people_count - child_count) * value_grown))}\n" +
                                  $"Точка отправления: {reader_marshur["Точка отправления"]}\n" +
                                  $"Наличие медстраховки: {reader_marshur["Наличие медстраховки"]}\n" +
                                  $"трансфер: {reader_marshur["трансфер"]}\n" +
                                  $"Дополнительные услуги: {reader_marshur["Дополнительные услуги"]}\n" +
                                  $"тип переезда: {reader_marshur["тип переезда"]}\n" +
                                  //$"Нужна виза?: {reader_marshur["Нужна виза?"]}\n" +
                                  $"Название курорта: {reader_marshrut_kurort["Название курорта"]}\n" +
                                  $"Название отеля: {reader_marshrut_kurort["Название отеля"]}\n" +
                                  $"Название страны: {reader_marshrut_kurort["Название страны"]}\n" +
                                  $"описание курорта: {reader_marshrut_kurort["описание курорта"]}\n" +
                                  $"ДатаНачалаКурорта: {reader_marshrut_kurort["ДатаНачалаКурорта"]}\n",
                                  
                                  
                                  new Font("Arial", 14), Brushes.Black, 0, 0);



            GlobalVariables.connection.Close();
        }
    }
}
