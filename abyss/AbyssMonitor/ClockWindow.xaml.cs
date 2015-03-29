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
using System.Windows.Shapes;

namespace AbyssMonitor
{
    /// <summary>
    /// Interaction logic for Clock.xaml
    /// </summary>
    public partial class ClockWindow : Window
    {
        public ClockWindow()
        {
            InitializeComponent();

            this.MouseDown += Window_MouseDown;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        public void SetTimeText(string time)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                this.Time.Text = time;
            }));
        }

        public void SetTime(long milliseconds)
        {
            long hours = milliseconds / (60 * 60 * 1000);
            long minutes = milliseconds / (60 * 1000);
            long seconds = Ceiling(milliseconds, (1000));
            string prettyTime = string.Format(
                "{0}:{1}:{2}",
                hours.ToString("00"),
                minutes.ToString("00"),
                seconds.ToString("00"));

            SetTimeText(prettyTime);
        }

        private long Ceiling(long dividend, long divisor)
        {
            long quotient = dividend / divisor;
            long remainder = dividend % divisor;

            if(remainder > 0)
            {
                quotient++;
            }

            return quotient;
        }
    }
}
