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
        public event AbyssEvent CountdownTicked;

        [AbyssOutput]
        public event AbyssEvent CountdownStarted;
        
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

        public SPScreen()
            : base()
        {
            this.Name = "SPScreen";
            this.Screens = new List<AbyssScreenController>();
        }

        public override void Initialize()
        {
            foreach (AbyssScreenController screen in this.Screens)
            {
                screen.CountdownExpired += this.OnCountDownExpired;
                screen.CountdownStarted += this.OnCountDownStarted;
                screen.CountdownTicked += this.OnCountdownTicked;
            }
        }

        protected override void Process()
        {
            Debug.Log("SPScreen Proc Start [{0}]", Name);

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
            Debug.Log("SPScreen Proc Ended [{0}]", Name);
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

        private void OnCountdownTicked(object sender, EventArgs e)
        {
            if (CountdownTicked != null)
            {
                CountdownTicked(sender, e);
            }
        }
    }
}
