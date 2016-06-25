using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Speech.Synthesis;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using KAIMLBot;
using System.Reflection;

namespace KAIML
{
    internal static class WinApi
    {
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);

        public static int RegisterWindowMessage(string format, params object[] args)
        {
            string message = String.Format(format, args);
            return RegisterWindowMessage(message);
        }

        public const int HWND_BROADCAST = 0xffff;
        public const int SW_SHOWNORMAL = 1;

        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImportAttribute("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void ShowToFront(IntPtr window)
        {
            ShowWindow(window, SW_SHOWNORMAL);
            SetForegroundWindow(window);
        }
    }
    internal static class SingleInstance
    {
        public static string AssemblyGuid
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);
                if (attributes.Length == 0)
                {
                    return String.Empty;
                }
                return ((System.Runtime.InteropServices.GuidAttribute)attributes[0]).Value;
            }
        }
        public static readonly int WM_SHOWFIRSTINSTANCE =
            WinApi.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", AssemblyGuid);
        static Mutex mutex;
        public static bool Start()
        {
            bool onlyInstance = false;
            string mutexName = String.Format("Local\\{0}", AssemblyGuid);

            // if you want your app to be limited to a single instance
            // across ALL SESSIONS (multiple users & terminal services), then use the following line instead:
            // string mutexName = String.Format("Global\\{0}", ProgramInfo.AssemblyGuid);

            mutex = new Mutex(true, mutexName, out onlyInstance);
            run = true;
            return onlyInstance;
        }
        public static void ShowFirstInstance()
        {
            WinApi.PostMessage(
                (IntPtr)WinApi.HWND_BROADCAST,
                WM_SHOWFIRSTINSTANCE,
                IntPtr.Zero,
                IntPtr.Zero);

        }
        internal static bool run = false;
        public static void Stop()
        {
            mutex.ReleaseMutex();
            mutex.Close();
            run = false;
        }
    }
    class UdpState
    {
        public UdpClient u;
        public IPEndPoint e;
    }
    class Program
    {
    
     static Bot myBot;
     static User myUser;

        public static bool messageReceived = false;

public static void ReceiveCallback(IAsyncResult ar)
{
  UdpClient u = (UdpClient)((UdpState)(ar.AsyncState)).u;
  IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;

  Byte[] receiveBytes = u.EndReceive(ar, ref e);
  string receiveString = Encoding.ASCII.GetString(receiveBytes);

  messageReceived = true;

  UdpState s = new UdpState();
  s.e = e;
  s.u = u;

  Console.Write(receiveString);
  Console.WriteLine(" ");
  Request r = new Request(receiveString, myUser, myBot);
      Result res = myBot.Chat(r);
      SPS.SpeakAsync(res.Output);
      Console.WriteLine("Bot: " + res.Output);

      u.Send(Encoding.UTF8.GetBytes(res.Output), res.Output.Length, e);
  u.BeginReceive(new AsyncCallback(ReceiveCallback), s);

}

public static SpeechSynthesizer SPS;
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
            if (!SingleInstance.Start())
                return;
            SPS = new SpeechSynthesizer();
            SPS.Rate = 0;
            ReceiveMessages();
            myBot = new Bot();
            myBot.loadSettings();
            myUser = new User("consoleUser", myBot);
            myBot.isAcceptingUserInput = false;
            myBot.loadAIMLFromFiles();
            myBot.isAcceptingUserInput = true;
            while (true)
            {
                Console.Write("You: ");
                string input = Console.ReadLine();
                if (input.ToLower() == "quit")
                {
                    break;
                }
                else
                {
                    Request r = new Request(input, myUser, myBot);
                    Result res = myBot.Chat(r);
                    SPS.SpeakAsync(res.Output);
                    Console.WriteLine("Bot: " + res.Output);
                }
            }

            SingleInstance.Stop();
        }
  
    }
}
