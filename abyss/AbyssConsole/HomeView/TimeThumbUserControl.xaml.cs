using AbyssLibrary;
using AbyssScreen;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for PhysicalObjectUserControl.xaml
    /// </summary>
    public partial class TimeThumbUserControl : AbstractThumbnailUserControl
    {
        CountDownTimer countdownTimer;

        public TimeThumbUserControl()
        {
            InitializeComponent();
        }

        public void AddClock(CountDownTimer timer)
        {
            this.countdownTimer = timer;
        }

        public override void Refresh()
        {
            if(countdownTimer != null)
            {
                TimeLabel.Text = ClockWindow.GetPrettyTimeText(countdownTimer.GetTimeRemaining());
            }
            else
            {
                TimeLabel.Text = "No Clock Found";
            }
        }
    }
}
