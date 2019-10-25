using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    abstract class Agent
    {
        // WorkPaused property, so that we can pause all Agent threads, as well as resume them.
        public virtual bool PauseWork { get; set; } = false;

        public virtual void SendStatusToLog(string toLog)
        {

        }

    }
}
