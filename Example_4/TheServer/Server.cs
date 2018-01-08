using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Example_4.TheServer
{
    public class Server:ViewModelBase
    {
        private Socket serverSocket;
        private Thread acceptingThread;
        private Action<string> GuiUpdater;
        private List<ClientHandler> theClients = new List<ClientHandler>();

        public Server(int port, string ip, Action<string> guiUpdater)
        {
            this.GuiUpdater = guiUpdater;
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            serverSocket.Listen(5);
            StartAccepting();
        }

        private void StartAccepting()
        {
            acceptingThread = new Thread(new ThreadStart(Accept));
            acceptingThread.IsBackground = true;
            acceptingThread.Start();
        }

        private void Accept()
        {
            while (acceptingThread.IsAlive)
            {
                try
                {
                    theClients.Add(new ClientHandler(serverSocket.Accept()));
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public void NewMessageReceived(string message)
        {
            GuiUpdater(message);

            foreach (var client in theClients)
            {
                client.send(message);
            }
        }

        public bool AreClients()
        {
            if (theClients.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}

