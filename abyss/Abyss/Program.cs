using System;
using System.Collections.Generic;
using AbyssLibrary;

namespace Abyss
{
    class Program
    {
        static void Main(string[] args)
        {
            TestLightBulb testLightBulb1 = new TestLightBulb();
            TestLightBulb testLightBulb2 = new TestLightBulb();
            TestLightBulb testLightBulb3 = new TestLightBulb();
            TestLightBulb testLightBulb4 = new TestLightBulb();


            SPLightBulb altarLights = new SPLightBulb
            {
                Lights = new List<TestLightBulb>
                {
                    testLightBulb1,
                    testLightBulb2,
                    testLightBulb3,
                    testLightBulb4,
                }
            };

            SPDelay delayThenTurnOn = new SPDelay()
            {
                DurationMs = 1000,
            };
            delayThenTurnOn.Finished += altarLights.TurnOn;

            // wait a second then turn on lights
            delayThenTurnOn.Start();

        }
    }
}
