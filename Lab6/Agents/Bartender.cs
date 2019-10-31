using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace Lab6
{
    public class Bartender : Agent
    {
        private Glass carriedGlass = null;
        private Patron currentPatron = null;
        private enum State { AwaitingPatron, AwaitingGlass, PouringBeer, FetchingGlass, LeavingWork };
        private State currentState = default;
        ConcurrentQueue<Glass> glassesInShelf;
        ConcurrentQueue<Patron> queueToBar;
        ConcurrentDictionary<int, Patron> allPatrons;

        public Bartender(UIUpdater uiUpdater,
            ConcurrentQueue<Glass> glassesInShelf,
            ConcurrentQueue<Patron> queueToBar,
            ConcurrentDictionary<int, Patron> allPatrons) 
            : base(uiUpdater)
        {
            this.glassesInShelf = glassesInShelf;
            this.queueToBar = queueToBar;
            this.allPatrons = allPatrons;
        }

        public void Work()
        {
            while (!LeftPub)
            {
                SetState();
                switch (currentState)
                {
                    case State.AwaitingPatron:
                        WaitForPatron();
                        break;
                    case State.AwaitingGlass:
                        WaitForGlass();
                        break;
                    case State.FetchingGlass:
                        FetchGlass();
                        break;
                    case State.PouringBeer:
                        PourBeer();
                        carriedGlass = null;
                        currentPatron = null;
                        break;
                    case State.LeavingWork:
                        CleanBarAndLeaveWork();
                        break;
                }
            }
        }

        private void WaitForPatron()
        {
            LogStatus("Waiting for Patron", this);
            while (queueToBar.IsEmpty)
            {
                Thread.Sleep(50);
            }
        }

        private void WaitForGlass()
        {
            LogStatus("Waiting for a clean glass", this);
            while (glassesInShelf.IsEmpty)
            {
                Thread.Sleep(50);
            }
        }

        private void FetchGlass()
        {
            LogStatus("Fetching a glass", this);

            ActionDelay(3);
            while (carriedGlass == null)
            {
                glassesInShelf.TryDequeue(out carriedGlass);
            }
            uiUpdater.UpdateGlassesLabel(glassesInShelf.Count());
        }

        private void PourBeer()
        {
            while (currentPatron == null)
            {
                queueToBar.TryDequeue(out currentPatron);
            }
            LogStatus($"Pouring a beer for {currentPatron.GetName()}", this);
            ActionDelay(3);
            carriedGlass.ContainsBeer = true;
            currentPatron.SetBeer(carriedGlass);
        }

        private void CleanBarAndLeaveWork()
        {
            LogStatus("Bartender tidys up the bar", this);
            while (!allPatrons.IsEmpty)
            {
                Thread.Sleep(50);
            }
            LogStatus("Bartender leaves the pub", this);
            LeftPub = true;
        }

        private void SetState()
        {
            if (queueToBar.IsEmpty && PubClosing)
            {
                currentState = State.LeavingWork;
            }
            if (currentState == State.FetchingGlass && carriedGlass != null)
            {
                currentState = State.PouringBeer;
                return;
            }
            if (!queueToBar.IsEmpty && !glassesInShelf.IsEmpty)
            {
                currentState = State.FetchingGlass;
                return;
            }
            if (queueToBar.IsEmpty && !PubClosing)
            {
                currentState = State.AwaitingPatron;
                return;
            }
            if (glassesInShelf.IsEmpty && !queueToBar.IsEmpty)
            {
                currentState = State.AwaitingGlass;
                return;
            }
        }
    }
}
