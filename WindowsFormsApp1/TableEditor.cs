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
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class TableEditor : Form
    {
        List<Label> lables = new List<Label>();
        List<TextBox> textboxes = new List<TextBox>();

        private string TableName;

        public TableEditor(string TableName)
        {
            this.TableName = TableName;
            InitializeComponent();
        }

        //events
        private void TableEditor_Load(object sender, EventArgs e)
        {
            dataGridView1_Update();

            int y = 28;
            int h = 25;
            foreach (string name in GetTableColumnNames(TableName))
            {
                var label_tmp = new Label();

                label_tmp.AutoSize = true;
                label_tmp.Location = new System.Drawing.Point(12, y);
                label_tmp.Name = name;
                label_tmp.Size = new System.Drawing.Size(35, 13);
                label_tmp.TabIndex = 0;
                label_tmp.Text = name;

                var textBox_tmp = new TextBox();

                textBox_tmp.Location = new System.Drawing.Point(180, y);
                textBox_tmp.Name = name;
                textBox_tmp.Size = new System.Drawing.Size(100, 20);
                textBox_tmp.TabIndex = 1;


                y += h;

                this.Controls.Add(label_tmp);
                this.Controls.Add(textBox_tmp);


                lables.Add(label_tmp);
                textboxes.Add(textBox_tmp);
                textboxes[0].Enabled = false;
            }
        }
         
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try {

                for (int i = 0; i < textboxes.Count(); i++)
                {
                    textboxes[i].Text = dataGridView1.Rows[e.RowIndex].Cells[i].Value.ToString();
                }

            } catch { }
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            string insertStatement = $"INSERT INTO {TableName} (";

            foreach (var data in lables.Skip(1))
                insertStatement += "[" + data.Text + "],";
            insertStatement = insertStatement.Remove(insertStatement.Count() - 1);
            insertStatement += ") VALUES (";

            foreach (var data in textboxes.Skip(1))
            {
                if (data.Text.Length == 0 )
                {
                    MessageBox.Show("Одно из полей пусто");
                    return;
                }
                insertStatement += "'" + data.Text + "',";
            }
            insertStatement =  insertStatement.Remove(insertStatement.Count() - 1);
            insertStatement += ")";
            
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
                dataGridView1_Update();
                foreach (var data in textboxes)
                    data.Text = "";
            }
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            string updateStatement = $"UPDATE {TableName} SET ";
            
            for (int i = 1; i < lables.Count(); i++)
                updateStatement += $"[{lables[i].Text}]='{textboxes[i].Text}',";

            updateStatement = updateStatement.Remove(updateStatement.Count() - 1);
            updateStatement += $" WHERE [{lables[0].Text}]={textboxes[0].Text}";

            OleDbCommand insertCommand = new OleDbCommand(updateStatement, GlobalVariables.connection);
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
                dataGridView1_Update();
            }
        }

        private void button_remove_Click(object sender, EventArgs e)
        {
            string deleteStatement = $"DELETE FROM {TableName} WHERE [{lables[0].Text}]={textboxes[0].Text}";
            

            OleDbCommand insertCommand = new OleDbCommand(deleteStatement, GlobalVariables.connection);
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
                dataGridView1_Update();
            }
        }
        //other
        private IEnumerable<string> GetTableColumnNames(string tableName)
        {
            var result = new List<string>();
            // using (var sqlCon = new SqlConnection(conStr))
            // {
            GlobalVariables.connection.Open();
            var sqlCmd = GlobalVariables.connection.CreateCommand();
            sqlCmd.CommandText = "select * from " + tableName + " where 1=0";  // No data wanted, only schema
            sqlCmd.CommandType = CommandType.Text;

            var sqlDR = sqlCmd.ExecuteReader();
            var dataTable = sqlDR.GetSchemaTable();

            foreach (DataRow row in dataTable.Rows) result.Add(row.Field<string>("ColumnName"));
            GlobalVariables.connection.Close();
            // }

            return result;
        }

        private void dataGridView1_Update()
        {
            string strSql = $"SELECT * FROM {TableName}";
            OleDbCommand cmd = new OleDbCommand(strSql, GlobalVariables.connection);

            GlobalVariables.connection.Open();
                cmd.CommandType = CommandType.Text;
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataTable scores = new DataTable();
                da.Fill(scores);
                dataGridView1.DataSource = scores;
            GlobalVariables.connection.Close();
        }

    }
}
