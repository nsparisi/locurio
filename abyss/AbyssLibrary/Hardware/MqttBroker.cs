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
    public class MqttBroker : AbstractNetworkedDevice
    {
        public MqttClient Client { get; private set; }

        public MqttBroker(string name, string deviceMacAddress, string bestGuessIpAddress)
            : base(name, deviceMacAddress, bestGuessIpAddress)
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
            while (!this.IsConnected)
            {
                Thread.Sleep(1000);
            }

            Debug.Log("MqttBroker '{0}' is connecting ", this.Name);
            this.Client = new MqttClient(this.IpAddress);
            this.Client.MqttMsgSubscribed += MqttMsgSubscribedEvent;
            this.Client.MqttMsgUnsubscribed += MqttMsgUnSubscribedEvent;
            this.Client.Connect(this.Name);
        }
        
        private void MqttMsgSubscribedEvent(object sender, MqttMsgSubscribedEventArgs e)
        {
            Debug.Log("Subscribed");
        }

        private void MqttMsgUnSubscribedEvent(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            Debug.Log("UnSubscribed");
        }
    }
}
