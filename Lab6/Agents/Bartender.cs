using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace Lab6
{
    public class Bartender : Agent
    {
        private UIUpdater uiUpdater;
        private Glass carriedGlass = null;
        private Patron currentPatron = null;
        enum State { AwaitingPatron, AwaitingGlass, PouringBeer, FetchingGlass, LeavingWork };
        State currentState = default;

        public Bartender(UIUpdater uiUpdater)
        {
            this.uiUpdater = uiUpdater;
        }

        public void Work(ConcurrentQueue<Glass> glassesInShelf, ConcurrentQueue<Patron> queueToBar, ConcurrentDictionary<int, Patron> allPatrons)
        {
            while (!LeftPub)
            {
                SetState(glassesInShelf, queueToBar);
                switch (currentState)
                {
                    case State.AwaitingPatron:
                        WaitForPatron(queueToBar);
                        break;
                    case State.AwaitingGlass:
                        WaitForGlass(glassesInShelf);
                        break;
                    case State.FetchingGlass:
                        FetchGlass(glassesInShelf);
                        break;
                    case State.PouringBeer:
                        PourBeer(queueToBar);
                        carriedGlass = null;
                        currentPatron = null;
                        break;
                    case State.LeavingWork:
                        CleanBarAndLeaveWork(allPatrons);
                        break;
                }
            }
        }

        private void WaitForPatron(ConcurrentQueue<Patron> queueToBar)
        {
            LogStatus("Waiting for Patron");
            while (queueToBar.IsEmpty)
            {
                Thread.Sleep(50);
            }
        }

        private void WaitForGlass(ConcurrentQueue<Glass> glassesInShelf)
        {
            LogStatus("Waiting for a clean glass");
            while (glassesInShelf.IsEmpty)
            {
                Thread.Sleep(50);
            }
        }

        private void FetchGlass(ConcurrentQueue<Glass> glassesInShelf)
        {
            LogStatus("Fetching a glass");

            ActionDelay(3);
            while (carriedGlass == null)
            {
                glassesInShelf.TryDequeue(out carriedGlass);
            }
            uiUpdater.UpdateGlassesLabel(glassesInShelf.Count());
        }

        private void PourBeer(ConcurrentQueue<Patron> queueToBar)
        {
            while (currentPatron == null)
            {
                queueToBar.TryDequeue(out currentPatron);
            }
            LogStatus($"Pouring a beer for {currentPatron.GetName()}");
            ActionDelay(3);
            carriedGlass.ContainsBeer = true;
            currentPatron.SetBeer(carriedGlass);
        }

        private void CleanBarAndLeaveWork(ConcurrentDictionary<int, Patron> allPatrons)
        {
            LogStatus("Bartender cleans the bar");
            while (!allPatrons.IsEmpty)
            {
                Thread.Sleep(50);
            }
            LogStatus("Bartender leaves the pub");
            LeftPub = true;
        }

        public override void LogStatus(string newStatus)
        {
            uiUpdater.LogBartenderAction(newStatus);
        }

        private void SetState(ConcurrentQueue<Glass> glassesInShelf, ConcurrentQueue<Patron> queueToBar)
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
