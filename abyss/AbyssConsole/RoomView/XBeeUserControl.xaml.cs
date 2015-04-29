using AbyssLibrary;
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
    public partial class XBeeUserControl : AbstractPhysicalObjectUserControl
    {
        XBeeEndpoint endpoint;

        public XBeeUserControl()
        {
            InitializeComponent();
            SendMessageBox.Text = "";
        }

        public XBeeUserControl(XBeeEndpoint endpoint)
            : this()
        {
            this.endpoint = endpoint;
            Refresh();
        }

        public override void Refresh()
        {
            if (this.endpoint != null)
            {
                this.NameText.Content = this.endpoint.Name;
                this.IDText.Content = this.endpoint.EndpointID;
            }
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (this.endpoint != null)
            {
                endpoint.SendData(SendMessageBox.Text);
                SendMessageBox.Text = "";
            }
        }
    }
}
