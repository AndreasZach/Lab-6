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
        enum State { AwaitingPatron, AwaitingGlass, PouringBeer, FetchingGlass, LeavingWork };
        State currentState = default;

        public void Work(ConcurrentQueue<Glass> glassesInShelf, ConcurrentQueue<Patron> queueToBar, ConcurrentDictionary<int, Patron> allPatrons)
        {
            while (!LeftPub)
            {
                SetState(glassesInShelf, queueToBar, allPatrons);
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
                        break;
                    case State.LeavingWork:
                        LeaveWork();
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
            Thread.Sleep((int)(3000 * simulationSpeed));
            glassesInShelf.TryDequeue(out carriedGlass);
            UIUpdater.UpdateGlassesLabel(glassesInShelf.Count());
        }

        private void PourBeer(ConcurrentQueue<Patron> queueToBar)
        {
            queueToBar.TryDequeue(out currentPatron);
            if (currentPatron == null)
                return;
            LogStatus($"Pouring a beer for {currentPatron.GetName()}");
            Thread.Sleep((int)(3000 * simulationSpeed));
            carriedGlass.ContainsBeer = true;
            if (currentPatron == null)
                return;
            currentPatron.SetBeer(carriedGlass);
            carriedGlass = null;
            currentPatron = null;
        }

        private void LeaveWork()
        {
            LogStatus("Bartender leaves the pub");
            LeftPub = true;
        }

        public override void LogStatus(string newStatus)
        {
            UIUpdater.LogBartenderAction(newStatus);
        }

        private void SetState(ConcurrentQueue<Glass> glassesInShelf, ConcurrentQueue<Patron> queueToBar, ConcurrentDictionary<int, Patron> allPatrons)
        {
            if (currentState == State.FetchingGlass)
            {
                currentState = State.PouringBeer;
                return;
            }
            if (!queueToBar.IsEmpty && !glassesInShelf.IsEmpty && carriedGlass == null)
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
            if (allPatrons.IsEmpty && PubClosing)
            {
                currentState = State.LeavingWork;
            }
        }

    }
}
