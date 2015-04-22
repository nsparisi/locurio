using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
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
                port.Open();
                port.DataReceived += OnDataReceived;
            }
            catch(Exception e)
            {
                Debug.Log("Problem with serial port: ", e.ToString());
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
                Debug.Log("Problem sending serial data: ", e.ToString());
            }
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs args)
        {
            SerialPort serialPort = (SerialPort)sender;
            string data = string.Empty;
            string endpointID = string.Empty;
            string message = string.Empty;

            try
            {
                data = serialPort.ReadLine();
            }
            catch (Exception e)
            {
                Debug.Log("Problem reading serial data: ", e.ToString());
            }

            // data format will be
            // "EndpointID::message"
            if (data.Contains(Delimiter))
            {
                int delimiterIndex = data.IndexOf(Delimiter);
                endpointID = data.Substring(0, delimiterIndex);
                message = data.Substring(delimiterIndex + Delimiter.Length);
            }

            if (!string.IsNullOrEmpty(endpointID) && !string.IsNullOrEmpty(message))
            {

                if(endpoints.ContainsKey(endpointID))
                {
                    endpoints[endpointID].ReceivedMessage(message);
                }
            }
        }

        public void TestData()
        {
            port.DiscardInBuffer();
            port.DiscardOutBuffer();

            port.Write(new[] { 'a' }, 0, 1);
            port.Write(new[] { 'b' }, 0, 1);
            port.Write(new[] { 'a' }, 0, 1);
            port.Write(new[] { 'b' }, 0, 1);
            port.Write("b");
            port.Write("a");
            port.Write("test");

            char[] buffer = new char[1000];
            int line1 = port.Read(buffer, 0, 1);
            string line2 = port.ReadLine();
            string line3 = port.ReadLine();

            port.DiscardInBuffer();
            port.DiscardOutBuffer();

            // port.DataReceived += OnDataReceived;
            port.Close();
        }
    }
}
