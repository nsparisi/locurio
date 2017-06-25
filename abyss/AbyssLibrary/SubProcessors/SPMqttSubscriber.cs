using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AbyssLibrary
{
    public class SPMqttSubscriber : AbstractSubProcessor
    {
        [AbyssParameter]
        public string Topic { get; set; }

        [AbyssParameter]
        public string ExpectedMessage { get; set; }

        [AbyssParameter]
        public List<MqttBroker> Brokers
        {
            get;
            set;
        }

        [AbyssOutput]
        public event AbyssEvent DataReceived;

        [AbyssOutput]
        public event AbyssEvent ExpectedMessageReceived;

        public SPMqttSubscriber()
            : base()
        {
            this.Name = "SPMqttSubscriber";
            this.Brokers = new List<MqttBroker>();
        }

        public override void Initialize()
        {
            Thread t = new Thread(ThreadWaitAndConnectToClient);
            t.IsBackground = true;
            t.Start();
        }

        private void ThreadWaitAndConnectToClient()
        {
            Debug.Log("SPMqttSubscriber waiting to initialize");
            foreach (MqttBroker endpoint in this.Brokers)
            {
                while (endpoint.Client == null)
                {
                    Thread.Sleep(1000);
                }

                Debug.Log("SPMqttSubscriber subscribing to topic {0}", this.Topic);
                endpoint.Client.MqttMsgPublishReceived += MqttMsgPublishReceivedEvent;
                endpoint.Client.Subscribe(new string[] { this.Topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            }
        }
        
        private void MqttMsgPublishReceivedEvent(object sender, MqttMsgPublishEventArgs e)
        {
            if (this.IsDisabled)
            {
                return;
            }

            if (string.Equals(this.Topic, e.Topic))
            {
                string message = Encoding.Default.GetString(e.Message);
                Debug.Log(string.Format("MqttSubscriber Received T:{0} M:{1}", this.Topic, message));

                if (DataReceived != null)
                {
                    DataReceived(this, e);
                }

                if (ExpectedMessageReceived != null && message.Equals(this.ExpectedMessage))
                {
                    ExpectedMessageReceived(sender, e);
                }
            }
        }
    }
}
