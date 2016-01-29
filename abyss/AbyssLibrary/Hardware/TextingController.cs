using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace AbyssLibrary
{
    public class TextingController : AbstractNetworkedDevice
    {
        // port the android texting app is listening on
        private const int ServerPort = 6000;
        private const string Clear_Message_History = "CLEAR_MESSAGE_HISTORY";
        private const string Ping_Message = "PING_MESSAGE";
        private const string Heartbeat = "HEARTBEAT";

        public bool IsHeartbeatHealthy { get; private set; }
        Timer heartbeatTimer;

        public TextingController(string name, string deviceMacAddress)
            : base(name, deviceMacAddress)
        {
            IsHeartbeatHealthy = true;

            // untested code. may be an OK idea, but not taking any risks for now.
            //heartbeatTimer = new Timer(PollHeartbeat, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }

        private void PollHeartbeat(object obj)
        {
            SendTextMessage(Heartbeat);
        }

        public void ClearHistory()
        {
            SendTextMessage(Clear_Message_History);
        }

        public void PingMessage()
        {
            SendTextMessage(Ping_Message);
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
                Debug.Log("Trying to send packet: {0}", toSend);
                IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(IpAddress), ServerPort);
                byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes(toSend);

                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(serverAddress);
                clientSocket.Send(toSendBytes);
                clientSocket.Close();
                IsHeartbeatHealthy = true;
            }
            catch(Exception e)
            {
                Debug.Log("Problem sending text message:", e);
                IsHeartbeatHealthy = false;
            }
        }
    }
}
