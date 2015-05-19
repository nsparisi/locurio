﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class SPTextingController : AbstractSubProcessor
    {
        [AbyssParameter]
        public string TextMessage { get; set; }

        [AbyssParameter]
        public List<TextingController> TextingControllers
        {
            get;
            set;
        }

        [AbyssInput]
        public void SendTextMessage(object sender, EventArgs e)
        {
            StartProcess();
        }

        public SPTextingController()
            : base()
        {
            this.Name = "SPTextingController";
            this.TextingControllers = new List<TextingController>();
        }

        public override void Initialize()
        {
            // nothing to initialize
        }

        protected override void Process()
        {
            Debug.Log("SPTextingController Proc Start");

            foreach (TextingController controller in TextingControllers)
            {
                if (controller == null)
                {
                    continue;
                }

                controller.SendTextMessage(TextMessage);
            }

            ProcessEnded();
        }

        protected override void ProcessEnded()
        {
            Debug.Log("SPTextingController Proc Ended");
        }
    }
}
