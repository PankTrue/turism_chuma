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
    public partial class FinnalyReservation : Form
    {
        private int bronya_id;
        public FinnalyReservation(int bronya_id)
        {
            this.bronya_id = bronya_id;
            InitializeComponent();
        }

        private void FinnalyReservation_Load(object sender, EventArgs e)
        {
            GlobalVariables.connection.Open();
            OleDbCommand command = new OleDbCommand();
            command.Connection = GlobalVariables.connection;
            command.CommandText = $"SELECT * FROM Туристы WHERE [Код брони]={bronya_id}";
            OleDbDataReader reader = command.ExecuteReader();

            int child_count = 0, people_count = 0;

            TimeSpan current_age;


            while (reader.Read())
            {
                current_age = DateTime.Now - DateTime.Parse(reader["Дата рождения"].ToString());
                if ((current_age.TotalDays / 365) < 18)
                {
                    child_count++;
                }
                people_count++;
            }

            int value_chield = 1337, value_grown = 1488; //TODO: take from datebase

            label8.Text = people_count.ToString();
            label9.Text = child_count.ToString();
            label10.Text = (people_count - child_count).ToString();
            label11.Text = ((child_count * value_chield) + ((people_count - child_count) * value_grown)).ToString();


            GlobalVariables.connection.Close();
        }
    }
}
