using System;
using System.Collections.Generic;
using AbyssLibrary;
using System.Threading;

namespace Abyss
{
    public class TestRunner : IAbyssRunner
    {
        public void Start()
        {
            Thread t = new Thread(Run);
            t.Start();
        }

        public void Run()
        {
            MqttSubscriber mqttSubscriber_abyss = new MqttSubscriber("/locurio/librariansguild", "abyss", "94-DE-80-07-BD-AF", "192.168.1.7");
            AbyssSystem.Instance.RegisterPhysicalObject(mqttSubscriber_abyss);

            SPMqttReceivedMessage sp_mqttSubscriber1 = new SPMqttReceivedMessage()
            {
                ExpectedMessage = "puzzle_1_solved",
                Subscribers = MakeList(mqttSubscriber_abyss)
            };

            SPMqttReceivedMessage sp_mqttSubscriber2 = new SPMqttReceivedMessage()
            {
                ExpectedMessage = "puzzle_2_solved",
                Subscribers = MakeList(mqttSubscriber_abyss)
            };

            SPMqttReceivedMessage sp_mqttSubscriber3 = new SPMqttReceivedMessage()
            {
                ExpectedMessage = "puzzle_3_solved",
                Subscribers = MakeList(mqttSubscriber_abyss)
            };

            sp_mqttSubscriber1.Initialize();
            sp_mqttSubscriber2.Initialize();
            sp_mqttSubscriber3.Initialize();
            AbyssSystem.Instance.RegisterSubProcessor(sp_mqttSubscriber1);
            AbyssSystem.Instance.RegisterSubProcessor(sp_mqttSubscriber2);
            AbyssSystem.Instance.RegisterSubProcessor(sp_mqttSubscriber3);

            SPDebug sp_debug = new SPDebug()
            {
                Message = "test_debug"
            };

            sp_debug.Initialize();
            AbyssSystem.Instance.RegisterSubProcessor(sp_debug);

            sp_mqttSubscriber1.ExpectedMessageReceived += sp_debug.Start;
            sp_mqttSubscriber2.ExpectedMessageReceived += sp_debug.Start;
            sp_mqttSubscriber3.ExpectedMessageReceived += sp_debug.Start;

        }

        private List<T> MakeList<T>(params T[] listItems)
        {
            List<T> list = new List<T>();
            list.AddRange(listItems);
            return list;
        }
    }
}
