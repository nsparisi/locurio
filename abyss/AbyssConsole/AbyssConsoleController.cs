using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abyss;
using System.Windows;

namespace AbyssConsole
{
    public class AbyssConsoleController
    {
        MainWindow mainWindow;

        public void Start(MainWindow mainWindow)
        {
            AbyssRunner runner = new AbyssRunner();
            runner.Run();

            this.mainWindow = mainWindow;
        }

        public void Refresh()
        {
            this.mainWindow.ClearScreen();
            foreach(var physicalObject in AbyssSystem.Instance.PhysicalObjects)
            {
                this.mainWindow.AddPhysicalObject(physicalObject);
            }
        }

        public void ExitProgram()
        {

        }
    }
}
