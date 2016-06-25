using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;

namespace ConsoleApplication1
{
    class UdpState
    {
        public UdpClient u;
        public IPEndPoint e;
    }
    class Program
    {
        public static void ReceiveCallback(IAsyncResult ar)
        {
            UdpClient u = (UdpClient)((UdpState)(ar.AsyncState)).u;
            IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;

            Byte[] receiveBytes = u.EndReceive(ar, ref e);
            string receiveString = Encoding.ASCII.GetString(receiveBytes);

 
            UdpState s = new UdpState();
            s.e = e;
            s.u = u;

                     //u.Send(Encoding.UTF8.GetBytes(res.Output), res.Output.Length, e);
            u.BeginReceive(new AsyncCallback(ReceiveCallback), s);

        }

   
        public static void ReceiveMessages()
        {
            IPEndPoint e = new IPEndPoint(IPAddress.Any, 15000);
            UdpClient u;
            while (true)
            {
                try
                {
                    // Receive a message and write it to the console.

                    u = new UdpClient(e);
                    break;
                }
                catch
                {
                    Thread.Sleep(1000);
                }

            }

            UdpState s = new UdpState();
            s.e = e;
            s.u = u;


            u.BeginReceive(new AsyncCallback(ReceiveCallback), s);
        }
        static void Main(string[] args)
        {
            ReceiveMessages();
            UPnP.NAT.Discover();
            UPnP.NAT.ForwardPort(15000, ProtocolType.Udp, "CONSOLE");
            Console.Read();
            UPnP.NAT.DeleteForwardingRule(15000, ProtocolType.Udp);
        }
    }
}
