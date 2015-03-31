using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AbyssMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Closing += OnWindowClosing;
        }

        private void SpawnWindow_Click(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            app.Controller.SpawnClock();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            App app = (App)Application.Current;
            app.Controller.ExitProgram();
        }

        private void StartCountdown_Click(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            app.Controller.StartCountDown();
        }
    }
}
