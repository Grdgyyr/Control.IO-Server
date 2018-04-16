using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Control_IO.Misc;
using Control_IO.Server;
using udptest.Operation;

namespace Control_IO.Server
{
    class UDPConnection
    {
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 6000);
        UdpClient udpClient = null;        

        byte[] bytes;
        string inputData;        
        private string localIP;
        private int port;

        private void SetupServer()
        {
            while (true)
            {
                if (udpClient == null)
                {
                    udpClient = new UdpClient(6000);                    
                }

                try
                {
                    bytes = udpClient.Receive(ref groupEP);
                    inputData = Encoding.UTF8.GetString(bytes);

                    //ComputeData.ComputeReceivedData(inputData, _decimalSeparator);
                    //msgType = ComputeReceivedData(inputData);
                    //if (msgType == MessageType.Bye)
                    //{
                    //    SetDisconnectedInfoToUI();
                    //    udpClient.Close();
                    //    udpClient = null;
                    //}

                    //receiveMsg(inputData);
                }

                //On time out
                catch (SocketException)
                {
                    if (udpClient != null)
                        udpClient.Close();

                    udpClient = null;
                    //addtoLogs("Client disconnected...");
                }
            }
        }

        private void Do_Work(object sender, DoWorkEventArgs e)
        {
            while (true)
                SetupServer();
        }
    }
}
