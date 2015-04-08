using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class TestLightBulb : AbstractPhysicalObject
    {
        public void TurnOn()
        {
            Debug.Log("Turning On");
        }

        public void TurnOff()
        {
            Debug.Log("{Turning Off");
        }
    }
}
