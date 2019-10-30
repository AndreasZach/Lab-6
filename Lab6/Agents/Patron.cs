using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    public class Patron : Agent
    {
        private UIUpdater uiUpdater;
        static public int DebugTimeToStay { get; set; }
        private int patronID;
        private string name;
        private int minInterval = 20;
        private int maxInterval = 31;
        private Glass carriedBeer;
        private Chair chairUsed;
        enum State { GoingToBar, AwaitingBeer, FindChair, DrinkingBeer, LeavingPub };
        State currentState = default;

        public Patron(string name, int ID, UIUpdater uiUpdater)
        {
            this.name = name;
            this.uiUpdater = uiUpdater;
            patronID = ID;
            LogStatus($"{name} enters the pub");
        }

        public void DrownSorrows(ConcurrentQueue<Patron> queueToBar, ConcurrentQueue<Patron> queueToChairs, ConcurrentQueue<Chair> availableChairs,
            ConcurrentDictionary<int, Patron> allPatrons, ConcurrentBag<Glass> glassesOnTables)
        {
            while (!LeftPub)
            {
                SetState(queueToBar);
                switch (currentState)
                {
                    case State.GoingToBar:
                        GoToBar(queueToBar);
                        break;
                    case State.AwaitingBeer:
                        WaitForBeer();
                        break;
                    case State.FindChair:
                        FindChair(queueToChairs, availableChairs);
                        break;
                    case State.DrinkingBeer:
                        DrinkBeer();
                        break;
                    case State.LeavingPub:
                        LeavePub(allPatrons, availableChairs, glassesOnTables);
                        break;
                }
            }
        }

        private void GoToBar(ConcurrentQueue<Patron> queueToBar)
        {
            Thread.Sleep((int)((1 * DebugTimeToStay) * SimulationSpeed));
            queueToBar.Enqueue(this);
        }

        private void WaitForBeer()
        {
            while (!HasBeer())
            {
                Thread.Sleep(50);
            }
        }

        private void FindChair(ConcurrentQueue<Patron> queueToChair, ConcurrentQueue<Chair> availableChairs)
        {
            LogStatus($"{name} looks for an available chair"); 
            Thread.Sleep((int)((4 * DebugTimeToStay) * SimulationSpeed));
            queueToChair.Enqueue(this);
            while(availableChairs == null)
            {
                Thread.Sleep(50);
            }
            availableChairs.TryDequeue(out chairUsed);
            uiUpdater.UpdateChairLabel(availableChairs.Count());
            _ = queueToChair.TryDequeue(out _);
        }

        private void DrinkBeer()
        {
            LogStatus($"{name} sits down and drinks their beer");
            Thread.Sleep((int)((RandomIntGenerator.GetRandomInt(minInterval, maxInterval) * DebugTimeToStay) * simulationSpeed));
            carriedBeer.ContainsBeer = false;
        }

        private void LeavePub(ConcurrentDictionary<int, Patron> allPatrons, ConcurrentQueue<Chair> availableChairs, ConcurrentBag<Glass> glassesOnTables)
        {
            if(chairUsed != null)
                availableChairs.Enqueue(chairUsed);
            uiUpdater.UpdateChairLabel(availableChairs.Count());
            chairUsed = null;
            glassesOnTables.Add(carriedBeer);
            carriedBeer = null;
            LogStatus($"{name} leaves the pub");
            allPatrons.TryRemove(patronID, out _);
            uiUpdater.UpdatePatronLabel(allPatrons.Count());
            LeftPub = true;
        }

        public string GetName()
        {
            return name;
        }

        public bool HasBeer()
        {
            return carriedBeer != null;
        }

        public void SetBeer(Glass beer)
        {
            carriedBeer = beer;
        }
        

        public override void LogStatus(string newStatus)
        {
            uiUpdater.LogPatronAction(newStatus);
        }

        private void SetState(ConcurrentQueue<Patron> queueToBar)
        {
            if (currentState == State.DrinkingBeer)
            {
                currentState = State.LeavingPub;
                return;
            }
            if (currentState == State.FindChair)
            {
                currentState = State.DrinkingBeer;
                return;
            }
            if (HasBeer() && chairUsed == null)
            {
                currentState = State.FindChair;
                return;
            }
            if (!HasBeer() && !queueToBar.Contains(this))
            {
                currentState = State.GoingToBar;
                return;
            }
            if (!HasBeer() && queueToBar.Contains(this))
            {
                currentState = State.AwaitingBeer;
                return;
            }
        }
    }
}
