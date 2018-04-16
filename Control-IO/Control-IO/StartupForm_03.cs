using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Control_IO.Misc;
using Control_IO.Server;
using udptest.Operation;

namespace Control_IO
{
    public partial class StartupForm_03 : Form
    {         
        private string localIP;
        private int port;        

        public StartupForm_03()
        {
            InitializeComponent();
            InitializeValues();
            InitializeQR();
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

        private void proceedB_Click(object sender, EventArgs e)
        {
            FormMain_03 fm03 = new FormMain_03();
            fm03.StartupForm = this;
            fm03.Show();
            this.Hide();
        }

        private void InitializeValues()
        {
            localIP = new Connection_QR().GetIPAddress();
            //port = new Random().Next(10000, 30000);
            port = 6000;
            ipAddTB.Text = localIP;
            portTB.Text = port.ToString();        
        }

        private void InitializeQR()
        {
            Zen.Barcode.CodeQrBarcodeDraw qrcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            picBoxQR.Image = qrcode.Draw(localIP + ":" + port, 50);
        }
    }
}
