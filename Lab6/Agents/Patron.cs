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

        public Patron(string name, ConcurrentQueue<Patron> queueToBar, ConcurrentBag<Chair> availableChairs)
        {
            this.name = name;
            GoToBar(queueToBar, availableChairs);
        }

        public void GoToBar(ConcurrentQueue<Patron> queueToBar, ConcurrentBag<Chair> availableChairs)
        {
            Thread.Sleep(1000);
            queueToBar.Enqueue(this);
            while (!HasBeer())
            {
                HasBeer();
            }
            FindChair(availableChairs);
        }

        public void FindChair(ConcurrentBag<Chair> foundChair)
        {
            Chair tempChair;
            while(foundChair == null)
            {
                Thread.Sleep(50);
            }
            foundChair.TryTake(out tempChair);
            chairUsed = tempChair;
            DrinkBeer();
        }

        public void DrinkBeer()
        {
            Thread.Sleep((RandomIntGenerator.GetRandomInt(minInterval, maxInterval)) * 1000);
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
