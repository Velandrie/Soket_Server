using System;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;

namespace Soket_Server
{
    class Program
    {
        enum Group_State
        {
            waiting = 0,
            busy = 2 
        }

        private static byte[] _buffer = new byte[1024];
        private static List<Socket> _clientSockets = new List<Socket>();        
        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            Console.Title = "Server";
            SetupServer();

           
           

        }

        private static void SetupServer()
        {
            Console.WriteLine("Server Hazırlanıyor..");
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
            while (true)
            {
                _serverSocket.Listen(1);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

            }


        }
        private static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket = _serverSocket.EndAccept(AR);
            _clientSockets.Add(socket);
            Console.WriteLine("Client bağlandı..");
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

        }
        private static void ReceiveCallback(IAsyncResult AR)
        {
            Groups grup1 = new Groups();
            grup1.Group_state = "waiting";

            Groups grup2 = new Groups();
            grup2.Group_state = "busy";

            Socket socket = (Socket)AR.AsyncState;
            int received = socket.EndReceive(AR);
            byte[] dataBuf = new byte[received];
            Array.Copy(_buffer, dataBuf, received);
            string text = Encoding.ASCII.GetString(dataBuf);
            Console.WriteLine("Alınan Mesaj " + text);

            string response = string.Empty;
            /*if(Convert.ToInt16(text) == Group_State.busy)
            {
              
            }*/

            if (text == "1")
            {
                response = Group_State.busy.ToString();
            }
            if(text == "2")
            {
                response = Group_State.waiting.ToString(); ;

            }
            else
            {
                Console.Write("Server : ");
                string req = Console.ReadLine();

                
                
             byte[] buffer = Encoding.ASCII.GetBytes(req);
             socket.Send(buffer);
                

               


                byte[] receivedBuf = new byte[1024];
            }
            byte[] data = Encoding.ASCII.GetBytes(response);
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallBack), socket);
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);


        }

        private static void SendCallBack(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
        }

        

        }
        struct Groups
        {
        public string Group_state;
        
        }
    
    
    
}
    

