using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace SocketPingPong
{

    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game GameObject = new Game();
            Int32 GameMode = GameObject.MainWindows();
            bool loading = false;

            switch(GameMode)
            {
                case 49:
                    Console.WriteLine("Select Server Mode");
                    break;
                case 50:
                    Console.WriteLine("Select Client Mode");
                    break;
                case 51:
                    Console.WriteLine("Ok, Bye :)");
                    System.Threading.Thread.Sleep(1000);
                    return;
            }
        }
    }

    public struct EnemyPos
    {
        Int32 xPos;
        Int32 yPos;
        Int32 xlength;
    }

    public struct BallPos
    {
        Int32 xPos;
        Int32 yPos;
    }

    public class Game
    {
        public static ManualResetEvent alldone = new ManualResetEvent(false);
        public bool Connect
        {
            get { return _connect; }
        }

        public EnemyPos enemypos
        {
            get { return _enemypos; }
            set { _enemypos = enemypos; }
        }

        public BallPos ballpos
        {
            get { return _ballpos; }
            set { _ballpos = ballpos; }
        }

        private bool _connect = false;
        private EnemyPos _enemypos;
        private BallPos _ballpos;

        internal Int32 MainWindows()
        {
            Int32 SelectMenu = 0;
            Console.Clear();
            Console.SetWindowSize(50, 40);
            Console.BufferHeight = 40;
            Console.BufferWidth = 50;
            Console.Title = "SocketPingPong";
            for (int i = 0; i < 39; i++ )
            {
                for(int j = 0; j < 50; j++)
                {
                    if(i == 0 || i == 38)
                    {
                        Console.SetCursorPosition(j, i);
                        Console.Write("@");
                    }
                    else
                    {
                        Console.SetCursorPosition(1, i);
                        Console.Write('@');
                        Console.SetCursorPosition(49, i);
                        Console.Write('@');
                    }
                }
            }
            Console.SetCursorPosition(19, 14);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.SetCursorPosition(19, 14);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Ping - Pong");
            Console.SetCursorPosition(20, 17);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("1. Server");
            Console.SetCursorPosition(20, 18);
            Console.WriteLine("2. Client");
            Console.SetCursorPosition(20, 19);
            Console.WriteLine("3. Exit");

            SelectMenu = Console.ReadKey(false).KeyChar;
            if(SelectMenu < 49 || SelectMenu > 51)
            {
                Console.Clear();
                Console.SetCursorPosition(15, 14);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Wrong Select, ReSelect");
                System.Threading.Thread.Sleep(1000);
                MainWindows();
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            return SelectMenu;
        }

        internal void ServerGame()
        {

        }

        internal void ClientGame()
        {

        }

        internal void ServerSocket()
        {
            byte[] BUF = new byte[1024];
            
                Socket server = new Socket
                    (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];

                Console.Write("Input Port : ");
                string strPort = Console.ReadLine();
                Int32 port = Convert.ToInt32(strPort);

                IPEndPoint ipep = new IPEndPoint(ipAddress, port);

            try
            {
                server.Bind(ipep);
                server.Listen(1);
                while(true)
                {
                    alldone.Reset();

                    Console.WriteLine("Wating a Client Connect...");
                    server.BeginAccept(new AsyncCallback(AcceptCallback), server);
                }

            }
            catch (SocketException se)
            {
                Console.Clear();
                Console.WriteLine(se.ToString());
                System.Threading.Thread.Sleep(1000);
                MainWindows();
            }
        }

        internal void AcceptCallback(IAsyncResult ar)
        {
            alldone.Set();

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            StateObject state = (StateObject)ar.AsyncState;
            state.workSocket = handler;
            Console.WriteLine("Connected Complite!");
            Thread.Sleep(1000);
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);

        }

        internal void ReadCallback(IAsyncResult ar)
        {

        }

        internal void ClientSocket()
        {

        }

    }
}
