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
        public string Name { get; set; }
        public Glass CarriedBeer { get; set; }
        public Chair ChairUsed { get; set; }

        public Patron()
        {

        }
        public Patron(string name)
        {
            Thread.Sleep(1000);
            Name = name;
        }

        public void FindChair(ConcurrentBag<Chair> foundChair, Glass recievedBeer)
        {
            CarriedBeer = recievedBeer;
            Chair tempChair;
            while(foundChair == null)
            {
                Thread.Sleep(50);
            }
            foundChair.TryTake(out tempChair);
            ChairUsed = tempChair;
            DrinkBeer();
        }

        public void DrinkBeer()
        {
            Thread.Sleep(); //Random time between 10-20
        }
        //Return both chair and glass via property and Pub class
    }
}
