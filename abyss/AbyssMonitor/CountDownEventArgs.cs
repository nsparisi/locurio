using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssScreen
{
    public class CountDownEventArgs : EventArgs
    {
        public long TimeMs { get; private set; }

        public CountDownEventArgs(long timeMs)
        {
            this.TimeMs = timeMs;
        }
    }
}
