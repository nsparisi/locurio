using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class LimitlessLEDBridge : AbstractNetworkedDevice
    {
        public event AbyssEvent TurnedOff;
        public event AbyssEvent TurnedOn;
        public event AbyssEvent Changed;

        private const byte Command_AllOff = 0x41;
        private const byte Command_AllOn = 0x42;
        private const byte Command_SpeedDown = 0x43;
        private const byte Command_SpeedUp = 0x44;
        private const byte Command_Zone1On = 0x45;
        private const byte Command_Zone1Off = 0x46;
        private const byte Command_Zone2On = 0x47;
        private const byte Command_Zone2Off = 0x48;
        private const byte Command_Zone3On = 0x49;
        private const byte Command_Zone3Off = 0x4A;
        private const byte Command_Zone4On = 0x4B;
        private const byte Command_Zone4Off = 0x4C;

        private const byte Command_BeforeDelay_All = 0x42;
        private const byte Command_BeforeDelay_Zone1 = 0x45;
        private const byte Command_BeforeDelay_Zone2 = 0x47;
        private const byte Command_BeforeDelay_Zone3 = 0x49;
        private const byte Command_BeforeDelay_Zone4 = 0x4B;

        private const byte Command_AfterDelay_AllWhite = 0xC2;
        private const byte Command_AfterDelay_Zone1White = 0xC5;
        private const byte Command_AfterDelay_Zone2White = 0xC7;
        private const byte Command_AfterDelay_Zone3White = 0xC9;
        private const byte Command_AfterDelay_Zone4White = 0xCB;

        private const byte Command_AfterDelay_SetBrightness = 0x4E;
        private const byte Command_AfterDelay_SetColor = 0x40;

        private const byte Empty_Middle_Byte = 0x00;
        private const byte Final_Byte = 0x55;

        private object lockObj = new object();

        public enum ColorType
        {
            Violet = 0x00,
            Royal_Blue = 0x10,
            Baby_Blue = 0x20,
            Aqua = 0x30,
            Mint = 0x40,
            Seafoam_Green = 0x50,
            Green = 0x60,
            Lime_Green = 0x70,
            Yellow = 0x80,
            Yellow_Orange = 0x90,
            Orange = 0xA0,
            Red = 0xB0,
            Pink = 0xC0,
            Fusia = 0xD0,
            Lilac = 0xE0,
            Lavendar = 0xF0
        }

        public enum ZoneType { 
            All, 
            Zone1, 
            Zone2, 
            Zone3, 
            Zone4 
        }

        private int port;

        public LimitlessLEDBridge(string name, string deviceMacAddress, int port = 5577)
            : base(name, deviceMacAddress)
        {
            this.port = port;
        }

        private void SendCommand(byte[] message)
        {
            if (!this.IsConnected)
            {
                Debug.Log("Cannot send LED LightBulb message. LimitlessLEDBridge controller '{0}' is not connected to device '{1}'.", this.Name, this.MacAddress);
                return;
            }

            try
            {
                lock (lockObj)
                {
                    TcpClient tcpClient = new TcpClient(this.IpAddress, this.port);
                    NetworkStream networkStream = tcpClient.GetStream();
                    networkStream.Write(message, 0, message.Length);
                    tcpClient.Close();

                    // TODO remove this code block once above has been thoroughly tested.
                    /*
                    UdpClient udpClient = new UdpClient(this.IpAddress, this.port);
                    udpClient.Send(message, message.Length);

                    // TODO: this is a test, double/triple up on the message sent
                    Thread.Sleep(10);
                    udpClient.Send(message, message.Length);
                    udpClient.Close();
                    */
                }
            }
            catch (Exception e)
            {
                Debug.Log("There was an error sending LED LightBulb message {0}", message);
                Debug.Log("Error:", e);
            }
        }

        private void SendTwoCommandsWithDelay(
            byte[] messageBeforeDelay,
            byte[] messageAfterDelay)
        {
            if (!this.IsConnected)
            {
                Debug.Log("Cannot send LED LightBulb message. LimitlessLEDBridge controller '{0}' is not connected to device '{1}'.", this.Name, this.MacAddress);
                return;
            }

            try
            {
                lock (lockObj)
                {
                    TcpClient tcpClient = new TcpClient(this.IpAddress, this.port);
                    NetworkStream networkStream = tcpClient.GetStream();
                    networkStream.Write(messageBeforeDelay, 0, messageBeforeDelay.Length);
                    Thread.Sleep(100);
                    networkStream.Write(messageAfterDelay, 0, messageAfterDelay.Length);
                    tcpClient.Close();

                    // TODO remove this code block once above has been thoroughly tested.
                    /*
                    // for some commands, the bridge expects two messages
                    // that have a 100ms delay in between.
                    UdpClient udpClient = new UdpClient(this.IpAddress, this.port);

                    udpClient.Send(messageBeforeDelay, messageBeforeDelay.Length);

                    // TODO: this is a test, double/triple up on the message sent
                    Thread.Sleep(10);
                    udpClient.Send(messageBeforeDelay, messageBeforeDelay.Length);
                    Thread.Sleep(10);
                    udpClient.Send(messageBeforeDelay, messageBeforeDelay.Length);

                    Thread.Sleep(100);

                    udpClient.Send(messageAfterDelay, messageAfterDelay.Length);

                    // TODO: this is a test, double/triple up on the message sent
                    Thread.Sleep(10);
                    udpClient.Send(messageAfterDelay, messageAfterDelay.Length);
                    Thread.Sleep(10);
                    udpClient.Send(messageAfterDelay, messageAfterDelay.Length);

                    udpClient.Close();
                    */
                }
            }
            catch (Exception e)
            {
                Debug.Log("There was an error sending LED LightBulb messages '{0}' + '{1}'", messageBeforeDelay, messageAfterDelay);
                Debug.Log("Error:", e);
            }
        }

        public void TurnOn(ZoneType zone)
        {
            Debug.Log("Turning On {0}", zone);

            byte zoneOn = Command_AllOn;
            if (zone == ZoneType.All) zoneOn = Command_AllOn;
            else if (zone == ZoneType.Zone1) zoneOn = Command_Zone1On;
            else if (zone == ZoneType.Zone2) zoneOn = Command_Zone2On;
            else if (zone == ZoneType.Zone3) zoneOn = Command_Zone3On;
            else if (zone == ZoneType.Zone4) zoneOn = Command_Zone4On;

            SendCommand(new byte[]
                {
                    zoneOn, Empty_Middle_Byte, Final_Byte
                });

            OnTurnedOn(this, EventArgs.Empty);
            OnChanged(this, EventArgs.Empty);
        }

        public void TurnOff(ZoneType zone)
        {
            Debug.Log("Turning Off {0}", zone);

            byte zoneOff = Command_AllOff;
            if (zone == ZoneType.All) zoneOff = Command_AllOff;
            else if (zone == ZoneType.Zone1) zoneOff = Command_Zone1Off;
            else if (zone == ZoneType.Zone2) zoneOff = Command_Zone2Off;
            else if (zone == ZoneType.Zone3) zoneOff = Command_Zone3Off;
            else if (zone == ZoneType.Zone4) zoneOff = Command_Zone4Off;

            SendCommand(new byte[]
                {
                    zoneOff, Empty_Middle_Byte, Final_Byte
                });

            OnTurnedOff(this, EventArgs.Empty);
            OnChanged(this, EventArgs.Empty);
        }

        public void ChangeColor(ColorType color, ZoneType zone)
        {
            Debug.Log("Changing to color: {0}, {1}", color.ToString(), zone);

            byte beforeDelayZone = Command_BeforeDelay_All;
            if (zone == ZoneType.All) beforeDelayZone = Command_BeforeDelay_All;
            else if (zone == ZoneType.Zone1) beforeDelayZone = Command_BeforeDelay_Zone1;
            else if (zone == ZoneType.Zone2) beforeDelayZone = Command_BeforeDelay_Zone2;
            else if (zone == ZoneType.Zone3) beforeDelayZone = Command_BeforeDelay_Zone3;
            else if (zone == ZoneType.Zone4) beforeDelayZone = Command_BeforeDelay_Zone4;

            SendTwoCommandsWithDelay(
                new byte[] { beforeDelayZone, Empty_Middle_Byte, Final_Byte },
                new byte[] { Command_AfterDelay_SetColor, (byte)color, Final_Byte });

            OnChanged(this, EventArgs.Empty);
        }

        public void ChangeToWhite(ZoneType zone)
        {
            Debug.Log("Changing to white light {0}", zone);

            byte beforeDelayZone = Command_BeforeDelay_All;
            if (zone == ZoneType.All) beforeDelayZone = Command_BeforeDelay_All;
            else if (zone == ZoneType.Zone1) beforeDelayZone = Command_BeforeDelay_Zone1;
            else if (zone == ZoneType.Zone2) beforeDelayZone = Command_BeforeDelay_Zone2;
            else if (zone == ZoneType.Zone3) beforeDelayZone = Command_BeforeDelay_Zone3;
            else if (zone == ZoneType.Zone4) beforeDelayZone = Command_BeforeDelay_Zone4;

            SendTwoCommandsWithDelay(
                new byte[] { beforeDelayZone, Empty_Middle_Byte, Final_Byte },
                new byte[] { Command_AfterDelay_AllWhite, Empty_Middle_Byte, Final_Byte });

            OnChanged(this, EventArgs.Empty);
        }

        public void ChangeBrightness(double percentage, ZoneType zone)
        {
            Debug.Log("Changing to brightness: {0}, {1}", percentage.ToString(), zone);
            byte brightness = ConvertPercentToBrightnessByte(percentage);

            byte beforeDelayZone = Command_BeforeDelay_All;
            if (zone == ZoneType.All) beforeDelayZone = Command_BeforeDelay_All;
            else if (zone == ZoneType.Zone1) beforeDelayZone = Command_BeforeDelay_Zone1;
            else if (zone == ZoneType.Zone2) beforeDelayZone = Command_BeforeDelay_Zone2;
            else if (zone == ZoneType.Zone3) beforeDelayZone = Command_BeforeDelay_Zone3;
            else if (zone == ZoneType.Zone4) beforeDelayZone = Command_BeforeDelay_Zone4;

            SendTwoCommandsWithDelay(
                new byte[] { beforeDelayZone, Empty_Middle_Byte, Final_Byte },
                new byte[] { Command_AfterDelay_SetBrightness, (byte)brightness, Final_Byte });

            OnChanged(this, EventArgs.Empty);
        }

        private void OnTurnedOff(object sender, EventArgs e)
        {
            if (TurnedOff != null)
            {
                TurnedOff(sender, e);
            }
        }

        private void OnTurnedOn(object sender, EventArgs e)
        {
            if (TurnedOn != null)
            {
                TurnedOn(sender, e);
            }
        }

        private void OnChanged(object sender, EventArgs e)
        {
            if (Changed != null)
            {
                Changed(sender, e);
            }
        }

        private byte ConvertPercentToBrightnessByte(double brightness)
        {
            // range is 0x02 - 0x1B 
            // this is 26 possible steps
            int decimalValue = (int)Math.Floor(brightness * 25);
            byte byteValue = (byte)(0x02 + decimalValue);
            return byteValue;
        }
    }
}
