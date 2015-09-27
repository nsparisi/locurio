using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class SPTimerController : AbstractSubProcessor
    {
        [AbyssParameter]
        public long TimeInMilliseconds { get; set; }

        [AbyssParameter]
        public List<TimerController> TimerControllers
        {
            get;
            set;
        }

        [AbyssInput]
        public void StartTimer(object sender, EventArgs e)
        {
            settingType = TimerType.Start;
            StartProcess();
        }

        [AbyssInput]
        public void SuspendTimer(object sender, EventArgs e)
        {
            settingType = TimerType.Suspend;
            StartProcess();
        }

        [AbyssInput]
        public void ResetTimer(object sender, EventArgs e)
        {
            settingType = TimerType.Reset;
            StartProcess();
        }

        [AbyssInput]
        public void SetTime(object sender, EventArgs e)
        {
            settingType = TimerType.SetTime;
            StartProcess();
        }

        private enum TimerType { Start, Suspend, Reset, SetTime }
        private TimerType settingType;

        public SPTimerController()
            : base()
        {
            this.Name = "SPTimerController";
            this.TimerControllers = new List<TimerController>();
        }

        public override void Initialize()
        {
            // nothing to initialize
        }

        protected override void Process()
        {
            Debug.Log("SPTimerController Start [{0}] [type: {1}] [time: {2}]", Name, settingType, TimeInMilliseconds);
            TimerType localType = settingType;

            foreach (TimerController controller in TimerControllers)
            {
                if (controller == null)
                {
                    continue;
                }

                if (localType == TimerType.Start)
                {
                    controller.Start();
                }
                else if (localType == TimerType.Suspend)
                {
                    controller.Suspend();
                }
                else if (localType == TimerType.Reset)
                {
                    controller.Reset();
                }
                else if (localType == TimerType.SetTime)
                {
                    controller.SetTime(TimeInMilliseconds);
                }
            }

            ProcessEnded();
        }

        protected override void ProcessEnded()
        {
            Debug.Log("SPTimerController Ended [{0}]", Name);
        }
    }
}
