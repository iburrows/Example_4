using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Example_4.TheClient
{
    class Client
    {
        private string ip;
        private int port;
        private Action<string> MessageInformer;
        //private Action AbortInformer;
        Socket clientSocket;
        byte[] buffer = new byte[512];
        Thread receiveThread;

        public Client(string ip, int port, Action<string> messageInformer)
        {
            try
            {
                this.ip = ip;
                this.port = port;
                this.MessageInformer = messageInformer;
                //this.AbortInformer = clientDisconnected;
                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse(ip), port);
                clientSocket = client.Client;
                StartReceiving();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void StartReceiving()
        {
            receiveThread = new Thread(new ThreadStart(Receive));
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }

        private void Receive()
        {
            string message = "";
            while (receiveThread.IsAlive)
            {
                int length = clientSocket.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer,0,length);
                MessageInformer(message);
            }
        }

        public bool IsConnected()
        {
            return clientSocket.Connected;
        }
    }
}
