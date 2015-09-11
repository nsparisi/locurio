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
    public partial class LightSubPage : UserControl
    {
        private List<LimitlessLEDBridge> ledBridges;
        private List<string> ledBridgeNames;
        private List<LimitlessLEDBridge.ZoneType> zoneNames;

        SPLimitlessLEDBridge bridgeProcessor;

        public LightSubPage()
        {
            InitializeComponent();
            ledBridges = new List<LimitlessLEDBridge>();
            bridgeProcessor = new SPLimitlessLEDBridge();

            ledBridgeNames = new List<string>();
            this.BridgeListBox.ItemsSource = ledBridgeNames;

            zoneNames = new List<LimitlessLEDBridge.ZoneType> {
                LimitlessLEDBridge.ZoneType.All,
                LimitlessLEDBridge.ZoneType.Zone1,
                LimitlessLEDBridge.ZoneType.Zone2,
                LimitlessLEDBridge.ZoneType.Zone3,
                LimitlessLEDBridge.ZoneType.Zone4 };
            this.ZoneListBox.ItemsSource = zoneNames;
            this.ZoneListBox.SelectedIndex = 0;

            this.WhiteButton.MouseDown += White_Click;

            this.AquaButton.MouseDown += Aqua_Click;
            this.BabyBlueButton.MouseDown += BabyBlue_Click;
            this.FuchsiaButton.MouseDown += Fuchsia_Click;
            this.GreenButton.MouseDown += Green_Click;
            this.LimeGreenButton.MouseDown += LimeGreen_Click;
            this.MintButton.MouseDown += Mint_Click;
            this.OrangeButton.MouseDown += Orange_Click;
            this.PinkButton.MouseDown += Pink_Click;
            this.RedButton.MouseDown += Red_Click;
            this.RoyalBlueButton.MouseDown += RoyalBlue_Click;
            this.SeafoamGreenButton.MouseDown += SeafoamGreen_Click;
            this.VioletButton.MouseDown += Violet_Click;
            this.YellowButton.MouseDown += Yellow_Click;
            this.YellowOrangeButton.MouseDown += YellowOrange_Click;
            
        }

        public void AddLEDBridge(LimitlessLEDBridge ledBridge)
        {
            if(!ledBridges.Contains(ledBridge))
            {
                ledBridges.Add(ledBridge);
                ledBridgeNames.Add(string.Format("{0}", ledBridge.Name));

                // if this it the first item added, 
                // go ahead and change the dropdown to select it
                if (ledBridges.Count == 1)
                {
                    BridgeListBox.SelectedIndex = 0;
                }
            }
        }

        public void Refresh()
        {

        }

        private bool PrepareProcessor()
        {
            if (ledBridges.Count <= 0)
            {
                Debug.Log("Error: No LED Bridge were discovered.");
                return false;
            }

            if (BridgeListBox.SelectedIndex < 0)
            {
                Debug.Log("Error: No LED Bridge was selected in the dropdown.");
                return false;
            }

            if (BridgeListBox.SelectedIndex >= ledBridges.Count)
            {
                Debug.Log("Error: Abyss LightBulb-Menu is in a malformed state.");
                return false;
            }

            if(ZoneListBox.SelectedIndex < 0 || ZoneListBox.SelectedIndex >= zoneNames.Count)
            {
                Debug.Log("Error: Zone selection is invalid.");
                return false;
            }
            
            bridgeProcessor.Zones = new List<LimitlessLEDBridge.ZoneType> { zoneNames[ZoneListBox.SelectedIndex] };
            bridgeProcessor.Bridges = new List<LimitlessLEDBridge> { ledBridges[BridgeListBox.SelectedIndex] };
            return true;
        }

        private void BrightnessValue_Changed(object sender, RoutedEventArgs e)
        {
            if (PrepareProcessor())
            {
                bridgeProcessor.Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetBrightness;
                bridgeProcessor.Brightness = BrightnessSlider.Value;
                bridgeProcessor.Run(null, EventArgs.Empty);
            }
        }

        private void On_Click(object sender, RoutedEventArgs e)
        {
            if(PrepareProcessor())
            {
                bridgeProcessor.Command = SPLimitlessLEDBridge.LEDBridgeCommand.TurnOn;
                bridgeProcessor.Run(null, EventArgs.Empty);
            }
        }

        private void Off_Click(object sender, RoutedEventArgs e)
        {
            if (PrepareProcessor())
            {
                bridgeProcessor.Command = SPLimitlessLEDBridge.LEDBridgeCommand.TurnOff;
                bridgeProcessor.Run(null, EventArgs.Empty);
            }
        }

        private void White_Click(object sender, RoutedEventArgs e)
        {
            if (PrepareProcessor())
            {
                bridgeProcessor.Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetToWhite;
                bridgeProcessor.Run(null, EventArgs.Empty);
            }
        }

        private void Fuchsia_Click(object sender, RoutedEventArgs e)
        {
            ChangeColor(LimitlessLEDBridge.ColorType.Fusia);
        }

        private void Aqua_Click(object sender, RoutedEventArgs e)
        {
            ChangeColor(LimitlessLEDBridge.ColorType.Aqua);
        }
        
        private void Green_Click(object sender, RoutedEventArgs e)
        {
            ChangeColor(LimitlessLEDBridge.ColorType.Green);
        }

        private void LimeGreen_Click(object sender, RoutedEventArgs e)
        {
            ChangeColor(LimitlessLEDBridge.ColorType.Lime_Green);
        }

        private void Orange_Click(object sender, RoutedEventArgs e)
        {
            ChangeColor(LimitlessLEDBridge.ColorType.Orange);
        }

        private void Pink_Click(object sender, RoutedEventArgs e)
        {
            ChangeColor(LimitlessLEDBridge.ColorType.Pink);
        }

        private void Red_Click(object sender, RoutedEventArgs e)
        {
            ChangeColor(LimitlessLEDBridge.ColorType.Red);
        }

        private void RoyalBlue_Click(object sender, RoutedEventArgs e)
        {
            ChangeColor(LimitlessLEDBridge.ColorType.Royal_Blue);
        }

        private void Violet_Click(object sender, RoutedEventArgs e)
        {
            ChangeColor(LimitlessLEDBridge.ColorType.Violet);
        }

        private void Yellow_Click(object sender, RoutedEventArgs e)
        {
            ChangeColor(LimitlessLEDBridge.ColorType.Yellow);
        }

        private void YellowOrange_Click(object sender, RoutedEventArgs e)
        {
            ChangeColor(LimitlessLEDBridge.ColorType.Yellow_Orange);
        }
        
        private void BabyBlue_Click(object sender, RoutedEventArgs e)
        {
            ChangeColor(LimitlessLEDBridge.ColorType.Baby_Blue);
        }
        
        private void Mint_Click(object sender, RoutedEventArgs e)
        {
            ChangeColor(LimitlessLEDBridge.ColorType.Mint);
        }

        private void SeafoamGreen_Click(object sender, RoutedEventArgs e)
        {
            ChangeColor(LimitlessLEDBridge.ColorType.Seafoam_Green);
        }

        private void ChangeColor(LimitlessLEDBridge.ColorType color)
        {
            if (PrepareProcessor())
            {
                bridgeProcessor.Command = SPLimitlessLEDBridge.LEDBridgeCommand.SetColor;
                bridgeProcessor.Color = color;
                bridgeProcessor.Run(null, EventArgs.Empty);
            }
        }
    }
}
