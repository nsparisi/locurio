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
        public void Start(object sender, EventArgs e)
        {
            this.StartProcess();
        }

        protected override void Process()
        {
            Debug.Log("SPDelay Proc Start");
            Thread.Sleep(DurationMs);

            ProcessEnded();
        }

        protected override void ProcessEnded()
        {
            Debug.Log("SPDelay Proc Ended");
            if (Finished != null)
            {
                Finished(this, EventArgs.Empty);
            }
        }
        
        [AbyssOutput]
        public event AbyssEvent Finished;
    }
}
