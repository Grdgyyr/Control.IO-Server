using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Control_IO
{
    public partial class EvalForm_01 : Form
    {
        public Form MainForm { get; set; }

        public EvalForm_01()
        {
            InitializeComponent();
        }

        private void backB_Click(object sender, EventArgs e)
        {
            MainForm.Show();
            this.Dispose();
        }

        private void closeB_Click(object sender, EventArgs e)
        {
            Dispose();
            System.Environment.Exit(1);
        }

        private void minimizeB_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
