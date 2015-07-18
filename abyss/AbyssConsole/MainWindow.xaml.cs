using AbyssLibrary;
using AbyssScreen;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
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
    public partial class MainWindow : Window
    {
        public RoomSubPage RoomView { get; private set; }
        public HomeSubPage HomeView { get; private set; }
        public ClockSubPage TimeView { get; private set; }
        public HintSubPage HintView { get; private set; }

        CountDownTimer countdownTimer;
        List<string> logHistory;
        List<string> logCache;

        static object lockObj = new object();

        bool isExiting;

        public MainWindow()
        {
            InitializeComponent();

            this.Closing += OnWindowClosing;

            this.HomeView = new HomeSubPage();
            this.RoomView = new RoomSubPage();
            this.TimeView = new ClockSubPage();
            this.HintView = new HintSubPage();

            SwapToHomeView();

            // listen for logging
            logCache = new List<string>();
            logHistory = new List<string>();
            Debug.LogMessageEvent += LogReceived;
            this.ActivityLogBox.ItemsSource = logHistory;
        }

        public void Refresh()
        {
            if (isExiting)
            {
                return;
            }

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                RoomView.Refresh();
                HomeView.Refresh();

                lock (lockObj)
                {
                    if (logCache.Any())
                    {
                        bool shouldScroll = (this.ActivityLogBox.Items.Count - 1) == this.ActivityLogBox.SelectedIndex;
                        int lastItemIndex = this.ActivityLogBox.Items.Count - 1;

                        logHistory.AddRange(logCache);
                        logCache.Clear();
                        this.ActivityLogBox.Items.Refresh();

                        if(shouldScroll)
                        {
                            this.ActivityLogBox.SelectedIndex = this.ActivityLogBox.Items.Count - 1;
                            this.ActivityLogBox.ScrollIntoView(this.ActivityLogBox.SelectedItem);
                        }
                    }
                }

                if (countdownTimer != null)
                {
                    ClockLabel.Content = ClockController.GetPrettyTimeText(countdownTimer.GetTimeRemaining());
                }
            });
        }

        public void SwapToHomeView()
        {
            this.SubViewGrid.Children.Clear();
            this.SubViewGrid.Children.Add(HomeView);
        }

        public void SwapToRoomView()
        {
            this.SubViewGrid.Children.Clear();
            this.SubViewGrid.Children.Add(RoomView);
        }

        public void SwapToTimeView()
        {
            this.SubViewGrid.Children.Clear();
            this.SubViewGrid.Children.Add(TimeView);
        }

        public void SwapToHintView()
        {
            this.SubViewGrid.Children.Clear();
            this.SubViewGrid.Children.Add(HintView);
        }

        void OnWindowClosing(object sender, CancelEventArgs e)
        {
            App app = (App)Application.Current;
            app.Controller.ExitProgram();

            isExiting = true;
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            SwapToHomeView();
        }

        public void AddClock(CountDownTimer timer)
        {
            this.countdownTimer = timer;
        }

        public void LogReceived(object o, EventArgs args)
        {
            lock (lockObj)
            {
                AbyssLibrary.Debug.LogEventArgs logArgs = (AbyssLibrary.Debug.LogEventArgs)args;
                
                logCache.Add(string.Format("[{0}] {1}", logArgs.Timestamp.ToString("H:mm:ss"), logArgs.Message));
            }
        }
    }
}
