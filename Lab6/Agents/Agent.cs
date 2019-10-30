using System.Threading;

namespace Lab6
{
    public abstract class Agent
    {
        static public double SimulationSpeed {get; set;}
        public bool PubClosing { get; set; } = false;
        public bool LeftPub { get; protected set; } = false;
        public abstract void LogStatus(string status);
        protected void ActionDelay(double secondsDelay, Bouncer bouncer = null)
        {
            double ActionTimeDelay = Time.countdown - secondsDelay;
            while (Time.countdown > ActionTimeDelay)
            {
                if (bouncer != null && PubClosing)
                    return;
                Thread.Sleep(100);
            }
        }
    }
}
