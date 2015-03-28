using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss
{
    class LightBulbSubProcessor : AbstractSubProcessor
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

        }

        [AbyssInput]
        public void TurnOff()
        {

        }
        
        [AbyssOutput]
        public event OutputEvent Out;
    }
}
