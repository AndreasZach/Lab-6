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
        static public double DebugSpeed { get; set; }
        public ConcurrentBag<Glass> glassesCarried = new ConcurrentBag<Glass>();

        public void GatherDirtyGlasses(ConcurrentBag<Glass> glassesToGather)
        {
            if(glassesToGather.IsEmpty)
                LogStatus("Waiting for work");
            while (glassesToGather.IsEmpty)
            {
                Thread.Sleep(50);
            }
            LogStatus("Gathering Glasses");
            Thread.Sleep((int)((10000 * DebugSpeed) * simulationSpeed));
            while (!glassesToGather.IsEmpty)
            {
                glassesToGather.TryTake(out Glass toGather);
                glassesCarried.Add(toGather);
            }
        }

        public void CleanAndStoreGlasses(ConcurrentQueue<Glass> glassesOnShelf)
        {
            LogStatus("Washing and storing dishes");
            Thread.Sleep((int)((15000 * DebugSpeed) * simulationSpeed));
            while (!glassesCarried.IsEmpty)
            {
                glassesCarried.TryTake(out Glass toStore);
                glassesOnShelf.Enqueue(toStore);
                UIUpdater.UpdateGlassesLabel(glassesOnShelf.Count());
            }
        }

        public override void LogStatus(string newStatus)
        {
            UIUpdater.LogWaiterAction(newStatus);
        }
    }
}
