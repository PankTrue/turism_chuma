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

            int child_count, people_count = 0;
            DateTime current_age;

            while (reader.Read())
            {
                //current_age = DateTime.Now - DateTime.Parse(reader["Дата рождения"].ToString());
                people_count++;
            }

            label8.Text = people_count.ToString();


            GlobalVariables.connection.Close();
        }
    }
}
