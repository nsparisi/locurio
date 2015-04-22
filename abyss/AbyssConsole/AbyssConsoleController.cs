using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abyss;
using System.Windows;

namespace AbyssConsole
{
    public class AbyssConsoleController : IClientConsole
    {
        MainWindow mainWindow;

        public void Start(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

            AbyssSystem.Instance.RunConfiguration(new AbyssRunner());
            AbyssSystem.Instance.RegisterClientConsole(this);
        }

        public void Refresh()
        {
            this.mainWindow.Refresh();
        }

        public void ExitProgram()
        {
            AbyssSystem.Instance.UnregisterClientConsole(this);
        }
    }
}
