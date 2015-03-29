using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public abstract class AbstractSubProcessor
    {
        public delegate void OutputEvent();

        public string Name { get; set; }

        void AbstractSubProcess()
        {
            this.Name = "Default";
        }
    }
}
