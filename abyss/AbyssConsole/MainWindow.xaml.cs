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
        public GameControlPage GameControlView { get; private set; }
        public HintSubPage HintView { get; private set; }
        public SoundSubPage SoundView { get; private set; }
        public LightSubPage LightView { get; private set; }

        CountDownTimer countdownTimer;
        AbyssGameController gameController;
        List<string> logHistory;
        List<string> logCache;

        private const string GameStoppedClockString = "-- --";

        static object lockObj = new object();

        bool isExiting;

        public MainWindow()
        {
            InitializeComponent();

            this.Closing += OnWindowClosing;

            this.HomeView = new HomeSubPage();
            this.RoomView = new RoomSubPage();
            this.GameControlView = new GameControlPage();
            this.HintView = new HintSubPage();
            this.SoundView = new SoundSubPage();
            this.LightView = new LightSubPage();

            SwapToHomeView();

            // listen for logging
            logCache = new List<string>();
            logHistory = new List<string>();
            Debug.LogMessageEvent += LogReceived;
            this.ActivityLogBox.ItemsSource = logHistory;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want to quit Abyss?", "Exit Abyss", System.Windows.MessageBoxButton.OKCancel);
            if (messageBoxResult == MessageBoxResult.Cancel ||
                messageBoxResult == MessageBoxResult.No)
            {
                e.Cancel = true;
            }

            base.OnClosing(e);
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
                GameControlView.Refresh();
                HintView.Refresh();
                SoundView.Refresh();
                LightView.Refresh();

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

                if (countdownTimer != null && gameController != null)
                {
                    if (gameController.GameState != AbyssGameController.SPGameStateType.NotRunning)
                    {
                        ClockLabel.Content = ClockController.GetPrettyTimeText(countdownTimer.GetTimeRemaining());
                    }
                    else
                    {
                        ClockLabel.Content = GameStoppedClockString;
                    }
                }
                else
                {
                    ClockLabel.Content = GameStoppedClockString;
                }
            });
        }

        public string GetPrettyTimeRemaining()
        {
            if (countdownTimer != null && gameController != null &&
                gameController.GameState != AbyssGameController.SPGameStateType.NotRunning)
            {
                return ClockController.GetPrettyTimeText(countdownTimer.GetTimeRemaining());
            }

            return GameStoppedClockString;
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

        public void SwapToGameControlView()
        {
            this.SubViewGrid.Children.Clear();
            this.SubViewGrid.Children.Add(GameControlView);
        }

        public void SwapToHintView()
        {
            this.SubViewGrid.Children.Clear();
            this.SubViewGrid.Children.Add(HintView);
        }

        public void SwapToSoundView()
        {
            this.SubViewGrid.Children.Clear();
            this.SubViewGrid.Children.Add(SoundView);
        }

        public void SwapToLightView()
        {
            this.SubViewGrid.Children.Clear();
            this.SubViewGrid.Children.Add(LightView);
        }

        void OnWindowClosing(object sender, CancelEventArgs e)
        {
            App app = (App)Application.Current;
            app.Controller.ExitProgram();

            if (!e.Cancel)
            {
                isExiting = true;
            }
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            SwapToHomeView();
        }

        public void AddClock(CountDownTimer timer)
        {
            this.countdownTimer = timer;
        }

        public void AddGameController(AbyssGameController gameController)
        {
            this.gameController = gameController;
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
