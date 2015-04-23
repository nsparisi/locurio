using AbyssLibrary;
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

namespace AbyssConsole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ClockSubPage : UserControl
    {
        public ClockController controller;

        public ClockSubPage()
        {
            InitializeComponent();
        }

        public void AddClockController(ClockController controller)
        {
            this.controller = controller;
        }

        public void CloseAllClockWindows()
        {
            if (controller != null)
            {
                controller.CloseAllClockWindows();
            }
        }

        private void SpawnWindow_Click(object sender, RoutedEventArgs e)
        {
            if (controller != null)
            {
                controller.SpawnClock();
            }
        }

        private void StartCountdown_Click(object sender, RoutedEventArgs e)
        {
            if (controller != null)
            {
                controller.StartCountDown();
            }
        }
    }
}
