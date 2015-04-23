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
            TestLightBulb testLightBulb1 = new TestLightBulb("TestLeft1");
            TestLightBulb testLightBulb2 = new TestLightBulb("TestLeft2");
            TestLightBulb testLightBulb3 = new TestLightBulb("TestRight1");
            TestLightBulb testLightBulb4 = new TestLightBulb("TestRight2");
            XBeeEndpoint alterXbee = new XBeeEndpoint("1", "XBee Endpoint 1");
            AbyssScreenController screenController = new AbyssScreenController("Clock");

            AbyssSystem.Instance.RegisterPhysicalObject(testLightBulb1);
            AbyssSystem.Instance.RegisterPhysicalObject(testLightBulb2);
            AbyssSystem.Instance.RegisterPhysicalObject(testLightBulb3);
            AbyssSystem.Instance.RegisterPhysicalObject(testLightBulb4);
            AbyssSystem.Instance.RegisterPhysicalObject(alterXbee);
            AbyssSystem.Instance.RegisterPhysicalObject(screenController);

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

            SPScreen countdownScreen = new SPScreen
            {
                Screens = new List<AbyssScreenController>
                {
                    screenController,
                }
            };

            SPDelay delayThenTurnOn = new SPDelay()
            {
                DurationMs = 5000,
            };

            SPXBeeEndpoint altarFinishedProcessor = new SPXBeeEndpoint()
            {
                SendMessage = "testmessage",
                ExpectedMessage = "success",
                Endpoints = new List<XBeeEndpoint>
                {
                    alterXbee
                }
            };

            AbyssSystem.Instance.RegisterSubProcessor(altarLights);
            AbyssSystem.Instance.RegisterSubProcessor(countdownScreen);
            AbyssSystem.Instance.RegisterSubProcessor(delayThenTurnOn);
            AbyssSystem.Instance.RegisterSubProcessor(altarFinishedProcessor);

            altarFinishedProcessor.ExpectedMessageReceived += countdownScreen.Stop;
            altarFinishedProcessor.ExpectedMessageReceived += delayThenTurnOn.Start;
            delayThenTurnOn.Finished += altarLights.TurnOn;

            // for fun
            countdownScreen.CountdownExpired += altarLights.TurnOn;

            altarLights.Initialize();
            countdownScreen.Initialize();
            delayThenTurnOn.Initialize();
            altarFinishedProcessor.Initialize();
        }
    }
}
