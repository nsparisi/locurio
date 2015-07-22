using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AbyssLibrary
{
    public class TextingController : AbstractNetworkedDevice
    {
        private const string Clear_Message_History = "CLEAR_MESSAGE_HISTORY";
        
        public TextingController(string name, string deviceMacAddress)
            : base(name, deviceMacAddress)
        {
        }

        public void ClearHistory()
        {
            SendTextMessage(Clear_Message_History);
        }

        public void SendTextMessage(string toSend)
        {
            if (!this.IsConnected)
            {
                Debug.Log("Cannot send text message. Texting controller '{0}' is not connected to text device at '{1}'.", this.Name, this.MacAddress);
                return;
            }

            if(string.IsNullOrWhiteSpace(toSend))
            {
                return;
            }

            if(!toSend.Contains("\r\n"))
            {
                toSend += "\r\n";
            }

            Socket clientSocket = null;

            try
            {
                IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(IpAddress), 6000);
                byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes(toSend);

                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(serverAddress);
                clientSocket.Send(toSendBytes);
                clientSocket.Close();
            }
            catch(Exception e)
            {
                Debug.Log("Problem sending text message:", e);
            }
        }
    }
}
