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
    public partial class LightBulbUserControl : AbstractPhysicalObjectUserControl
    {
        LimitlessLEDBridge ledBridge;

        public LightBulbUserControl()
        {
            InitializeComponent();
        }

        public LightBulbUserControl(LimitlessLEDBridge ledBridge)
            : this()
        {
            this.ledBridge = ledBridge;
            Refresh();
        }

        public override void Refresh()
        {
            if (this.ledBridge != null)
            {
                this.NameText.Content = this.ledBridge.Name;

                if (this.ledBridge.IsConnected)
                {
                    this.IPAddressField.Content = this.ledBridge.IpAddress;
                }
            }
        }
        
        private void IPRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (this.ledBridge != null)
            {
                // hard refresh IP address if non-existent
                if (!this.ledBridge.IsConnected)
                {
                    this.ledBridge.RefreshIpAddress(true);
                }
            }
        }

        private void AllOn_Click(object sender, RoutedEventArgs e)
        {
            if (this.ledBridge != null)
            {
                ledBridge.TurnOn(LimitlessLEDBridge.ZoneType.All);
            }
        }

        private void AllOff_Click(object sender, RoutedEventArgs e)
        {
            if (this.ledBridge != null)
            {
                ledBridge.TurnOff(LimitlessLEDBridge.ZoneType.All);
            }
        }
    }
}
