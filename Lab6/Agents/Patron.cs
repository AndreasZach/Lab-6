using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    class Patron : Agent
    {
        public int DebugTimeToStay { get; set; }
        private int patronID;
        private string name;
        private Glass carriedBeer;
        private Chair chairUsed;
        public int minInterval { get; set; }
        public int maxInterval { get; set; }

        public Patron(string name, int ID)
        {
            this.name = name;
            patronID = ID;
            SendStatusToLog($"{name} enters the pub");
        }

        public void GoToBar(ConcurrentQueue<Patron> queueToBar)
        {
            Thread.Sleep(1 * DebugTimeToStay);
            queueToBar.Enqueue(this);
            while (!HasBeer())
            {
                HasBeer();
            }
            _ = queueToBar.TryDequeue(out _);
        }

        public void FindChair(ConcurrentQueue<Patron> queueToChair, ConcurrentBag<Chair> availableChairs)
        {
            SendStatusToLog($"{name} looks for an available chair");
            Thread.Sleep(4 * DebugTimeToStay);
            queueToChair.Enqueue(this);
            while(availableChairs == null)
            {
                Thread.Sleep(50);
            }
            availableChairs.TryTake(out chairUsed);
            chairUsed.IsAvailable = false; //Keep?
            _ = queueToChair.TryDequeue(out _);
        }

        public void DrinkBeer()
        {
            Thread.Sleep((RandomIntGenerator.GetRandomInt(minInterval, maxInterval)) * DebugTimeToStay);
            carriedBeer.IsDirty = true;
        }

        public void LeaveBar(ConcurrentDictionary<int, Patron> allPatrons, ConcurrentBag<Chair> availableChairs, ConcurrentBag<Glass> glassesOnTables)
        {
            carriedBeer.IsAvailable = true;
            chairUsed.IsAvailable = true;
            availableChairs.Add(chairUsed);
            chairUsed = null;
            glassesOnTables.Add(carriedBeer);
            carriedBeer = null;
            allPatrons.TryRemove(patronID, out _);
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

    }
}
