using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    public class Bartender : Agent
    {
        private Glass carriedGlass = null;
        private Patron currentPatron = null;

        public void Work(ConcurrentQueue<Glass> glassesInShelf, ConcurrentQueue<Patron> queueToBar)
        {
            if (queueToBar.IsEmpty && EndWork)
                return;
            if (queueToBar.IsEmpty)
            {
                LogStatus("Waiting for patron");
                while (queueToBar.IsEmpty && !EndWork)
                {
                    Thread.Sleep(50);
                }
            }
            queueToBar.TryDequeue(out currentPatron);
            if (glassesInShelf.IsEmpty)
            {
                LogStatus("Waiting for a clean glass");
                while (glassesInShelf.IsEmpty)
                {
                    Thread.Sleep(50);
                }
            }
            if (carriedGlass == null)
                glassesInShelf.TryDequeue(out carriedGlass);
            UIUpdater.UpdateGlassesLabel(glassesInShelf.Count());
        }

        public void FetchGlass()
        {
            if (EndWork)
                return;
            LogStatus("Fetching a glass");
            Thread.Sleep((int)(3000 * simulationSpeed));
        }

        public void ServeBeer()
        {
            if (EndWork)
                return;
            if (currentPatron == null)
                return;
            LogStatus($"Pouring a beer for {currentPatron.GetName()}");
            Thread.Sleep((int)(3000 * simulationSpeed));
            currentPatron.SetBeer(carriedGlass);
            carriedGlass = null;
            currentPatron = null;
        }

        public override void LogStatus(string newStatus)
        {
            UIUpdater.LogBartenderAction(newStatus);
        }

        public void CleanUp(ConcurrentQueue<Glass> glassesInShelf)
        {
            if (carriedGlass != null)
                glassesInShelf.Enqueue(carriedGlass);
            UIUpdater.UpdateGlassesLabel(glassesInShelf.Count());
        }
    }
}
