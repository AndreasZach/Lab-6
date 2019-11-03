using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace Lab6
{
    public class Waiter : Agent
    {
        static public double DebugSpeed { get; set; }
        public ConcurrentBag<Glass> glassesCarried = new ConcurrentBag<Glass>();
        private enum State { AwaitingWork, GatheringGlasses, WashingDishes, LeavingPub };
        private State currentState = default;
        ConcurrentQueue<Glass> glassesInShelf;
        ConcurrentBag<Glass> glassesOnTables;
        ConcurrentDictionary<int, Patron> allPatrons;

        public Waiter(UIUpdater uiUpdater,
            ConcurrentQueue<Glass> glassesInShelf,
            ConcurrentBag<Glass> glassesOnTables,
            ConcurrentDictionary<int, Patron> allPatrons)
            : base(uiUpdater)
        {
            this.glassesInShelf = glassesInShelf;
            this.glassesOnTables = glassesOnTables;
            this.allPatrons = allPatrons;
        }

        public void Work()
        {
            while (!LeftPub)
            {
                SetState();
                switch (currentState)
                {
                    case State.AwaitingWork:
                        WaitForWork();
                        break;
                    case State.GatheringGlasses:
                        GatherDirtyGlasses();
                        break;
                    case State.WashingDishes:
                        CleanAndStoreGlasses();
                        break;
                    case State.LeavingPub:
                        LeavePub();
                        break;
                }
            }
        }

        private void WaitForWork()
        {
            LogStatus("Waiting for work", this);
            while (glassesOnTables.IsEmpty)
            {
                Thread.Sleep(50);
            }
        }

        private void GatherDirtyGlasses()
        {
            LogStatus("Gathering Glasses", this);
            ActionDelay((10 * DebugSpeed));
            while (!glassesOnTables.IsEmpty)
            {
                glassesOnTables.TryTake(out Glass toGather);
                glassesCarried.Add(toGather);
            }
        }

        private void CleanAndStoreGlasses()
        {
            LogStatus("Washing and storing dishes", this);
            ActionDelay((15 * DebugSpeed));
            while (!glassesCarried.IsEmpty)
            {
                glassesCarried.TryTake(out Glass toStore);
                glassesInShelf.Enqueue(toStore);
                uiUpdater.UpdateGlassesLabel(glassesInShelf.Count());
            }
        }

        private void LeavePub()
        {
            LogStatus("Waiter Leaves the pub", this);
            LeftPub = true;
            
        }

        private void SetState()
        {
            if (allPatrons.IsEmpty && PubClosing && !glassesCarried.IsEmpty)
            {
                currentState = State.WashingDishes;
                return;
            }
            if (currentState == State.GatheringGlasses)
            {
                currentState = State.WashingDishes;
                return;
            }
            if (!glassesOnTables.IsEmpty)
            {
                currentState = State.GatheringGlasses;
                return;
            }
            if (!PubClosing && glassesOnTables.IsEmpty)
            {
                currentState = State.AwaitingWork;
                return;
            }
            if (allPatrons.IsEmpty && PubClosing && glassesOnTables.IsEmpty)
            {
                currentState = State.LeavingPub;
                return;
            }
        }
    }
}
