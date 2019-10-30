using System.Threading;

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
            double ActionTimeDelay = Time.countdown - secondsDelay;
            while (Time.countdown > ActionTimeDelay)
            {
                Thread.Sleep(100);
            }
        }
    }
}
