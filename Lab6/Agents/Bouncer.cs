using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    class Bouncer : Agent
    {

        public ConcurrentBag<string> patronNames = new ConcurrentBag<string>();
        public int minInterval { get; set; }
        public int maxInterval { get; set; }

        //TODO: Make it so that thread continues after returning a Patron
        public void AllowPatronEntry(ConcurrentBag<Patron> allPatrons, Action<Patron> createPatronTask)
        {
            while (LeftPub != true)
            {
                Thread.Sleep((RandomIntGenerator.GetRandomInt(minInterval, maxInterval)) * 1000);
                string name;
                patronNames.TryTake(out name);
                if (name != null)
                {
                    Patron tempPatron = new Patron(name);
                    createPatronTask(tempPatron);
                    allPatrons.Add(tempPatron);
                }
            }
        }
    }
}
