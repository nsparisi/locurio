using AbyssLibrary;
using AbyssScreen;
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
    /// Interaction logic for RoomView.xaml
    /// </summary>
    public partial class HomeSubPage : UserControl
    {
        DashboardUserControl gameDashboard;
        DashboardUserControl hintDashboard;
        DashboardUserControl soundDashboard;
        DashboardUserControl lightDashboard;
        DashboardUserControl settingsDashboard;
        DashboardUserControl deviceDashboard;
        DashboardUserControl scriptingDashboard;
        DashboardUserControl debugDashboard;

        public HomeSubPage()
        {
            InitializeComponent();

            gameDashboard = new DashboardUserControl();
            gameDashboard.SetupTile(DashboardUserControl.IconType.GameControlIcon, "GAME CONTROL");
            gameDashboard.MouseDown += Game_Click;
            this.WrapPanel.Children.Add(gameDashboard);

            hintDashboard = new DashboardUserControl();
            hintDashboard.SetupTile(DashboardUserControl.IconType.HintsIcon, "HINTS");
            hintDashboard.MouseDown += Hint_Click;
            this.WrapPanel.Children.Add(hintDashboard);

            soundDashboard = new DashboardUserControl();
            soundDashboard.SetupTile(DashboardUserControl.IconType.SoundIcon, "SOUND");
            soundDashboard.MouseDown += Sound_Click;
            this.WrapPanel.Children.Add(soundDashboard);

            lightDashboard = new DashboardUserControl();
            lightDashboard.SetupTile(DashboardUserControl.IconType.LightsIcon, "LIGHTING");
            lightDashboard.MouseDown += Lighting_Click;
            lightDashboard.FadeTile();
            this.WrapPanel.Children.Add(lightDashboard);

            settingsDashboard = new DashboardUserControl();
            settingsDashboard.SetupTile(DashboardUserControl.IconType.SettingsIcon, "SETTINGS");
            settingsDashboard.MouseDown += Setting_Click;
            settingsDashboard.FadeTile();
            this.WrapPanel.Children.Add(settingsDashboard);

            deviceDashboard = new DashboardUserControl();
            deviceDashboard.SetupTile(DashboardUserControl.IconType.DeviceManagerIcon, "DEVICE MANAGER");
            deviceDashboard.MouseDown += Device_Click;
            this.WrapPanel.Children.Add(deviceDashboard);

            scriptingDashboard = new DashboardUserControl();
            scriptingDashboard.SetupTile(DashboardUserControl.IconType.ScriptingIcon, "SCRIPTING");
            scriptingDashboard.MouseDown += Scripting_Click;
            scriptingDashboard.FadeTile();
            this.WrapPanel.Children.Add(scriptingDashboard);

            debugDashboard = new DashboardUserControl();
            debugDashboard.SetupTile(DashboardUserControl.IconType.DebugIcon, "DEBUG / TEST");
            debugDashboard.MouseDown += Debug_Click;
            debugDashboard.FadeTile();
            this.WrapPanel.Children.Add(debugDashboard);
        }

        public void Refresh()
        {
            gameDashboard.Refresh();
            hintDashboard.Refresh();
            soundDashboard.Refresh();
            lightDashboard.Refresh();
            settingsDashboard.Refresh();
            deviceDashboard.Refresh();
            scriptingDashboard.Refresh();
            debugDashboard.Refresh();
        }

        private void Game_Click(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            app.RootWindow.SwapToGameControlView();
        }

        private void Hint_Click(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            app.RootWindow.SwapToHintView();
        }

        private void Sound_Click(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            app.RootWindow.SwapToSoundView();
        }

        private void Lighting_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Device_Click(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            app.RootWindow.SwapToRoomView();
        }

        private void Scripting_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Debug_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
