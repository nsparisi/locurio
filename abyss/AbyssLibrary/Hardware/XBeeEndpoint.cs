using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class XBeeEndpoint : AbstractPhysicalObject
    {
        public event AbyssEvent DataReceived;
        public string EndpointID { get; private set; }

        public XBeeEndpoint(string endpointID, string name)
            : base(name)
        {
            this.EndpointID = endpointID;

            XBeeExplorer.Instance.RegisterEndpoint(this);
        }

        public void SendData(string message)
        {
            XBeeExplorer.Instance.SendData(this, message);
        }

        internal void ReceivedMessage(string message)
        {
            if (DataReceived != null)
            {
                DataReceived(this, new XBeeReceivedEvent(EndpointID, message));
            }
        }
    }
}
