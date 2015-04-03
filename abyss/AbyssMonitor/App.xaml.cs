using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AbyssScreen
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public AppController Controller { get; private set; }

        public App() : this(new AppController())
        {
        }

        public App(AppController appController) : base()
        {
            this.Controller = appController;
        }
    }
}
