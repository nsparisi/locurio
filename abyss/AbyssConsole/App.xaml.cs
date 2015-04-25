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
        public AbyssConsoleController Controller { get; private set; }

        public MainWindow RootWindow { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            RootWindow = new MainWindow();
            RootWindow.Show();

            Controller = new AbyssConsoleController();
            Controller.Start(this);
        }
    }

}
