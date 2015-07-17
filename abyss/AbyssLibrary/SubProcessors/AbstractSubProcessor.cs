using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public abstract class AbstractSubProcessor : ISubProcessor
    {
        public Guid ID { get; private set; }
        public string Name { get; set; }

        protected AbstractSubProcessor()
        {
            this.Name = "Default";
            this.ID = Guid.NewGuid();
        }

        public virtual void Initialize()
        {

        }

        protected virtual void StartProcess()
        {
            Thread thread = new Thread(Process);
            thread.IsBackground = true;
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
