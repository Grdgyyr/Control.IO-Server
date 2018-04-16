using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using Control_IO.Misc;
using Control_IO.Server;
using udptest.Operation;

namespace Control_IO
{
    public partial class FormMain_UpdatedUI : Form
    {
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 6000);
        UdpClient udpClient = null;
        byte[] bytes;
        string inputData;        
        BackgroundWorker bg_work = new BackgroundWorker();
        
        string _decimalSeparator;
        private const float MIN_MOV = 0.035f;
        private float _x, _y;

        private string localIP;
        private int port;

        public FormMain_UpdatedUI()
        {
            InitializeComponent();
            InitializeValues();
            InitializeQR();
            bg_work.WorkerReportsProgress = true;
            bg_work.DoWork += Do_Work;
            bg_work.RunWorkerAsync();

            System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;
            _decimalSeparator = ci.NumberFormat.CurrencyDecimalSeparator;
        }

        private void minimizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Dispose();
            System.Environment.Exit(1);
        }
        private void InitializeValues()
        {
            localIP = new Connection_QR().GetIPAddress();
            port = 6000;
            ipTB.Text = localIP + ":" + port;

        }

        private void InitializeQR()
        {
            Zen.Barcode.CodeQrBarcodeDraw qrcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            picBoxQR.Image = qrcode.Draw(localIP + ":" + port, 50);
        }

        private void SetupServer()
        {
            while (true)
            {
                if (udpClient == null)
                {
                    udpClient = new UdpClient(6000);
                    addtoLogs("Server online...");
                }

                try
                {
                    bytes = udpClient.Receive(ref groupEP);
                    inputData = Encoding.UTF8.GetString(bytes);

                    ComputeData.ComputeReceivedData(inputData, _decimalSeparator);
                    //msgType = ComputeReceivedData(inputData);
                    //if (msgType == MessageType.Bye)
                    //{
                    //    SetDisconnectedInfoToUI();
                    //    udpClient.Close();
                    //    udpClient = null;
                    //}

                    receiveMsg(inputData);
                }

                //On time out
                catch (SocketException)
                {
                    if (udpClient != null)
                        udpClient.Close();

                    udpClient = null;
                    addtoLogs("Client disconnected...");
                }
            }
        }


        private void Do_Work(object sender, DoWorkEventArgs e)
        {
            while (true)
                SetupServer();
        }

        private void receiveMsg(String msg)
        {
            debugTB.Invoke((MethodInvoker)delegate
            {
                debugTB.Text += msg + "\r\n";

                int maxLines = 6;

                if (debugTB.Lines.Length > maxLines)
                {
                    string[] newLines = new string[maxLines];

                    Array.Copy(debugTB.Lines, 1, newLines, 0, maxLines);

                    debugTB.Lines = newLines;
                }
            });
        }

        private void addtoLogs(String msg)
        {
            logsTB.Invoke((MethodInvoker)delegate
            {
                logsTB.Text += msg + "\r\n";


            });
        }

        private void msgTB_TextChanged(object sender, EventArgs e)
        {
            debugTB.SelectionStart = debugTB.Text.Length;
            debugTB.ScrollToCaret();
        }

        private void logsTB_TextChanged(object sender, EventArgs e)
        {
            debugTB.SelectionStart = debugTB.Text.Length;
            debugTB.ScrollToCaret();
        }


        private void btnTest_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Browse Text Files";

            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;

            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.ShowReadOnly = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //txtBrowse.Text = openFileDialog1.FileName;
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start(txtBrowse.Text);

        }

        private void picBoxQR_Click(object sender, EventArgs e)
        {

        }
    }
}
