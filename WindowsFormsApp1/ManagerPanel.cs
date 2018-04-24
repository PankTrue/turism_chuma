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
    public partial class ManagerPanel : Form
    {
        public ManagerPanel()
        {
            InitializeComponent();
        }

        private void ManagerPanel_Load(object sender, EventArgs e)
        {
            dataGrid_Update();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string strSql = $"SELECT * FROM МаршрутКурорты WHERE ДатаНачалаКурорта between #{dateTimePicker1.Value.ToString("yyyy'/'MM'/'dd")}# and #{dateTimePicker2.Value.ToString("yyyy'/'MM'/'dd")}#";
            if(textBox1.Text != "")
                strSql += $" AND [Название страны] = '{textBox1.Text}'";
            OleDbCommand cmd = new OleDbCommand(strSql, GlobalVariables.connection);

            GlobalVariables.connection.Open();
            cmd.CommandType = CommandType.Text;
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable scores = new DataTable();
            da.Fill(scores);
            dataGridView1.DataSource = scores;
            GlobalVariables.connection.Close();
        }


        private void dataGrid_Update()
        {
            string strSql = $"SELECT * FROM МаршрутКурорты";
            OleDbCommand cmd = new OleDbCommand(strSql, GlobalVariables.connection);

            GlobalVariables.connection.Open();
            cmd.CommandType = CommandType.Text;
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable scores = new DataTable();
            da.Fill(scores);
            dataGridView1.DataSource = scores;
            GlobalVariables.connection.Close();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        { 
            Form form = new Reservation(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()));
            form.Show();
        }
    }
}
