using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class SPSoundControl : AbstractSubProcessor
    {
        [AbyssParameter]
        public string SongFileName { get; set; }

        [AbyssParameter]
        public float Volume { get; set; }

        [AbyssParameter]
        public List<VLCServerControl> VLCControllers
        {
            get;
            set;
        }

        private bool calledPlay;
        private bool calledPause;
        private bool calledSetVolume;
        private bool calledStop;

        [AbyssInput]
        public void Play(object sender, EventArgs e)
        {
            calledPlay = true;
            StartProcess();
        }

        [AbyssInput]
        public void Pause(object sender, EventArgs e)
        {
            calledPause = true;
            StartProcess();
        }

        [AbyssInput]
        public void SetVolume(object sender, EventArgs e)
        {
            calledSetVolume = true;
            StartProcess();
        }

        [AbyssInput]
        public void Stop(object sender, EventArgs e)
        {
            calledStop = true;
            StartProcess();
        }

        public SPSoundControl()
            : base()
        {
            this.Name = "SPSoundControl";
            this.VLCControllers = new List<VLCServerControl>();
        }

        public override void Initialize()
        {
            // nothing to initialize
        }

        protected override void Process()
        {
            Debug.Log("SPSoundControl Proc Start");

            foreach (VLCServerControl controller in VLCControllers)
            {
                if (controller == null)
                {
                    continue;
                }

                if (calledPlay)
                {
                    controller.Play(SongFileName);
                }
                else if(calledPause)
                {
                    controller.Pause();
                }
                else if (calledSetVolume)
                {
                    controller.SetVolume(Volume);
                }
                else if (calledStop)
                {
                    controller.Stop();
                }
            }

            ProcessEnded();
        }

        protected override void ProcessEnded()
        {
            Debug.Log("SPSoundControl Proc Ended");
            ResetFlags();
        }

        private void ResetFlags()
        {
            calledPlay = false;
            calledPause = false;
            calledSetVolume = false;
            calledStop = false;
        }
    }
}
