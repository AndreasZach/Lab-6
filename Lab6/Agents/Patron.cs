using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    public class Patron : Agent
    {
        private UIUpdater uiUpdater;
        static public int DebugTimeToStay { get; set; }
        private int patronID;
        private string name;
        private Glass carriedBeer;
        private Chair chairUsed;
        private int minInterval = 10;
        private int maxInterval = 21;

        public Patron(string name, int ID, UIUpdater uiUpdater)
        {
            this.name = name;
            this.uiUpdater = uiUpdater;
            patronID = ID;
            LogStatus($"{name} enters the pub");
        }

        public void GoToBar(ConcurrentQueue<Patron> queueToBar)
        {
            Thread.Sleep((int)((1 * DebugTimeToStay) * SimulationSpeed));
            queueToBar.Enqueue(this);
            while(!HasBeer())
            {
                if (EndWork)
                    return;
                Thread.Sleep(50);
            }
        }

        public void FindChair(ConcurrentQueue<Patron> queueToChair, ConcurrentBag<Chair> availableChairs)
        {
            if (EndWork)
                return;
            LogStatus($"{name} looks for an available chair"); 
            Thread.Sleep((int)((4 * DebugTimeToStay) * SimulationSpeed));
            queueToChair.Enqueue(this);
            while(availableChairs == null)
            {
                Thread.Sleep(50);
            }
            availableChairs.TryTake(out chairUsed);
            uiUpdater.UpdateChairLabel(availableChairs.Count());
            _ = queueToChair.TryDequeue(out _);
        }

        public void DrinkBeer()
        {
            if (EndWork)
                return;
            LogStatus($"{name} sits down and drinks their beer");
            Thread.Sleep((int)((RandomIntGenerator.GetRandomInt(minInterval, maxInterval) * DebugTimeToStay) * SimulationSpeed));
        }

        public void LeaveBar(ConcurrentDictionary<int, Patron> allPatrons, ConcurrentBag<Chair> availableChairs, ConcurrentBag<Glass> glassesOnTables)
        {
            if (chairUsed != null)
                availableChairs.Add(chairUsed);
            uiUpdater.UpdateChairLabel(availableChairs.Count());
            chairUsed = null;
            if (carriedBeer != null)
                glassesOnTables.Add(carriedBeer);
            carriedBeer = null;
            allPatrons.TryRemove(patronID, out _);
            LogStatus($"{name} leaves the pub");
            uiUpdater.UpdatePatronLabel(allPatrons.Count());
        }

        public string GetName()
        {
            return name;
        }

        public bool HasBeer()
        {
            return carriedBeer != null;
        }

        public void SetBeer(Glass beer)
        {
            carriedBeer = beer;
        }

        public override void LogStatus(string newStatus)
        {
            uiUpdater.LogPatronAction(newStatus);
        }
    }
}
