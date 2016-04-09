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
    public class TimerController : AbstractNetworkedDevice
    {
        // port the android texting app is listening on
        private const int ServerPort = 6000;

        private const string Timer_Start = "TIMER_START";
        private const string Timer_Suspend = "TIMER_SUSPEND";
        private const string Timer_Reset = "TIMER_RESET";
        private const string Timer_Set_Time = "TIMER_SET_TIME";

        private object lockObj = new object();

        public TimerController(string name, string deviceMacAddress, string bestGuessIpAddress)
            : base(name, deviceMacAddress, bestGuessIpAddress)
        {
        }

        public void Start()
        {
            SendMessageToDevice(Timer_Start);
        }

        public void Suspend()
        {
            SendMessageToDevice(Timer_Suspend);
        }

        public void Reset()
        {
            SendMessageToDevice(Timer_Reset);
        }

        public void SetTime(long milliseconds)
        {
            SendMessageToDevice(Timer_Set_Time + milliseconds);
        }

        private void SendMessageToDevice(string toSend)
        {
            if (!this.IsConnected)
            {
                Debug.Log("Cannot send message to timer. TimerController '{0}' is not connected to device at '{1}'.", this.Name, this.MacAddress);
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

            lock(lockObj)
            {
                Socket clientSocket = null;

                try
                {
                    IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(IpAddress), ServerPort);
                    byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes(toSend);

                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    clientSocket.Connect(serverAddress);
                    clientSocket.Send(toSendBytes);
                    clientSocket.Close();
                }
                catch (Exception e)
                {
                    Debug.Log("Problem sending text message:", e);
                }
            }
        }
    }
}
