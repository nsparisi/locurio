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
    public partial class PhysicalObjectUserControl : AbstractPhysicalObjectUserControl
    {
        private IPhysicalObject physicalObject;

        public PhysicalObjectUserControl()
        {
            InitializeComponent();
        }

        public PhysicalObjectUserControl(IPhysicalObject physicalObject)
            : this()
        {
            this.physicalObject = physicalObject;
            Refresh();
        }

        public override void Refresh()
        {
            if(this.physicalObject != null)
            {
                this.NameText.Content = this.physicalObject.Name;
            }
        }
    }
}
