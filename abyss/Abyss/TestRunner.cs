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
            //MqttSubscriber mqttSubscriber_abyss = new MqttSubscriber("/locurio/librariansguild", "abyss", "94-DE-80-07-BD-AF", "192.168.50.50");
            MqttBroker mqttBroker = new MqttBroker("Abyss-TheCoordinator", "B8:27:EB:46:1A:AA", "192.168.50.50");
            AbyssSystem.Instance.RegisterPhysicalObject(mqttBroker);

            SPMqttSubscriber sp_mqttSubscriber1 = new SPMqttSubscriber()
            {
                Brokers = MakeList(mqttBroker),
                Topic = "/puzzle/one/two/three",
                ExpectedMessage = "puzzle_1_solved"
            };
            sp_mqttSubscriber1.Initialize();
            AbyssSystem.Instance.RegisterSubProcessor(sp_mqttSubscriber1);

            SPDebug sp_debug = new SPDebug()
            {
                Message = "test_debug"
            };
            sp_debug.Initialize();
            AbyssSystem.Instance.RegisterSubProcessor(sp_debug);

            SPMqttPublisher sp_mqttPublisher1 = new SPMqttPublisher()
            {
                Brokers = MakeList(mqttBroker),
                Topic = "/debug/verbose",
                Message = "abyss here, hello!"
            };
            sp_mqttPublisher1.Initialize();
            AbyssSystem.Instance.RegisterSubProcessor(sp_mqttPublisher1);

            sp_mqttSubscriber1.ExpectedMessageReceived += sp_debug.Start;
            sp_mqttSubscriber1.ExpectedMessageReceived += sp_mqttPublisher1.Start;
        }

        private List<T> MakeList<T>(params T[] listItems)
        {
            List<T> list = new List<T>();
            list.AddRange(listItems);
            return list;
        }
    }
}
