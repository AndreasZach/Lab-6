using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    public class Waiter : Agent
    {
        private UIUpdater uiUpdater;
        static public double DebugSpeed { get; set; }
        public ConcurrentBag<Glass> glassesCarried = new ConcurrentBag<Glass>();
        enum State { AwaitingWork, GatheringGlasses, WashingDishes, LeavingPub };
        State currentState = default;

        public Waiter(UIUpdater uiUpdater)
        {
            this.uiUpdater = uiUpdater;
        }

        public void Work(ConcurrentBag<Glass> glassesToGather, ConcurrentQueue<Glass> glassesInShelf, ConcurrentDictionary<int, Patron> allPatrons)
        {
            while (!LeftPub)
            {
                SetState(glassesToGather, allPatrons);
                switch (currentState)
                {
                    case State.AwaitingWork:
                        WaitForWork(glassesToGather);
                        break;
                    case State.GatheringGlasses:
                        GatherDirtyGlasses(glassesToGather);
                        break;
                    case State.WashingDishes:
                        CleanAndStoreGlasses(glassesInShelf);
                        break;
                    case State.LeavingPub:
                        LeavePub();
                        break;
                }
            }
        }

        private void WaitForWork(ConcurrentBag<Glass> glassesToGather)
        {
            LogStatus("Waiting for work");
            while (glassesToGather.IsEmpty)
            {
                Thread.Sleep(50);
            }
        }

        private void GatherDirtyGlasses(ConcurrentBag<Glass> glassesToGather)
        {
            LogStatus("Gathering Glasses");
            Thread.Sleep((int)((10000 * DebugSpeed) * SimulationSpeed));
            while (!glassesToGather.IsEmpty)
            {
                glassesToGather.TryTake(out Glass toGather);
                glassesCarried.Add(toGather);
            }
        }

        private void CleanAndStoreGlasses(ConcurrentQueue<Glass> glassesInShelf)
        {
            LogStatus("Washing and storing dishes");
            Thread.Sleep((int)((15000 * DebugSpeed) * SimulationSpeed));
            while (!glassesCarried.IsEmpty)
            {
                glassesCarried.TryTake(out Glass toStore);
                glassesOnShelf.Enqueue(toStore);
                uiUpdater.UpdateGlassesLabel(glassesOnShelf.Count());
            }
        }

        private void LeavePub()
        {
            LogStatus("Waiter Leaves the pub");
            LeftPub = true;
        }
        

        public override void LogStatus(string newStatus)
        {
            uiUpdater.LogWaiterAction(newStatus);
        }

        private void SetState(ConcurrentBag<Glass> glassesToGather, ConcurrentDictionary<int, Patron> allPatrons)
        {
            if (currentState == State.GatheringGlasses)
            {
                currentState = State.WashingDishes;
                return;
            }
            if (!glassesToGather.IsEmpty)
            {
                currentState = State.GatheringGlasses;
                return;
            }
            if (!PubClosing && glassesToGather.IsEmpty)
            {
                currentState = State.AwaitingWork;
                return;
            }
            if (allPatrons.IsEmpty && PubClosing && !glassesCarried.IsEmpty)
            {
                currentState = State.WashingDishes;
                return;
            }
            if (allPatrons.IsEmpty && PubClosing && glassesToGather.IsEmpty)
            {
                currentState = State.LeavingPub;
                return;
            }
        }
    }
}
