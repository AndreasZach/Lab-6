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
        private string name;
        private Glass carriedBeer;
        private Chair chairUsed;
        public int minInterval { get; set; }
        public int maxInterval { get; set; }

        public Patron(string name)
        {
            this.name = name;
            
        }

        public void GoToBar(ConcurrentQueue<Patron> queueToBar)
        {
            Thread.Sleep(1000);
            queueToBar.Enqueue(this);
            while (!HasBeer())
            {
                HasBeer();
            }
        }

        public void FindChair(ConcurrentQueue<Patron> queueToBar,ConcurrentQueue<Patron> queueToChair, ConcurrentBag<Chair> availableChairs)
        {
            _ = queueToBar.TryDequeue(out _);
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
            Thread.Sleep((RandomIntGenerator.GetRandomInt(minInterval, maxInterval)) * 1000);
            carriedBeer.IsDirty = true;

        }

        public void LeaveBar(ConcurrentBag<Patron> allPatrons, ConcurrentBag<Chair> availableChairs, ConcurrentBag<Glass> glassesOnTables)
        {
            carriedBeer.IsAvailable = true;
            chairUsed.IsAvailable = true;
            availableChairs.Add(chairUsed);
            chairUsed = null;
            glassesOnTables.Add(carriedBeer);
            carriedBeer = null;
            concurre
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
