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
    public class TextingController : AbstractPhysicalObject
    {
        public string IpAddress { get; private set; }
        
        public TextingController(string ipAddress)
            : base()
        {
            this.IpAddress = ipAddress;
        }

        public TextingController(string ipAddress, string name)
            : base(name)
        {
            this.IpAddress = ipAddress;
        }

        public void SendTextMessage(string toSend)
        {
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
                Debug.Log(e);
            }
        }
    }
}
