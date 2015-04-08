using System;
using System.Collections.Generic;
using AbyssLibrary;
using System.Threading;

namespace Abyss
{
    public class AbyssRunner
    {
        public void Start()
        {
            Thread t = new Thread(Run);
            t.Start();
        }

        public void Run()
        {
            TestLightBulb testLightBulb1 = new TestLightBulb();
            TestLightBulb testLightBulb2 = new TestLightBulb();
            TestLightBulb testLightBulb3 = new TestLightBulb();
            TestLightBulb testLightBulb4 = new TestLightBulb();

            AbyssSystem.Instance.RegisterPhysicalObject(testLightBulb1);
            AbyssSystem.Instance.RegisterPhysicalObject(testLightBulb2);
            AbyssSystem.Instance.RegisterPhysicalObject(testLightBulb3);
            AbyssSystem.Instance.RegisterPhysicalObject(testLightBulb4);

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
            AbyssSystem.Instance.RegisterSubProcessor(altarLights);

            AbyssScreenController screenController = new AbyssScreenController();
            AbyssSystem.Instance.RegisterPhysicalObject(screenController);

            SPScreen countdownScreen = new SPScreen
            {
                Screens = new List<AbyssScreenController>
                {
                    screenController,
                }
            };
            AbyssSystem.Instance.RegisterSubProcessor(countdownScreen);

            SPDelay delayThenTurnOn = new SPDelay()
            {
                DurationMs = 5000,
            };
            AbyssSystem.Instance.RegisterSubProcessor(delayThenTurnOn);

            delayThenTurnOn.Finished += altarLights.TurnOn;
            delayThenTurnOn.Finished += countdownScreen.Stop;
            countdownScreen.CountdownStarted += delayThenTurnOn.Start;

            altarLights.Initialize();
            countdownScreen.Initialize();
            delayThenTurnOn.Initialize();
        }
    }
}
