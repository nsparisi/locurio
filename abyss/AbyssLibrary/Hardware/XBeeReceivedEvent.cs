using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class XBeeReceivedEvent : EventArgs
    {
        public string EndpointID { get; private set; }
        public string Message { get; private set; }

        public XBeeReceivedEvent(string endpointID, string message)
        {
            this.Message = message;
            this.EndpointID = endpointID;
        }
    }
}
