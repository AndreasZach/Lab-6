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
        static public double SimulationSpeed {get; set;}
        static public bool EndWork { get; set; } = false;
        public virtual bool PauseWork { get; set; } = false;
        public abstract void LogStatus(string status);
    }
}
