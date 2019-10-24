using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    class Bartender : Agent
    {
        private Glass carriedGlass = null;
        private Patron currentPatron = null;

        public void Work(ConcurrentQueue<Glass> glassesInShelf, ConcurrentQueue<Patron> queueToBar)
        {
            if (queueToBar.IsEmpty)
            {
                SendStatusToLog("Waiting for patron");
                while (queueToBar.IsEmpty)
                {
                    queueToBar.TryPeek(out currentPatron);
                }
            }
            if (glassesInShelf.IsEmpty)
            {
                SendStatusToLog("Waiting for a clean glass");
                while (glassesInShelf.IsEmpty)
                {
                    glassesInShelf.TryDequeue(out carriedGlass);
                }
            }
            FetchGlass();
            ServeBeer();
        }

        public void FetchGlass()
        {
            carriedGlass.IsAvailable = false;
            Thread.Sleep(3000); //Call method for correct time.
        }

        public void ServeBeer()
        {
            Thread.Sleep(3000); //Call method for correct time.
            currentPatron.CarriedBeer = carriedGlass;
            carriedGlass = null;
            currentPatron = null;
        }

        public override void SendStatusToLog(string toLog)
        {
            //Method(toLog)
        }
    }
}
