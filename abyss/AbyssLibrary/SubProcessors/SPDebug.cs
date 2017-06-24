using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public class SPDebug : AbstractSubProcessor
    {
        [AbyssParameter]
        public string Message;

        [AbyssInput]
        public void Start(object sender, EventArgs e)
        {
            this.StartProcess();
        }

        protected override void Process()
        {
            Debug.Log(this.Message);
            ProcessEnded();
        }
    }
}
