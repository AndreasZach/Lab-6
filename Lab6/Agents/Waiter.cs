using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    public class Waiter : Agent
    {
        private UIUpdater uiUpdater;
        static public double DebugSpeed { get; set; }
        ConcurrentBag<Glass> glassesCarried = new ConcurrentBag<Glass>();

        public Waiter(UIUpdater uiUpdater)
        {
            this.uiUpdater = uiUpdater;
        }

        public void GatherDirtyGlasses(ConcurrentBag<Glass> glassesToGather)
        {
            if(glassesToGather.IsEmpty)
                LogStatus("Waiting for work");
            while (glassesToGather.IsEmpty)
            {
                Thread.Sleep(50);
            }
            LogStatus("Gathering Glasses");
            Thread.Sleep((int)((10000 * DebugSpeed) * SimulationSpeed));
            while (!glassesToGather.IsEmpty)
            {
                glassesToGather.TryTake(out Glass toGather);
                glassesCarried.Add(toGather);
            }
        }

        public void CleanAndStoreGlasses(ConcurrentQueue<Glass> glassesOnShelf)
        {
            LogStatus("Washing and storing dishes");
            Thread.Sleep((int)((15000 * DebugSpeed) * SimulationSpeed));
            while (!glassesCarried.IsEmpty)
            {
                glassesCarried.TryTake(out Glass toStore);
                glassesOnShelf.Enqueue(toStore);
                uiUpdater.UpdateGlassesLabel(glassesOnShelf.Count());
            }
        }

        public override void LogStatus(string newStatus)
        {
            uiUpdater.LogWaiterAction(newStatus);
        }
    }
}
