using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class SPLightBulb : AbstractSubProcessor
    {
        [AbyssParameter]
        public List<TestLightBulb> Lights
        {
            get;
            set;
        }

        [AbyssInput]
        public void TurnOn()
        {
            foreach (TestLightBulb light in this.Lights)
            {
                light.TurnOn();
            }

            if (Out != null)
            {
                Out();
            }
        }

        [AbyssInput]
        public void TurnOff()
        {
            foreach (TestLightBulb light in this.Lights)
            {
                light.TurnOff();
            }
            
            if(Out != null)
            {
                Out();
            }
        }
        
        [AbyssOutput]
        public event OutputEvent Out;


        public SPLightBulb()
        {
            this.Name = "SPLightBulb";
        }
    }
}
