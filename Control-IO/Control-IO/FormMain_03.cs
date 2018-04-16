using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Control_IO.Misc;
using Control_IO.Server;
using udptest.Operation;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Control_IO
{
    public partial class FormMain_03 : Form
    {
        public Form StartupForm { get; set; }
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
        private string valuePath = "";

        public FormMain_03()
        {
            InitializeComponent();
            InitializeValues();
            InitializeQR();
            InitializeGestures();
            bg_work.WorkerReportsProgress = true;
            bg_work.DoWork += Do_Work;
            bg_work.RunWorkerAsync();

            System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;
            _decimalSeparator = ci.NumberFormat.CurrencyDecimalSeparator;
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
                    setConnected();

                    ComputeData.ComputeReceivedData(inputData, _decimalSeparator);
                    //msgType = ComputeReceivedData(inputData);
                    //if (msgType == MessageType.Bye)
                    //{
                    //    SetDisconnectedInfoToUI();
                    //    udpClient.Close();
                    //    udpClient = null;
                    //}

                    receiveMsg(inputData);

                    string[] array = inputData.Split('|');

                    if (array[0] == "info")
                        SetDeviceInfo(array);

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

        private void InitializeValues()
        {
            localIP = new Connection_QR().GetIPAddress();
            port = 6000;
            ipAddTB.Text = localIP;
            portTB.Text = port.ToString();
        }

        private void InitializeGestures()
        {
            gestureListCB.DataSource = ComputeGesture.GetGestureList();
        }
        

        private void InitializeQR()
        {
            Zen.Barcode.CodeQrBarcodeDraw qrcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            picBoxQR.Image = qrcode.Draw(localIP + ":" + port, 50);
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

        private void backB_Click(object sender, EventArgs e)
        {
            StartupForm.Show();
            this.Hide();
        }

        private void evalB_Click(object sender, EventArgs e)
        {
            EvalForm_01 ef01 = new EvalForm_01();
            ef01.MainForm = this;
            ef01.Show();
            this.Hide();
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

        private void setConnected()
        {
            logsTB.Invoke((MethodInvoker)delegate
            {
                if (ipStatusTB.Text != "Connected")
                    ipStatusTB.Text = "Connected";
            });
        }
        
        private void SetDeviceInfo(String[] info)
        {
            deviceNameTB.Invoke((MethodInvoker)delegate
            {
                deviceNameTB.Text = info[1];
            });
            macAddTB.Invoke((MethodInvoker)delegate
            {
                macAddTB.Text = info[2];
            });
        }

        private void refBtn_Click(object sender, EventArgs e)
        {
            InitializeGestures();
        }

        private void gestureListCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            valueLbl.Text = "";
            keyCB.SelectedIndex = -1;

            List<string> gesture = ComputeGesture.GetGesture(gestureListCB.SelectedIndex);
            categoryCB.SelectedIndex = GetGestureCatIdx(gesture[1]);

            if (categoryCB.SelectedIndex == 0)
            {
                string[] fileName = gesture[2].Split('\\');
                valueLbl.Text = fileName[fileName.Count()-1];
            }
            else
                keyCB.SelectedIndex = keyCB.FindStringExact(gesture[2]);

        }

        private string GetGestureCatName(int idx)
        {
            switch (idx)
            {
                case 0:
                    return "open";
                case 1:
                    return "key";
                case 3:
                    return "macro";
                default:
                    return "";
            }
        }
        private int GetGestureCatIdx(string cat)
        {
            switch (cat)
            {
                case "open":
                    return 0;                    
                case "key":
                    return 1;
                case "macro":
                    return 2;
                default:
                    return -1;
            }
        }

        private void categoryCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            openFileBtn.Visible = false;
            valueLbl.Visible = false;
            keyCB.Visible = false;

            if (categoryCB.SelectedIndex == 0)
            {
                openFileBtn.Visible = true;
                valueLbl.Visible = true;
            }
            else if (categoryCB.SelectedIndex == 1)
                keyCB.Visible = true;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (categoryCB.SelectedIndex == 0)
                ComputeGesture.AssignFunction(gestureListCB.SelectedItem.ToString(), GetGestureCatName(categoryCB.SelectedIndex), valuePath);
            else if (categoryCB.SelectedIndex == 1)
                ComputeGesture.AssignFunction(gestureListCB.SelectedItem.ToString(), GetGestureCatName(categoryCB.SelectedIndex), keyCB.SelectedItem.ToString());
        }

        private void gestureListCB_MouseClick(object sender, MouseEventArgs e)
        {
            int idx = gestureListCB.SelectedIndex;
            InitializeGestures();
            gestureListCB.SelectedIndex = idx;
        }

        private void openFileBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                valueLbl.Text = dlg.SafeFileName;
                valuePath = dlg.FileName;
            }
        }
    }
}
