using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AbyssLibrary
{
    public class SPMqttPublisher : AbstractSubProcessor
    {
        [AbyssParameter]
        public string Topic { get; set; }

        [AbyssParameter]
        public string Message { get; set; }

        [AbyssParameter]
        public List<MqttBroker> Brokers
        {
            get;
            set;
        }

        public SPMqttPublisher()
            : base()
        {
            this.Name = "SPMqttPublisher";
            this.Brokers = new List<MqttBroker>();
        }

        [AbyssInput]
        public void Start(object sender, EventArgs e)
        {
            StartProcess();
        }

        protected override void Process()
        {
            Debug.Log("SPMqttPublisher Start T:{0} M:{1}", this.Topic, this.Message);

            byte[] messageBytes = Encoding.Default.GetBytes(this.Message);
            foreach (MqttBroker broker in this.Brokers)
            {
                if (broker.Client != null && broker.Client.IsConnected)
                {
                    Debug.Log("Sending T:{0} M:{1}", this.Topic, this.Message);
                    broker.Client.Publish(this.Topic, messageBytes);
                }
            }

            Debug.Log("SPMqttPublisher End");
            ProcessEnded();
        }
    }
}
