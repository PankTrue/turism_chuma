using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApp1
{
    
    static class Program
    {
        [STAThread]
        static void Main()
        {

            GlobalVariables.connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\Main.accdb;
            Persist security Info = false;";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
