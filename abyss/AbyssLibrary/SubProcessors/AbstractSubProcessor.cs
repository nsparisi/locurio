using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public abstract class AbstractSubProcessor
    {
        public string Name { get; set; }

        void AbstractSubProcess()
        {
            this.Name = "Default";
        }

        public virtual void Initialize()
        {

        }

        protected virtual void StartProcess()
        {
            Thread thread = new Thread(Process);
            thread.Start();
        }

        protected virtual void Process()
        {
            ProcessEnded();
        }

        protected virtual void ProcessEnded()
        {
        }
    }
}
