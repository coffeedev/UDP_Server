using System;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Runtime.InteropServices;

namespace UDP_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            NetServer listener = new NetServer(11000);
            listener.OpenServer();

            String temp = "0.0\t0.0\t0.0";

            int x = (int)Decimal.Parse(temp.Split("\t")[0]) * 10;
        }
    }

    public class SetMousePos
    {
        int _x = 0, _y = 0;
        String _data = String.Empty;
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int xPos, int yPos);

        //public SetMousePos(int x, int y)
        //{
        //    _x = x;
        //    _y = y;
        //}

        public SetMousePos(string data) => _data = data;

        public void Display()
        {
            try
            {
                int x = 1 + (int)Decimal.Parse(_data.Split("\t")[0]) * 10;
                int y = 1 + (int)Decimal.Parse(_data.Split("\t")[1]) * 10;
                
                Console.WriteLine("X : {0}, Y : {1}", x, y);

                if (x > 0 || y > 0)
                {
                    SetCursorPos(x, y);
                }
            }
            catch (Exception)
            {

                throw;
            }
           

        }
    }


    public class NetServer
    {
        short listenPort = 0;
        public NetServer(short port)
        {
            listenPort = port;
        }

        public void OpenServer()
        {
            //Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            bool done = false;
            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
            string received_data;
            byte[] receive_byte_array;
            try
            {
                while (!done)
                {
                    Console.WriteLine("Waiting for broadcast");

                    receive_byte_array = listener.Receive(ref groupEP);
                    Console.WriteLine("Received a broadcast from {0}", groupEP.ToString());
                    received_data = Encoding.ASCII.GetString(receive_byte_array, 0, receive_byte_array.Length);
                    Console.WriteLine("\n{0}\n", received_data);
                    SetMousePos mp = new SetMousePos(received_data);
                    mp.Display();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            listener.Close();

        }

    }
}
