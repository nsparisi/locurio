using Abyss;
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
    /// Interaction logic for RoomView.xaml
    /// </summary>
    public partial class RoomSubPage : UserControl
    {
        Dictionary<IPhysicalObject, AbstractPhysicalObjectUserControl> uiMappings;

        public RoomSubPage()
        {
            uiMappings = new Dictionary<IPhysicalObject, AbstractPhysicalObjectUserControl>();
            InitializeComponent();
        }

        public void Refresh()
        {
            foreach (var userControl in uiMappings.Values)
            {
                userControl.Refresh();
            }
        }

        public void ClearScreen()
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                this.WrapPanel.Children.Clear();
            });
        }

        public void AddPhysicalObject(IPhysicalObject physicalObject)
        {
            if (uiMappings.ContainsKey(physicalObject))
            {
                return;
            }

            AbstractPhysicalObjectUserControl uiItem = null;

            if (physicalObject is LimitlessLEDBridge)
            {
                uiItem = new LightBulbUserControl((LimitlessLEDBridge)physicalObject);
            }
            else if (physicalObject is XBeeEndpoint)
            {
                uiItem = new XBeeUserControl((XBeeEndpoint)physicalObject);
            }
            else if (physicalObject is VLCServerControl)
            {
                uiItem = new SoundUserControl((VLCServerControl)physicalObject);
            }
            else if (physicalObject is TextingController)
            {
                // ignore
            }
            else if (physicalObject is AbyssScreenController)
            {
                // ignore
            }
            else if (physicalObject is AbyssGameController)
            {
                // ignore   
            }
            else
            {
                uiItem = new PhysicalObjectUserControl(physicalObject);
            }

            if (uiItem != null)
            {
                this.WrapPanel.Children.Add(uiItem);
                uiMappings.Add(physicalObject, uiItem);
            }
        }
    }
}
