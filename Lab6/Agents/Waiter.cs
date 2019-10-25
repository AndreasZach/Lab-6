using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    class Waiter : Agent
    {
        public double DebugSpeed { get; set; } = 1;
        ConcurrentBag<Glass> glassesCarried = new ConcurrentBag<Glass>();

        public void GatherDirtyGlasses(ConcurrentBag<Glass> glassesToGather)
        {
            if(glassesToGather.IsEmpty)
                SendStatusToLog("Waiting for work");
            while (glassesToGather.IsEmpty)
            {
                Thread.Sleep(50);
            }
            SendStatusToLog("Gathering Glasses");
            Thread.Sleep((int)(10000 * DebugSpeed));
            while (!glassesToGather.IsEmpty)
            {
                glassesToGather.TryTake(out Glass toGather);
                glassesCarried.Add(toGather);
            }
        }

        public void CleanAndStoreGlasses(ConcurrentQueue<Glass> glassesOnShelf)
        {
            SendStatusToLog("Washing and storing dishes");
            Thread.Sleep((int)(15000 * DebugSpeed));
            while (!glassesCarried.IsEmpty)
            {
                glassesCarried.TryTake(out Glass toStore);
                toStore.IsDirty = false;
                toStore.IsAvailable = true;
                glassesOnShelf.Enqueue(toStore);
            }
        }
    }
}
