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
        static public double SimulationSpeed {get; set;}
        public bool PubClosing { get; set; } = false;
        public bool LeftPub { get; protected set; } = false;
        public abstract void LogStatus(string status);
        protected void ActionDelay(double secondsDelay)
        {
            double ActionTimeDelay = Time.countDown - secondsDelay;
            while (Time.countDown > ActionTimeDelay)
            {
                Thread.Sleep(100);
            }
        }
    }
}
