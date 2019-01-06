using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EdgeJs;

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

        public class MilightCommand
        {
            public int Zone = 1;
            public string Command = "";
            public int Value = 0;
        }

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
            Fuchsia = 0xD0,
            Lilac = 0xE0,
            Lavender = 0xF0
        }

        public enum ZoneType { 
            All = 0, 
            Zone1 = 1, 
            Zone2 = 2, 
            Zone3 = 3, 
            Zone4 = 4 
        }

        private int port;

        public LimitlessLEDBridge(string name, string deviceMacAddress, string bestGuessIpAddress, int port = 8899)
            : base(name, deviceMacAddress, bestGuessIpAddress)
        {
            this.port = port;
            Setup(bestGuessIpAddress);
        }

        Func<object, Task<object>> sendFunc = null;

        private void Setup(string ip)
        {
            var jsProg = @"
                var Milight = require('node-milight-promise').MilightController;
                var commands = require('node-milight-promise').commandsV6;
                 
                if ('undefined' === typeof GLOBAL.light)
                {
                    console.log('making light');
                    GLOBAL.light = new Milight({
                        ip: '$$$IP$$$',
                            delayBetweenCommands: 50,
                            commandRepeat: 2,
                            type: 'v6'
                        });
                }

                return function (data, callback) {                 
                          var todo = [];

                         for (var command of data) {
                            zone = command.Zone;
                            switch (command.Command) {
                                case 'on':
                                    todo.push(commands.rgbw.on(zone));
                                    break;
                                case 'off':
                                    todo.push(commands.rgbw.off(zone));
                                    break;
                                case 'color':
                                    todo.push(commands.rgbw.on(zone));
                                    todo.push(commands.rgbw.brightness(zone, 100));
                                    todo.push(commands.rgbw.hue(zone, command.Value, true));
                                    break;
                                case 'brightness':
                                    todo.push(commands.rgbw.on(zone));
                                    todo.push(commands.rgbw.brightness(zone, command.Value));
                                    break;
                                case 'white':
                                    todo.push(commands.rgbw.on(zone));
                                    todo.push(commands.rgbw.whiteMode(zone));
                                }

                         }

                        light.sendCommands(...todo);
                }
            ";

            jsProg = jsProg.Replace("$$$IP$$$", ip);
            sendFunc = Edge.Func(jsProg);
        }

        private void SendCommand(MilightCommand c)
        {
            if (sendFunc == null)
            {
                Debug.Log("Can't send command because light isn't connected yet.");
            }

            SendCommands(new[] {c});
        }

        private void SendCommands(MilightCommand[] c)
        {
            sendFunc(c);
        }

        private int ZoneTypeToInt(ZoneType zone)
        {
            return (int) zone;
        }

        public void TurnOn(ZoneType zone)
        {

            Debug.Log("Turning On {0}", zone);

            SendCommand(new MilightCommand() { Command = "on", Zone = (int)zone});

            OnTurnedOn(this, EventArgs.Empty);
            OnChanged(this, EventArgs.Empty);
        }

        public void TurnOff(ZoneType zone)
        {
            Debug.Log("Turning Off {0}", zone);

            SendCommand(new MilightCommand() { Command = "off", Zone = (int)zone });

            OnTurnedOff(this, EventArgs.Empty);
            OnChanged(this, EventArgs.Empty);
        }

        public void ChangeColor(ColorType color, ZoneType zone)
        {
            
            Debug.Log("Changing to color: {0}, {1}", color.ToString(), zone);


            SendCommand(new MilightCommand() { Command = "color", Zone = (int)zone, Value = (int)color});

            OnChanged(this, EventArgs.Empty);
        }

        public void ChangeToWhite(ZoneType zone)
        {
            Debug.Log("Changing to white light {0}", zone);

            SendCommand(new MilightCommand() { Command = "white", Zone = (int)zone});

            OnChanged(this, EventArgs.Empty);
        }

        public void ChangeBrightness(double percentage, ZoneType zone)
        {
            Debug.Log("Changing to brightness: {0}, {1}", percentage.ToString(), zone);
            SendCommand(new MilightCommand() { Command = "brightness", Zone = (int)zone, Value = (int)(percentage * 100.0) });
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
        
    }
}
