using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AbyssLibrary
{
    public class MqttSubscriber : AbstractNetworkedDevice
    {
        public event AbyssEvent DataReceived;
        public string Topic { get; private set; }

        public MqttSubscriber(string topic, string name, string deviceMacAddress, string bestGuessIpAddress)
            : base(name, deviceMacAddress, bestGuessIpAddress)
        {
            this.Topic = topic;
        }

        public void Initialize(string topic)
        {
            this.WaitAndConnectToClient();
        }

        public void WaitAndConnectToClient()
        {
            Thread t = new Thread(ThreadWaitAndConnectToClient);
            t.IsBackground = true;
            t.Start();
        }

        private void ThreadWaitAndConnectToClient()
        {
            Debug.Log("ThreadWaitAndConnectToClient start ");
            while (!this.IsConnected)
            {
                Thread.Sleep(1000);
            }

            Debug.Log("ThreadWaitAndConnectToClient connecting to Mqttclient");
            MqttClient client = new MqttClient(this.IpAddress);
            client.MqttMsgPublishReceived += MqttMsgPublishReceivedEvent;
            client.MqttMsgSubscribed += MqttMsgSubscribedEvent;
            client.MqttMsgUnsubscribed += MqttMsgUnSubscribedEvent;
            client.Connect(this.Name);
            client.Subscribe(new string[] { this.Topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        private void MqttMsgPublishReceivedEvent(object sender, MqttMsgPublishEventArgs e)
        {
            string message = Encoding.Default.GetString(e.Message);
            Debug.Log("Received: " + message);

            if (DataReceived != null)
            {
                DataReceived(this, e);
            }
        }

        private void MqttMsgSubscribedEvent(object sender, MqttMsgSubscribedEventArgs e)
        {
            Debug.Log("Subscribed");
        }

        private void MqttMsgUnSubscribedEvent(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            Debug.Log("UnSubscribed");
        }

        public void DebugImpersonateMessage(string message)
        {
            Debug.Log("Impersonating message delivery: {0}", message);
            MqttMsgPublishReceivedEvent(this, new MqttMsgPublishEventArgs(this.Topic, Encoding.Default.GetBytes(message), false, 2, false));
        }
    }
}
