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
        MainWindow mainWindow;
        Thread refresher;

        public void Start(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

            // for v1.0, AbyssConsole will create AbyssSystem.
            AbyssSystem.Instance.RunConfiguration(new AbyssRunner());
            AbyssSystem.Instance.RegisterClientConsole(this);

            // handle physical objects
            foreach (var physicalObject in AbyssSystem.Instance.PhysicalObjects)
            {
                if (physicalObject is AbyssScreenController)
                {
                    mainWindow.TimeView.AddClockController(
                        new ClockController((AbyssScreenController)physicalObject));
                }

                mainWindow.RoomView.AddPhysicalObject(physicalObject);
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
                this.mainWindow.Refresh();
                Thread.Sleep(100);
            }
        }

        public void ExitProgram()
        {
            AbyssSystem.Instance.UnregisterClientConsole(this);

            mainWindow.TimeView.CloseAllClockWindows();
        }
    }
}
