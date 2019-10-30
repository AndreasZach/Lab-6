using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

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

        public void Work(ConcurrentBag<Glass> glassesToGather, ConcurrentQueue<Glass> glassesOnShelf, ConcurrentDictionary<int, Patron> allPatrons)
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
                        CleanAndStoreGlasses(glassesOnShelf);
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
            ActionDelay(10);
            while (!glassesToGather.IsEmpty)
            {
                glassesToGather.TryTake(out Glass toGather);
                glassesCarried.Add(toGather);
            }
        }

        private void CleanAndStoreGlasses(ConcurrentQueue<Glass> glassesInShelf)
        {
            LogStatus("Washing and storing dishes");
            ActionDelay(15);
            while (!glassesCarried.IsEmpty)
            {
                glassesCarried.TryTake(out Glass toStore);
                glassesInShelf.Enqueue(toStore);
                uiUpdater.UpdateGlassesLabel(glassesInShelf.Count());
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
