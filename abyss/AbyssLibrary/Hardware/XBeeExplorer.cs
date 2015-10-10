using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    internal class XBeeExplorer
    {
        const string ComPort = "COM3";
        const string Delimiter = "::";
        private SerialPort port;

        private Dictionary<string, XBeeEndpoint> endpoints;

        private static XBeeExplorer instance;
        public static XBeeExplorer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new XBeeExplorer();
                }

                return instance;
            }
        }

        private XBeeExplorer()
        {
            endpoints = new Dictionary<string, XBeeEndpoint>();

            try
            {
                port = new SerialPort(ComPort, 9600, Parity.None, 8, StopBits.One);
                if (!port.IsOpen)
                {
                    port.Open();
                }
                port.DataReceived += OnDataReceived;
            }
            catch(Exception e)
            {
                Debug.Log("Problem opening serial port: ", e);
            }
        }

        internal void RegisterEndpoint(XBeeEndpoint endpoint)
        {
            if (!endpoints.ContainsKey(endpoint.EndpointID))
            {
                endpoints.Add(endpoint.EndpointID, endpoint);
            }
        }

        internal void SendData(XBeeEndpoint endpoint, string message)
        {
            string formattedMessage = string.Format("{0}{1}{2}", endpoint.EndpointID, Delimiter, message);
            Debug.Log("Sending message to XBee: {0}", formattedMessage);

            try
            {
                port.WriteLine(formattedMessage);
            }
            catch (Exception e)
            {
                Debug.Log("Problem sending serial data: ", e);
            }
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs args)
        {
            // buffer may not be fully loaded -- wait for the message to fill up 
            Thread.Sleep(100);
            SerialPort serialPort = (SerialPort)sender;
            char[] buffer = new char[serialPort.BytesToRead];

            try
            {
                serialPort.Read(buffer, 0, serialPort.BytesToRead);
            }
            catch (Exception e)
            {
                Debug.Log("Problem reading serial data: ", e);
            }

            string[] allMessages = (new string(buffer)).Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string data in allMessages)
            {
                string endpointID = string.Empty;
                string message = string.Empty;

                // data format will be
                // "EndpointID::message"
                if (data.Contains(Delimiter))
                {
                    int delimiterIndex = data.IndexOf(Delimiter);
                    endpointID = data.Substring(0, delimiterIndex);
                    message = data.Substring(delimiterIndex + Delimiter.Length);
                    Debug.Log("Msg: {0}", message);
                }

                if (!string.IsNullOrEmpty(endpointID) && !string.IsNullOrEmpty(message))
                {

                    if (endpoints.ContainsKey(endpointID))
                    {
                        endpoints[endpointID].ReceivedMessage(message);
                    }
                }
            }
        }
    }
}
