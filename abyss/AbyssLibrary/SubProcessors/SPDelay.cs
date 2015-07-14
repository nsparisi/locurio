﻿using System;
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

        [AbyssOutput]
        public event AbyssEvent Finished;

        protected override void Process()
        {
            Debug.Log("SPDelay Proc Start [{0}]", Name);
            Thread.Sleep(DurationMs);

            ProcessEnded();
        }

        protected override void ProcessEnded()
        {
            Debug.Log("SPDelay Proc Ended [{0}]", Name);
            if (Finished != null)
            {
                Finished(this, EventArgs.Empty);
            }
        }
    }
}
