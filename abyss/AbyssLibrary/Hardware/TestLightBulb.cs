using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class TestLightBulb : AbstractPhysicalObject
    {
        public enum LightBulbState { On, Off }
        public LightBulbState State { get; private set; }

        public event AbyssEvent TurnedOff;
        public event AbyssEvent TurnedOn;

        public TestLightBulb()
            : base()
        {

        }

        public TestLightBulb(string name)
            : base(name)
        {

        }

        public void TurnOn()
        {
            Debug.Log("Turning On");
            this.State = LightBulbState.On;
            OnTurnedOn(this, EventArgs.Empty);
        }

        public void TurnOff()
        {
            Debug.Log("Turning Off");
            this.State = LightBulbState.Off;
            OnTurnedOff(this, EventArgs.Empty);
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
