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
        public bool IsDisabled { get; private set; }

        private List<Thread> threads;
        private object lockObj = new object();

        protected AbstractSubProcessor()
        {
            this.Name = "Default";
            this.ID = Guid.NewGuid();
            this.threads = new List<Thread>();
        }

        public virtual void Initialize()
        {

        }

        [AbyssInput]
        public void Disable(object sender, EventArgs e)
        {
            IsDisabled = true;
            KillThreads();
        }

        [AbyssInput]
        public void Enable(object sender, EventArgs e)
        {
            IsDisabled = false;
        }

        protected virtual void StartProcess()
        {
            if (IsDisabled)
            {
                return;
            }

            CleanThreads();

            Thread thread = new Thread(Process);
            thread.IsBackground = true;
            thread.Start();

            lock (lockObj)
            {
                threads.Add(thread);
            }
        }

        protected virtual void Process()
        {
            ProcessEnded();
        }

        protected virtual void ProcessEnded()
        {
        }

        private void KillThreads()
        {
            lock (lockObj)
            {
                foreach (Thread thread in threads)
                {
                    thread.Abort();
                }

                this.threads.Clear();
            }
        }

        private void CleanThreads()
        {
            lock (lockObj)
            {
                for (int i = threads.Count -1; i >= 0; i-- )
                {
                    Thread thread = threads[i];
                    if (thread.ThreadState == ThreadState.Aborted ||
                        thread.ThreadState == ThreadState.Stopped)
                    {
                        threads.RemoveAt(i);
                    }
                }
            }
        }
    }
}
