﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApp1
{
    public partial class AdminPanel : Form
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form form = new TableEditor("Сотрудники");
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form = new TableEditor("МаршрутКурорты");
            form.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form form = new TableEditor("Бронь");
            form.Show();
        }
    }
}
