using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Example_4.TheServer
{
    class ClientHandler
    {
        private Socket socket;
        private byte[] buffer = new byte[512];

        public ClientHandler(Socket socket)
        {
            this.socket = socket;
        }

        public void send(string message)
        {
            socket.Send(Encoding.UTF8.GetBytes(message));
        }
    }
}
