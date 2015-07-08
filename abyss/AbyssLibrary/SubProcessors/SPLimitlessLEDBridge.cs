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
        public LimitlessLEDBridge.ZoneType Zone
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
            Debug.Log("SPLightBulb Proc Start");

            foreach (LimitlessLEDBridge bridge in this.Bridges)
            {
                if(Command == LEDBridgeCommand.TurnOn)
                {
                    bridge.TurnOn(Zone);
                }
                else if (Command == LEDBridgeCommand.TurnOff)
                {
                    bridge.TurnOff(Zone);
                }
                else if (Command == LEDBridgeCommand.SetBrightness)
                {
                    bridge.ChangeBrightness(Brightness, Zone);
                }
                else if (Command == LEDBridgeCommand.SetColor)
                {
                    bridge.ChangeColor(Color, Zone);
                }
                else if (Command == LEDBridgeCommand.SetToWhite)
                {
                    bridge.ChangeToWhite(Zone);
                }
            }

            ProcessEnded();
        }

        protected override void ProcessEnded()
        {
            Debug.Log("SPLightBulb Proc Ended");
        }

        public override void Initialize()
        {
            foreach (var bridge in this.Bridges)
            {
                bridge.TurnedOn += OnTurnedOn;
                bridge.TurnedOff += OnTurnedOff;
                bridge.Changed += OnChanged;
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
