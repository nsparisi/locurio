using AbyssLibrary;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for PhysicalObjectUserControl.xaml
    /// </summary>
    public partial class SoundUserControl : AbstractPhysicalObjectUserControl
    {
        VLCServerControl soundController;

        // creating a SP to handle interacting with the hardware object
        // it's convinient since it handles threading for me already
        SPSoundControl soundControllerSubProcess;

        public SoundUserControl()
        {
            InitializeComponent();
        }

        public SoundUserControl(VLCServerControl soundController)
            : this()
        {
            this.soundController = soundController;
            this.soundControllerSubProcess = new SPSoundControl()
            {
                VLCControllers = new List<VLCServerControl>()
                {
                    this.soundController
                }
            };

            Refresh();
        }

        public override void Refresh()
        {
            if (this.soundController != null)
            {
                this.NameText.Content = this.soundController.Name;
                this.IPAddressField.Content = this.soundController.IpAddress;
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (this.soundController != null)
            {
                this.soundControllerSubProcess.SongFileName = FileName.Text;
                this.soundControllerSubProcess.Play(this, EventArgs.Empty);
            }
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if (this.soundController != null)
            {
                this.soundControllerSubProcess.Pause(this, EventArgs.Empty);
            }
        }

        private void IPRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (this.soundController != null)
            {
                // hard refresh IP address if non-existent
                if (!this.soundController.IsConnected)
                {
                    this.soundController.RefreshIpAddress(true);
                }
            }
        }

        private void SetVolume_Click(object sender, RoutedEventArgs e)
        {
            if (this.soundController != null)
            {
                float volume;
                if (float.TryParse(VolumeBox.Text, out volume))
                {
                    this.soundControllerSubProcess.Volume = volume;
                    this.soundControllerSubProcess.SetVolume(this, EventArgs.Empty);
                }
            }
        }
    }
}
