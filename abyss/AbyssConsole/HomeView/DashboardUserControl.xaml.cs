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
    /// Interaction logic for DashboardUserControl.xaml
    /// </summary>
    public partial class DashboardUserControl : AbstractThumbnailUserControl
    {
        public enum IconType { DebugIcon, DeviceManagerIcon, GameControlIcon, HintsIcon, LightsIcon, ScriptingIcon, SettingsIcon, SoundIcon }
        
        public DashboardUserControl()
        {
            InitializeComponent();
        }

        public void SetupTile(IconType icon, string text)
        {
            TileIcon.Source = (ImageSource)Resources[icon.ToString()];
            TileLabel.Content = text;
        }

        public override void Refresh()
        {
        }

        public void FadeTile()
        {
            Fade.Opacity = 0.5;
        }
    }
}
