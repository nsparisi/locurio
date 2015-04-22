using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbyssLibrary;

namespace Abyss
{
    public class AbyssSystem
    {
        public List<ISubProcessor> SubProcessors { get; private set; }
        public List<IPhysicalObject> PhysicalObjects { get; private set; }
        public List<IClientConsole> Clients { get; private set; }

        private static AbyssSystem instance;
        public static AbyssSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AbyssSystem();
                }

                return instance;
            }
        }

        private AbyssSystem()
        {
            SubProcessors = new List<ISubProcessor>();
            PhysicalObjects = new List<IPhysicalObject>();
            Clients = new List<IClientConsole>();
        }

        public void RegisterSubProcessor(ISubProcessor subProcessor)
        {
            if (!SubProcessors.Contains(subProcessor))
            {
                SubProcessors.Add(subProcessor);
            }
        }

        public void RegisterPhysicalObject(IPhysicalObject physicalObject)
        {
            if (!PhysicalObjects.Contains(physicalObject))
            {
                PhysicalObjects.Add(physicalObject);
            }
        }

        public void RegisterClientConsole(IClientConsole client)
        {
            if (!Clients.Contains(client))
            {
                Clients.Add(client);
            }
        }

        public void UnregisterClientConsole(IClientConsole client)
        {
            if (Clients.Contains(client))
            {
                Clients.Remove(client);
            }
        }

        private void CleanSystem()
        {
            this.PhysicalObjects.Clear();
            this.SubProcessors.Clear();
        }

        public void RunConfiguration(AbyssRunner runner)
        {
            CleanSystem();
            runner.Run();
        }
    }
}
