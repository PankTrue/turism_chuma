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
    public partial class Reservation : Form
    {
        private int IdKurort;
        public Reservation(int IdKurort)
        {
            this.IdKurort = IdKurort;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //insert data in Marshrut
            string insertStatementForMarshrut = $"INSERT INTO Маршрут ([Точка отправления],[Дополнительные услуги],[тип переезда],[Наличие медстраховки],[трансфер])" +
                                                    $" VALUES ('{textBox1.Text}','{textBox2.Text}','{textBox3.Text}',{checkBox1.Checked},{checkBox2.Checked})";

            OleDbCommand insertCommand_0 = new OleDbCommand(insertStatementForMarshrut, GlobalVariables.connection);
            GlobalVariables.connection.Open();
            try
            {
                insertCommand_0.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
                GlobalVariables.connection.Close();
            }


            //get id marshrut
            OleDbCommand command = new OleDbCommand();
            command.Connection = GlobalVariables.connection;
            command.CommandText = $"SELECT * FROM Маршрут WHERE [Точка отправления]='{textBox1.Text}' and [Дополнительные услуги]='{textBox2.Text}' and [тип переезда]='{textBox3.Text}' and [Наличие медстраховки]={checkBox1.Checked} and [трансфер]={checkBox2.Checked}";
            OleDbDataReader reader_0 = command.ExecuteReader();
            if (!reader_0.Read())
            {
                MessageBox.Show("Не удалось получить Код маршрута");
                GlobalVariables.connection.Close();
                return;
            }
            string id_marshrut = reader_0["Код маршрута"].ToString();


            insertStatementForMarshrut = $"INSERT INTO Бронь ([Код маршрута],[Код сотрудника],[Дата резервирования],[Нужна виза?],[Код курорта])" +
                                                 $" VALUES ('{id_marshrut}','{GlobalVariables.current_user}','{DateTime.Now}',{checkBox3.Checked},'{IdKurort}')";

            OleDbCommand insertCommand_1 = new OleDbCommand(insertStatementForMarshrut, GlobalVariables.connection);
            try
            {
                insertCommand_1.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
                GlobalVariables.connection.Close();
            }

            OleDbCommand command_1 = new OleDbCommand();
            command_1.Connection = GlobalVariables.connection;
            command_1.CommandText = $"SELECT * FROM Бронь WHERE [Код маршрута]={id_marshrut} and [Код сотрудника]={GlobalVariables.current_user} and [Нужна виза?]={checkBox3.Checked} and [Код курорта]={IdKurort}";
            OleDbDataReader reader_1 = command_1.ExecuteReader();
            if (!reader_1.Read())
            {
                MessageBox.Show("Не удалось получить Код маршрута");
                GlobalVariables.connection.Close();
                return;
            }
            int bronya_id = Convert.ToInt32(reader_1["Код брони"]);
            var form = new AddTurist(bronya_id);
            form.Show();
            

            GlobalVariables.connection.Close();
            
        }
    }
}
