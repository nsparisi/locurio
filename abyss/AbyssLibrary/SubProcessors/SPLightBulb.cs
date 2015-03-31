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
        public event AbyssEvent Out;

        protected override void Process()
        {
            Debug.Log("SPLightBulb Proc Start");

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
            Debug.Log("SPLightBulb Proc Ended");

            if (Out != null)
            {
                Out(this, EventArgs.Empty);
            }
        }

        public SPLightBulb()
        {
            this.Name = "SPLightBulb";
        }
    }
}
