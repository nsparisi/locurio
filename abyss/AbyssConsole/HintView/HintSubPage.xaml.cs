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
        private SPTextingController controllerSubProcessor;
        private List<string> messageHistory;

        public HintSubPage()
        {
            this.controllerSubProcessor = new SPTextingController();
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
            this.controllerSubProcessor.TextingControllers.Add(textingController);
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
            for (int i = 0; i < this.controllerSubProcessor.TextingControllers.Count; i++)
            {
                if (!this.controllerSubProcessor.TextingControllers[i].IsConnected)
                {
                    shouldRefreshDevice = true;
                }

                if (i > 0)
                {
                    devices += ", ";
                }

                devices += string.Format(
                    "{0} ({1})",
                    this.controllerSubProcessor.TextingControllers[i].Name,
                    this.controllerSubProcessor.TextingControllers[i].IpAddress);
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
            if (controllerSubProcessor != null && !string.IsNullOrWhiteSpace(this.TextBox.Text))
            {
                controllerSubProcessor.TextMessage = this.TextBox.Text;
                controllerSubProcessor.SendTextMessage(this, EventArgs.Empty);
                messageHistory.Add(controllerSubProcessor.TextMessage);
                this.TextBox.Clear();
                UpdateHistory();
            }
        }

        private void ClearMessageHistory_Click(object sender, RoutedEventArgs e)
        {
            if (controllerSubProcessor != null)
            {
                controllerSubProcessor.ClearHistory(this, EventArgs.Empty);
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
            if (this.controllerSubProcessor != null)
            {
                foreach (var textDevice in this.controllerSubProcessor.TextingControllers)
                {
                    // hard refresh IP address if non-existent
                    if (!textDevice.IsConnected)
                    {
                        textDevice.RefreshIpAddress(true);
                    }
                }
            }
        }
    }
}
