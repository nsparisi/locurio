using System;
using System.Collections.Generic;
using AbyssLibrary;
using System.Threading;

namespace Abyss
{
    class AbyssRunner
    {
        public void Start()
        {
            Thread t = new Thread(Run);
            t.Start();
        }

        void Run()
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

            AbyssMonitorController monitorController = new AbyssMonitorController();
            SPMonitor countdownMonitor = new SPMonitor
            {
                Monitors = new List<AbyssMonitorController>
                {
                    monitorController,
                }
            };

            SPDelay delayThenTurnOn = new SPDelay()
            {
                DurationMs = 5000,
            };

            delayThenTurnOn.Finished += altarLights.TurnOn;
            delayThenTurnOn.Finished += countdownMonitor.Stop;
            countdownMonitor.CountdownStarted += delayThenTurnOn.Start;

            altarLights.Initialize();
            countdownMonitor.Initialize();
            delayThenTurnOn.Initialize();
        }
    }
}
