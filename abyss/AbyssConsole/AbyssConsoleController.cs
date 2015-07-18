using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abyss;
using System.Windows;
using System.Threading;
using AbyssLibrary;

namespace AbyssConsole
{
    public class AbyssConsoleController : IClientConsole
    {
        App abyssConsoleApp;
        Thread refresher;

        public void Start(App abyssConsoleApp)
        {
            this.abyssConsoleApp = abyssConsoleApp;

            // for v1.0, AbyssConsole will create AbyssSystem.
            AbyssSystem.Instance.RunConfiguration(new AbyssRunner());
            AbyssSystem.Instance.RegisterClientConsole(this);

            // handle physical objects
            foreach (var physicalObject in AbyssSystem.Instance.PhysicalObjects)
            {
                if (physicalObject is AbyssScreenController)
                {
                    abyssConsoleApp.RootWindow.TimeView.AddClock((AbyssScreenController)physicalObject);
                    abyssConsoleApp.RootWindow.AddClock(((AbyssScreenController)physicalObject).CountDownTimer);
                }

                if(physicalObject is TextingController)
                {
                    abyssConsoleApp.RootWindow.HintView.AddTextingController((TextingController)physicalObject);
                }

                abyssConsoleApp.RootWindow.RoomView.AddPhysicalObject(physicalObject);
            }

            // set up fast refresher
            refresher = new Thread(Refresh);
            refresher.IsBackground = true;
            refresher.Start();
        }

        public void Refresh()
        {
            while (true)
            {
                abyssConsoleApp.RootWindow.Refresh();
                Thread.Sleep(100);
            }
        }

        public void ExitProgram()
        {
            AbyssSystem.Instance.UnregisterClientConsole(this);
        }
    }
}
