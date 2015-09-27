using Abyss;
using AbyssLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    public partial class GameControlPage : UserControl
    {
        private const int DefaultTime70Minutes = 70;

        public ClockController clockController;
        public AbyssGameController gameController;
        public XBeeEndpoint altarXbeeEndpoint;

        private SPTimerController controllerSubProcessor;
        bool shouldRefreshDevice;

        bool shouldPrintPreviousTime = false;

        public GameControlPage()
        {
            this.controllerSubProcessor = new SPTimerController();
            InitializeComponent();
        }

        public void AddTimerController(TimerController timerController)
        {
            this.controllerSubProcessor.TimerControllers.Add(timerController);
            RefreshDeviceField();
        }

        public void AddClock(AbyssScreenController screenController)
        {
            clockController = new ClockController(screenController);
        }

        public void AddGame(AbyssGameController gameController)
        {
            this.gameController = gameController;
        }

        public void AddAltarXbee(XBeeEndpoint xbeeEndpoint)
        {
            this.altarXbeeEndpoint = xbeeEndpoint;
        }

        public void Refresh()
        {
            if(gameController != null)
            {
                if (gameController.GameState == AbyssGameController.SPGameStateType.NotRunning)
                {
                    this.StatusLabel.Content = "NO GAME IS RUNNING";

                    if (shouldPrintPreviousTime && clockController != null)
                    {
                        shouldPrintPreviousTime = false;
                        string previousTime = ClockController.GetPrettyTimeText(clockController.GetTimeRemaining());
                        Debug.Log("Previous time ended with {0} remaining.", previousTime);
                        this.PreviousTime.Content = string.Format("Previous time ended with {0} remaining.", previousTime);
                    }
                }
                else if (gameController.GameState == AbyssGameController.SPGameStateType.Running)
                {
                    this.StatusLabel.Content = "RUNNING";
                    shouldPrintPreviousTime = true;
                }
                else if (gameController.GameState == AbyssGameController.SPGameStateType.Paused)
                {
                    this.StatusLabel.Content = "PAUSED";
                }
                else if (gameController.GameState == AbyssGameController.SPGameStateType.Testing)
                {
                    this.StatusLabel.Content = "TEST MODE";
                }

                this.StartButton.Opacity = gameController.CanStart ? 1 : 0.2;
                this.WinButton.Opacity = gameController.CanWin ? 1 : 0.2;
                this.LoseButton.Opacity = gameController.CanLose ? 1 : 0.2;
                this.PauseButton.Opacity = gameController.CanPause ? 1 : 0.2;
                this.UnPauseButton.Opacity = gameController.CanUnPause? 1 : 0.2;
                this.TestButton.Opacity = gameController.CanEnterTestMode ? 1 : 0.2;
                this.AltarTopButton.Opacity = gameController.CanWin ? 1 : 0.2;
            }

            if (shouldRefreshDevice)
            {
                shouldRefreshDevice = false;
                RefreshDeviceField();
            }
        }

        private void SetClockUsingMinutesBox()
        {
            // try to read the value in the minutes box and use that
            // otherwise default
            int minutes = TryParseMinutesBox();

            if (clockController != null)
            {
                Debug.Log("Setting time as: m {0}", minutes);
                clockController.SetTime(minutes);
            }

            if (controllerSubProcessor != null)
            {
                controllerSubProcessor.TimeInMilliseconds = minutes * 60 * 1000;
                controllerSubProcessor.SetTime(this, EventArgs.Empty);
            }
        }

        private int TryParseMinutesBox()
        {
            // try to read the value in the minutes box and use that
            // otherwise default
            int minutes;
            string minutesString = this.MinutesBox.Text;
            if (int.TryParse(minutesString, out minutes))
            {
                return minutes;
            }
            else
            {
                Debug.Log("Please specify a valid number. {0} is invalid.", minutesString);
            }

            return DefaultTime70Minutes;
        }

        private void SetClock_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = 
                System.Windows.MessageBox.Show(
                "Are you sure you want to set the game clock?", 
                "Confirm Set Clock", 
                System.Windows.MessageBoxButton.OKCancel);

            if (messageBoxResult == MessageBoxResult.OK || messageBoxResult == MessageBoxResult.Yes)
            {
                SetClockUsingMinutesBox();
            }
        }

        private void GameStart_Click(object sender, RoutedEventArgs e)
        {
            if (gameController != null && gameController.CanStart)
            {
                MessageBoxResult messageBoxResult = 
                    System.Windows.MessageBox.Show(
                    "Are you sure you want to start the game? This will start the game clock and soundtrack, and change light settings.", 
                    "Confirm Start", 
                    System.Windows.MessageBoxButton.OKCancel);

                if (messageBoxResult == MessageBoxResult.OK || messageBoxResult == MessageBoxResult.Yes)
                {
                    // reset all processors, stop them if they are running
                    AbyssSystem.Instance.DisableAllSubProcessors();
                    Thread.Sleep(500);
                    AbyssSystem.Instance.EnableAllSubProcessors();
                    Thread.Sleep(200);
                    SetClockUsingMinutesBox();
                    gameController.Start();
                }
            }
        }

        private void GameStop_Click(object sender, RoutedEventArgs e)
        {
            if (gameController != null && gameController.CanStop)
            {
                MessageBoxResult messageBoxResult =
                    System.Windows.MessageBox.Show(
                    "Are you sure you want to stop and reset the game? This will stop the game clock and soundtrack, and return light settings to normal.",
                    "Confirm Stop",
                    System.Windows.MessageBoxButton.OKCancel);

                if (messageBoxResult == MessageBoxResult.OK || messageBoxResult == MessageBoxResult.Yes)
                {
                    Debug.Log("Countdown timer was stopped at {0} ",
                        ClockController.GetPrettyTimeText(clockController.GetTimeRemaining()));

                    // reset all processors, stop them if they are running
                    AbyssSystem.Instance.DisableAllSubProcessors();
                    Thread.Sleep(500);
                    AbyssSystem.Instance.EnableAllSubProcessors();
                    gameController.Stop();

                    // reset the clock
                    if (controllerSubProcessor != null)
                    {
                        int minutes = TryParseMinutesBox();
                        controllerSubProcessor.TimeInMilliseconds = minutes * 60 * 1000;
                        controllerSubProcessor.SetTime(this, EventArgs.Empty);
                    }
                }
            }
        }

        private void GameWin_Click(object sender, RoutedEventArgs e)
        {
            if (gameController != null && gameController.CanWin)
            {
                MessageBoxResult messageBoxResult =
                    System.Windows.MessageBox.Show(
                    "Are you sure you want to win the game? This will perform the end-of-game win sequence including playing audio dialogue and changing lights and stopping the game clock.",
                    "Confirm Win",
                    System.Windows.MessageBoxButton.OKCancel);

                if (messageBoxResult == MessageBoxResult.OK || messageBoxResult == MessageBoxResult.Yes)
                {
                    gameController.Win();
                }
            }
        }

        private void GameLose_Click(object sender, RoutedEventArgs e)
        {
            if (gameController != null && gameController.CanLose)
            {
                MessageBoxResult messageBoxResult =
                    System.Windows.MessageBox.Show(
                    "Are you sure you want to lose the game? This will perform the end-of-game lose sequence including playing audio dialogue and changing lights.",
                    "Confirm Lose",
                    System.Windows.MessageBoxButton.OKCancel);
                
                if (messageBoxResult == MessageBoxResult.OK || messageBoxResult == MessageBoxResult.Yes)
                {
                    gameController.Lose();
                }
            }
        }

        private void GamePause_Click(object sender, RoutedEventArgs e)
        {
            if (gameController != null && gameController.CanPause)
            {
                MessageBoxResult messageBoxResult =
                    System.Windows.MessageBox.Show(
                    "Are you sure you want to pause the game? This will pause all audio and the game clock.",
                    "Confirm Pause",
                    System.Windows.MessageBoxButton.OKCancel);
                
                if (messageBoxResult == MessageBoxResult.OK || messageBoxResult == MessageBoxResult.Yes)
                {
                    gameController.Pause();
                }
            }
        }

        private void GameUnPause_Click(object sender, RoutedEventArgs e)
        {
            if (gameController != null && gameController.CanUnPause)
            {
                MessageBoxResult messageBoxResult =
                    System.Windows.MessageBox.Show(
                    "Are you sure you want to unpause the game? This will resume all audio and the game clock.",
                    "Confirm Un-Pause",
                    System.Windows.MessageBoxButton.OKCancel);
                
                if (messageBoxResult == MessageBoxResult.OK || messageBoxResult == MessageBoxResult.Yes)
                {
                    gameController.UnPause();
                }
            }
        }

        private void TestMode_Click(object sender, RoutedEventArgs e)
        {
            if (gameController != null && gameController.CanEnterTestMode)
            {
                MessageBoxResult messageBoxResult =
                    System.Windows.MessageBox.Show(
                    "Are you sure you want to start test mode? This will play test audio on all media servers, and change the color of lights.",
                    "Confirm Test Mode",
                    System.Windows.MessageBoxButton.OKCancel);

                if (messageBoxResult == MessageBoxResult.OK || messageBoxResult == MessageBoxResult.Yes)
                {
                    gameController.EnterTestMode();
                }
            }
        }

        // todo: this is a hacky fix for handling lost Altar communication
        private void AltarTopSolved_Click(object sender, RoutedEventArgs e)
        {
            if (altarXbeeEndpoint != null && gameController.CanWin)
            {
                MessageBoxResult messageBoxResult =
                    System.Windows.MessageBox.Show(
                    "Are you sure you want to run the Altar Top Solve event? This will play the wind effects and change the lights in the secret room.",
                    "Confirm Altar Top Solve",
                    System.Windows.MessageBoxButton.OKCancel);

                if (messageBoxResult == MessageBoxResult.OK || messageBoxResult == MessageBoxResult.Yes)
                {
                    altarXbeeEndpoint.DebugImpersonateMessage("TOPSOLVED");
                }
            }
        }

        private void RefreshDeviceField()
        {
            string devices = "";
            for (int i = 0; i < this.controllerSubProcessor.TimerControllers.Count; i++)
            {
                if (!this.controllerSubProcessor.TimerControllers[i].IsConnected)
                {
                    shouldRefreshDevice = true;
                }

                if (i > 0)
                {
                    devices += ", ";
                }

                devices += string.Format(
                    "{0} ({1})",
                    this.controllerSubProcessor.TimerControllers[i].Name,
                    this.controllerSubProcessor.TimerControllers[i].IpAddress);
            }

            this.DeviceField.Content = devices;
        }

        private void IPRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (this.controllerSubProcessor != null)
            {
                foreach (var timerDevice in this.controllerSubProcessor.TimerControllers)
                {
                    // hard refresh IP address if non-existent
                    if (!timerDevice.IsConnected)
                    {
                        timerDevice.RefreshIpAddress(true);
                    }
                }
            }
        }
    }
}
