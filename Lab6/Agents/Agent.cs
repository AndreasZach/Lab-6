using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    public abstract class Agent
    {
        // WorkPaused property, so that we can pause all Agent threads, as well as resume them.
        static public double simulationSpeed {get; set;}
        public bool PubClosing { get; set; } = false;
        public bool LeftPub { get; internal set; } = false;
        public virtual bool PauseWork { get; set; } = false;
        public abstract void LogStatus(string status);
    }
}
