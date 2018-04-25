using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public static class GlobalVariables
    {
        public static  OleDbConnection connection = new OleDbConnection();
        public static int current_user;
        public static Form mainForm;
    }
}
