using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace KCSN
{
    public delegate void ReceiveAsync();
   public class Network
    {
       // location in hex form
       public string Location = SEA.ConvertToHex(GeoLocation.GetLocation());
       public Connection CSNetworkConnection;
       public Network()
       {
           try
           {
               CloudCheck.Init();
               CloudCheck.Detected += new CloudDetected(CloudCheck_Detected);
               CSNetworkConnection = new Connection();
               SEA.InitializeKey(2048, Encoding.UTF8.GetBytes("5SA9H3QDG2/*653V9Q+ez1g9*e6vcxg*tufd688yfdhy4gu7789e89ez56789y7t7/rt7e87t8tr787ç/78"), 57);
             
              
               ReceiveAsync recv = new ReceiveAsync(AsyncReceive);
               recv.BeginInvoke(null, null);
           }
           catch
           {

           }
           finally
           {

           }
       }
       void CloudCheck_Detected(string hash, string virusname, string filename)
       {
           try
           {
               // hex (b64(sea(text)))
               CSNetworkConnection.PostData(SEA.ConvertToHex(SEA.EncryptToBase64(Encoding.UTF8.GetBytes("hash=" + hash + "|name=" + virusname + "|file=" + filename))), "cloud", Location);

           }
           catch
           {

           }
           finally
           {

           }
       }
       public void CheckReputation(string hash,string filename)
       {
           try
           {
               CloudCheck.CheckAsync(hash, filename);
           }
           catch
           {

           }
           finally
           {

           }
       }
       public void Connect(string username, string UID)
       {
           try
           {

               CSNetworkConnection.Connect(username, UID);
           }
           catch
           {

           }
           finally
           {

           }
          
       }
       public void Disconnect()
       {
           try
           {
               CSNetworkConnection.Disconnect();

           }
           catch
           {

           }
           finally
           {

           }
       }
       public void PostData(string data, string type)
       {
           try
           {
               // hex (b64(sea(text)))
               CSNetworkConnection.PostData(SEA.ConvertToHex(SEA.EncryptToBase64(Encoding.UTF8.GetBytes(data))), type, Location);
           }
           catch
           {

           }
           finally
           {

           }
       }
       private void ProcessMessage(string smsg)
       {
           foreach (string msg in smsg.Split(';'))
           {
               string operand = msg.Substring(0, 3);
               string data = msg.Remove(0, 3);
               switch (operand)
               {
                   case "JVP":

                       break;
                   case "IOA":

                       break;
                   case "AVC":

                       break;
                   case "GUI":

                       break;
              }
           }
       }
       public void AsyncReceive()
       {
       lb_001:
           {
               try
               {
                   string resp = CSNetworkConnection.Receive();
                   if (resp != null)
                       ProcessMessage(resp);   
                                    
               }
               catch
               {

               }
               finally
               {

               }
           }
           Thread.Sleep(60000);
           goto lb_001;
       }
    }
}
