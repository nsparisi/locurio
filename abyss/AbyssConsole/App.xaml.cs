using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AbyssConsole
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        Thread refresher;
        public AbyssConsoleController Controller { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            Controller = new AbyssConsoleController();
            Controller.Start(mainWindow);

            refresher = new Thread(Refresh);
            refresher.IsBackground = true;
            refresher.Start();
        }

        void Refresh()
        {
            while(true)
            {
                Controller.Refresh();
                Thread.Sleep(100);
            }
        }
    }

}
