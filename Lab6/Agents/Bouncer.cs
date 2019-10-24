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
        //Add random name generator method to fill the List. Need to make it many names, so as to never let it get empty.
        public ConcurrentBag<string> patronNames = new ConcurrentBag<string>();
        public int minInterval { get; set; }
        public int maxInterval { get; set; }

        //TODO: Make it so that thread continues after returning a Patron

        public void BouncerStart(ConcurrentQueue<Patron> queueToBar)
        {
            while (LeftPub == false)
            {
                Thread.Sleep((RandomIntGenerator.GetRandomInt(minInterval, maxInterval)) * 1000);
                string name;
                patronNames.TryTake(out name);
                if (name != null)
                    queueToBar.Enqueue(new Patron(name));
            }
        }
        public void AllowPatronEntry(ConcurrentQueue<Patron> queueToBar)
        {
            while (LeftPub != true)
            {
                string name;
                patronNames.TryTake(out name);
                if (name != null)
                    queueToBar.Enqueue(new Patron(name));
                Thread.Sleep((RandomIntGenerator.GetRandomInt(minInterval,maxInterval)) * 1000);
            }
        }
    }
}
