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

        [AbyssInput]
        public void ClearHistory(object sender, EventArgs e)
        {
            clearHistory = true;
            StartProcess();
        }

        private bool clearHistory;

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
            string debugText = string.IsNullOrEmpty(TextMessage) ? string.Empty : TextMessage;
            Debug.Log("SPTextingController Start [{0}] [text: {1}...]", Name, debugText.TruncateLongString(30));

            foreach (TextingController controller in TextingControllers)
            {
                if (controller == null)
                {
                    continue;
                }

                if (clearHistory)
                {
                    controller.ClearHistory();
                }
                else
                {
                    controller.SendTextMessage(TextMessage);
                }
            }

            ProcessEnded();
        }

        protected override void ProcessEnded()
        {
            Debug.Log("SPTextingController Ended [{0}]", Name);
            clearHistory = false;
        }
    }
}
