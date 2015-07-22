using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class SPLightBulb : AbstractSubProcessor
    {
        bool turnOn = false;

        public SPLightBulb()
            : base()
        {
            this.Name = "SPLightBulb";
            this.Lights = new List<TestLightBulb>();
        }

        [AbyssParameter]
        public List<TestLightBulb> Lights
        {
            get;
            set;
        }

        [AbyssInput]
        public void TurnOn(object sender, EventArgs e)
        {
            turnOn = true;
            StartProcess();
        }

        [AbyssInput]
        public void TurnOff(object sender, EventArgs e)
        {
            turnOn = false;
            StartProcess();
        }

        [AbyssOutput]
        public event AbyssEvent TurnedOff;

        [AbyssOutput]
        public event AbyssEvent TurnedOn;

        protected override void Process()
        {
            Debug.Log("SPLightBulb Start  [{0}]", Name);

            if(turnOn)
            {
                foreach (TestLightBulb light in this.Lights)
                {
                    light.TurnOn();
                }
            } 
            else
            {
                foreach (TestLightBulb light in this.Lights)
                {
                    light.TurnOff();
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
            foreach (var light in this.Lights)
            {
                light.TurnedOn += OnTurnedOn;
                light.TurnedOff += OnTurnedOff;
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
    }
}
