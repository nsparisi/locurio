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
    public partial class SoundSubPage : UserControl
    {
        private List<VLCServerControl> vlcServers;
        private List<string> serverNamesList;

        public SoundSubPage()
        {
            InitializeComponent();
            vlcServers = new List<VLCServerControl>();
            serverNamesList = new List<string>();
            this.ServerListBox.ItemsSource = serverNamesList;
        }

        public void AddVlcServer(VLCServerControl vlcServer)
        {
            if(!vlcServers.Contains(vlcServer))
            {
                vlcServers.Add(vlcServer);

                serverNamesList.Add(string.Format("{0}", vlcServer.Name));
                ServerListBox.Items.Refresh();
            }
        }

        public void Refresh()
        { 

        }
        
        private void Go_Click(object sender, RoutedEventArgs e)
        {
            if (vlcServers.Count > 0 && ServerListBox.SelectedIndex >= 0)
            {
                VLCServerControl server = vlcServers[ServerListBox.SelectedIndex];

                Uri serverUri;
                if (server.IsConnected && Uri.TryCreate(server.ServerURL, UriKind.Absolute, out serverUri))
                {
                    this.VlcBrowser.Navigate(serverUri);
                    this.UrlLabel.Content = serverUri;
                }
            }
        }
    }
}
