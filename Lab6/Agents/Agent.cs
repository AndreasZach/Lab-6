using System.Threading;

namespace Lab6
{
    public abstract class Agent
    {
        protected UIUpdater uiUpdater;

        protected Agent(UIUpdater uiUpdater)
        {
            this.uiUpdater = uiUpdater;
        }

        static public double SimulationSpeed {get; set;}
        public bool PubClosing { get; set; } = false;
        public bool LeftPub { get; protected set; } = false;

        protected void LogStatus(string newStatus, Agent agent)
        {
            if (agent is Bartender)
            {
                uiUpdater.LogBartenderAction(newStatus);
                return;
            }
            if (agent is Bouncer || agent is Patron)
            {
                uiUpdater.LogPatronAction(newStatus);
                return;
            }
            if (agent is Waiter)
            {
                uiUpdater.LogWaiterAction(newStatus);
                return;
            }
        }

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
