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
    public partial class HintSubPage : UserControl
    {
        private List<SPTextingController> controllerSubProcessors;
        private List<string> messageHistory;

        public HintSubPage()
        {
            this.controllerSubProcessors = new List<SPTextingController>();
            this.messageHistory = new List<string>();
            InitializeComponent();
            PopulateHints();
        }

        private void PopulateHints()
        {
            string[] hints = new[]
            {
                "You should go to there.",
                "This is a valuable hint.",
                "Help me obi-wan",
                "Another really good one",
                "Noxamillion always likes to use a live rabbit in his act. But sometimes the rabbit eats the keys to the locks!"
            };

            this.SuggestionList.SelectedItemChanged += Suggestion_Selected;
        }

        bool shouldRefreshDevice;
        public void AddTextingController(TextingController textingController)
        {
            SPTextingController newController = new SPTextingController();
            newController.TextingControllers = new List<TextingController>(new[] { textingController });
            this.controllerSubProcessors.Add(newController);

            RefreshDeviceField();
        }

        public void Refresh()
        {
            if (shouldRefreshDevice)
            {
                shouldRefreshDevice = false;
                RefreshDeviceField();
            }
        }

        private void RefreshDeviceField()
        {
            string devices = "";
            for (int i = 0; i < this.controllerSubProcessors.Count; i++)
            {
                TextingController controller = this.controllerSubProcessors[i].TextingControllers.First();
                if (!controller.IsConnected)
                {
                    shouldRefreshDevice = true;
                }

                if (i > 0)
                {
                    devices += ", ";
                }

                devices += string.Format(
                    "{0} ({1})",
                    controller.Name,
                    controller.IpAddress);
            }

            this.DeviceField.Content = devices;
        }

        private void Suggestion_Selected(object sender, RoutedEventArgs e)
        {
            if (this.SuggestionList.SelectedItem != null)
            {
                TreeViewItem selection = (TreeViewItem)this.SuggestionList.SelectedItem;
                if (selection != null)
                {
                    this.TextBox.Text = selection.Header.ToString();
                }
            }
        }

        private void SendHint_Click(object sender, RoutedEventArgs e)
        {
            if (controllerSubProcessors != null && !string.IsNullOrWhiteSpace(this.TextBox.Text))
            {
                foreach (SPTextingController controller in controllerSubProcessors)
                {
                    controller.TextMessage = this.TextBox.Text;
                    controller.SendTextMessage(this, EventArgs.Empty);
                }

                // add a prefix to message history to show time remaining
                string prefix = string.Empty;
                App app = (App)Application.Current;
                if (app != null && app.RootWindow != null)
                {
                    prefix = string.Format("[{0}] ", app.RootWindow.GetPrettyTimeRemaining());
                }

                messageHistory.Add(prefix + this.TextBox.Text);
                this.TextBox.Clear();
                UpdateHistory();

                // clear current selection
                TreeViewItem selection = (TreeViewItem)this.SuggestionList.SelectedItem;
                if (selection != null)
                {
                    selection.IsSelected = false;
                    SuggestionList.Focus();
                }
            }
        }

        private void ClearMessageHistory_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult =
                System.Windows.MessageBox.Show(
                "Are you sure you want to clear the hint history? This will clear all the hints from the HINT PHONES.",
                "Confirm Clear History",
                System.Windows.MessageBoxButton.OKCancel);

            if (!(messageBoxResult == MessageBoxResult.OK || messageBoxResult == MessageBoxResult.Yes))
            {
                return;
            }

            if (controllerSubProcessors != null)
            {
                foreach (SPTextingController controller in controllerSubProcessors)
                {
                    controller.ClearHistory(this, EventArgs.Empty);
                }
            }

            messageHistory.Clear();
            UpdateHistory();
        }

        private void UpdateHistory()
        {
            this.SentHistory.ItemsSource = messageHistory;
            this.SentHistory.Items.Refresh();
        }

        private void IPRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (this.controllerSubProcessors != null)
            {
                foreach (SPTextingController controller in controllerSubProcessors)
                {
                    // hard refresh IP address if non-existent
                    if (!controller.TextingControllers.First().IsConnected)
                    {
                        controller.TextingControllers.First().RefreshIpAddress(true);
                    }
                }
            }
        }
    }
}
