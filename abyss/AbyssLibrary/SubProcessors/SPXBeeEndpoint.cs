using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class SPXBeeEndpoint : AbstractSubProcessor
    {
        [AbyssParameter]
        public string SendMessage { get; set; }

        [AbyssParameter]
        public string ExpectedMessage { get; set; }

        [AbyssParameter]
        public List<XBeeEndpoint> Endpoints
        {
            get;
            set;
        }

        [AbyssInput]
        public void SendData(object sender, EventArgs e)
        {
            StartProcess();
        }

        [AbyssOutput]
        public event AbyssEvent DataReceived;

        [AbyssOutput]
        public event AbyssEvent ExpectedMessageReceived;

        public SPXBeeEndpoint()
            : base()
        {
            this.Name = "SPXBeeEndpoint";
            this.Endpoints = new List<XBeeEndpoint>();
        }

        public override void Initialize()
        {
            foreach (XBeeEndpoint endpoint in Endpoints)
            {
                endpoint.DataReceived += OnReceivedData;
            }
        }

        protected override void Process()
        {
            Debug.Log("SPXBeeEndpoint Start [{0}]", Name);

            foreach (XBeeEndpoint endpoint in Endpoints)
            {
                endpoint.SendData(SendMessage);
            }

            ProcessEnded();
        }

        protected override void ProcessEnded()
        {
            Debug.Log("SPXBeeEndpoint Ended [{0}]", Name);
        }

        private void OnReceivedData(object sender, EventArgs args)
        {
            if(this.IsDisabled)
            {
                return;
            }

            if (DataReceived != null)
            {
                DataReceived(sender, args);
            }

            XBeeReceivedEvent xbeeArgs = (XBeeReceivedEvent)args;
            if (ExpectedMessageReceived != null && xbeeArgs.Message.Equals(ExpectedMessage))
            {
                ExpectedMessageReceived(sender, args);
            }
        }

        public void DebugReceivedExpectedMessage()
        {
            if (ExpectedMessageReceived != null)
            {
                ExpectedMessageReceived(null, EventArgs.Empty);
            }
        }
    }
}
