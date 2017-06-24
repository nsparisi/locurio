using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AbyssLibrary
{
    public class SPMqttReceivedMessage : AbstractSubProcessor
    {
        [AbyssParameter]
        public string ExpectedMessage { get; set; }

        [AbyssParameter]
        public List<MqttSubscriber> Subscribers
        {
            get;
            set;
        }

        [AbyssOutput]
        public event AbyssEvent DataReceived;

        [AbyssOutput]
        public event AbyssEvent ExpectedMessageReceived;

        public SPMqttReceivedMessage()
            : base()
        {
            this.Name = "SPMqttSubscriber";
            this.Subscribers = new List<MqttSubscriber>();
        }

        public override void Initialize()
        {
            foreach (MqttSubscriber endpoint in this.Subscribers)
            {
                endpoint.DataReceived += OnReceivedData;
            }
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

            MqttMsgPublishEventArgs mqttArgs = (MqttMsgPublishEventArgs)args;
            string message = Encoding.Default.GetString(mqttArgs.Message);
            if (ExpectedMessageReceived != null && message.Equals(ExpectedMessage))
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
