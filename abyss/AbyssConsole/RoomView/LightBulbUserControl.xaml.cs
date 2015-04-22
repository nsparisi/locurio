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
        TestLightBulb light;

        public LightBulbUserControl()
        {
            InitializeComponent();
        }

        public LightBulbUserControl(TestLightBulb light)
            : this()
        {
            this.light = light;
            Refresh();
        }

        public override void Refresh()
        {
            if (this.light != null)
            {
                this.NameText.Content = this.light.Name;
                this.StateText.Content = this.light.State.ToString();
            }
        }

        private void On_Click(object sender, RoutedEventArgs e)
        {
            if(this.light != null)
            {
                this.light.TurnOn();
            }
        }

        private void Off_Click(object sender, RoutedEventArgs e)
        {
            if (this.light != null)
            {
                this.light.TurnOff();
            }
        }
    }
}
