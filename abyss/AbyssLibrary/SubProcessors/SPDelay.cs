using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class SPDelay : AbstractSubProcessor
    {
        [AbyssParameter]
        public int DurationMs;

        [AbyssInput]
        public void Start()
        {
            Thread.Sleep(DurationMs);

            if (Finished != null)
            {
                Finished();
            }
        }
        
        [AbyssOutput]
        public event OutputEvent Finished;
    }
}
