using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class SPLimitlessLEDBridge : AbstractSubProcessor
    {
        public enum LEDBridgeCommand { TurnOn, TurnOff, SetToWhite, SetBrightness, SetColor }
        
        public SPLimitlessLEDBridge()
            : base()
        {
            this.Name = "SPLEDBridge";
            this.Bridges = new List<LimitlessLEDBridge>();
            this.Zones = new List<LimitlessLEDBridge.ZoneType>();
        }

        [AbyssParameter]
        public List<LimitlessLEDBridge> Bridges
        {
            get;
            set;
        }

        [AbyssParameter]
        public LEDBridgeCommand Command
        {
            get;
            set;
        }

        [AbyssParameter]
        public LimitlessLEDBridge.ZoneType? Zone
        {
            get;
            set;
        }

        [AbyssParameter]
        public List<LimitlessLEDBridge.ZoneType> Zones
        {
            get;
            set;
        }

        /// <summary>
        /// Brightness decimal from 0.0 - 1.0
        /// </summary>
        [AbyssParameter]
        public double Brightness
        {
            get;
            set;
        }

        [AbyssParameter]
        public LimitlessLEDBridge.ColorType Color
        {
            get;
            set;
        }

        [AbyssInput]
        public void Run(object sender, EventArgs e)
        {
            StartProcess();
        }

        [AbyssOutput]
        public event AbyssEvent TurnedOff;

        [AbyssOutput]
        public event AbyssEvent TurnedOn;

        [AbyssOutput]
        public event AbyssEvent Changed;

        protected override void Process()
        {
            Debug.Log("SPLightBulb Start [{0}] [command: {1}] [brightness: {2}] [color: {3}]", Name, Command, Brightness, Color);
            
            foreach (LimitlessLEDBridge bridge in this.Bridges)
            {
                foreach (var eachZone in Zones)
                {
                    if (Command == LEDBridgeCommand.TurnOn)
                    {
                        bridge.TurnOn(eachZone);
                    }
                    else if (Command == LEDBridgeCommand.TurnOff)
                    {
                        bridge.TurnOff(eachZone);
                    }
                    else if (Command == LEDBridgeCommand.SetBrightness)
                    {
                        bridge.ChangeBrightness(Brightness, eachZone);
                    }
                    else if (Command == LEDBridgeCommand.SetColor)
                    {
                        bridge.ChangeColor(Color, eachZone);
                    }
                    else if (Command == LEDBridgeCommand.SetToWhite)
                    {
                        bridge.ChangeToWhite(eachZone);
                    }
                }
            }

            ProcessEnded();
        }

        protected override void ProcessEnded()
        {
            Debug.Log("SPLightBulb Ended [{0}]", Name);
        }

        public override void Initialize()
        {
            foreach (var bridge in this.Bridges)
            {
                bridge.TurnedOn += OnTurnedOn;
                bridge.TurnedOff += OnTurnedOff;
                bridge.Changed += OnChanged;
            }

            if (Zone != null)
            {
                Zones.Add(Zone.Value);
            }
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
