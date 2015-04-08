using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class SPScreen : AbstractSubProcessor
    {
        bool startCountdown = false;

        [AbyssParameter]
        public List<AbyssScreenController> Screens
        {
            get;
            set;
        }

        [AbyssOutput]
        public event AbyssEvent CountdownExpired;

        [AbyssOutput]
        public event AbyssEvent CountdownStopped;

        [AbyssOutput]
        public event AbyssEvent CountdownStarted;

        public SPScreen()
            : base()
        {
            this.Name = "SPScreen";
        }

        [AbyssInput]
        public void Start(object sender, EventArgs e)
        {
            startCountdown = true;
            StartProcess();
        }

        [AbyssInput]
        public void Stop(object sender, EventArgs e)
        {
            startCountdown = false;
            StartProcess();
        }

        protected override void Process()
        {
            Debug.Log("SPScreen Proc Start");

            if (startCountdown)
            {
                foreach (AbyssScreenController screen in this.Screens)
                {
                    screen.Start();
                }
            }
            else
            {
                foreach (AbyssScreenController screen in this.Screens)
                {
                    screen.Stop();
                }
            }

            ProcessEnded();
        }

        protected override void ProcessEnded()
        {
            Debug.Log("SPScreen Proc Ended");
        }

        private void OnCountDownExpired(object sender, EventArgs e)
        {
            if (CountdownExpired != null)
            {
                CountdownExpired(sender, e);
            }
        }

        private void OnCountDownStarted(object sender, EventArgs e)
        {
            if (CountdownStarted != null)
            {
                CountdownStarted(sender, e);
            }
        }

        private void OnCountDownStopped(object sender, EventArgs e)
        {
            if (CountdownStopped != null)
            {
                CountdownStopped(sender, e);
            }
        }

        public override void Initialize()
        {
            foreach (AbyssScreenController screen in this.Screens)
            {
                screen.CountdownExpired += this.OnCountDownExpired;
                screen.CountdownStarted += this.OnCountDownStarted;
                screen.CountdownStopped += this.OnCountDownStopped;
            }
        }
    }
}
