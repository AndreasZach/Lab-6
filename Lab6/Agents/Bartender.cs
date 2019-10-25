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
                    Thread.Sleep(50);
                }
                glassesInShelf.TryDequeue(out carriedGlass);
            }
        }

        public void FetchGlass()
        {
            SendStatusToLog("Fetching a glass");
            carriedGlass.IsAvailable = false;
            Thread.Sleep(3000);
        }

        public void ServeBeer()
        {
            SendStatusToLog($"Pouring a beer for {currentPatron.GetName()}");
            Thread.Sleep(3000);
            currentPatron.SetBeer(carriedGlass);
            carriedGlass = null;
            currentPatron = null;
        }
    }
}
