using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AbyssConsole
{
    public abstract class AbstractThumbnailUserControl : UserControl
    {
        abstract public void Refresh();
    }
}
