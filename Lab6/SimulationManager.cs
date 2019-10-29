using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    public class SimulationManager
    {

        // TODO: Clean up any sloppy code. Make the code more object oriented. (Looking at you, Bouncer class...).
        // TODO: Make the Time class able to fast forward the countdown, in sync with the Speed setting.
        // TODO: Look at static members and classes, see what we can make more object-oriented.

        public Pub pubSimulation;
        public UIUpdater uiUpdater;
        public Dictionary<TestState, Action> stateHandlers = new Dictionary<TestState, Action>();
        public Dictionary<string, int> simSpeed = new Dictionary<string, int>
        {
            { "Speed: x1", 1 },
            { "Speed: x2", 2 },
            { "Speed: x4", 4 }
        };

        public SimulationManager()
        {
            uiUpdater = new UIUpdater();
            pubSimulation = new Pub(uiUpdater);
        }

        public enum TestState
        {
            Standard,
            MoreGlasses,
            MoreChairs,
            PatronDoubleStay,
            TurboWaiter,
            BusinessTimeIncrease,
            CouplesNight,
            HappyHour
        };

        public void Run()
        {
            pubSimulation.OpenPub();
        }

        public void PopulateTestCollection()
        {
            stateHandlers.Add(TestState.Standard, () => 
            {
                SetPubState();
            });
            stateHandlers.Add(TestState.MoreGlasses, () => 
            {
                SetPubState(glasses: 20, chairs: 3);
            });
            stateHandlers.Add(TestState.MoreChairs, () => 
            {
                SetPubState(glasses: 5, chairs: 20);
            });
            stateHandlers.Add(TestState.PatronDoubleStay, () => 
            {
                SetPubState(patronStayTime: 2000);
            });
            stateHandlers.Add(TestState.TurboWaiter, () => 
            {
                SetPubState(waiterSpeed: 0.5);
            });
            stateHandlers.Add(TestState.BusinessTimeIncrease, () => 
            {
                SetPubState(pubTime: 300);
            });
            stateHandlers.Add(TestState.CouplesNight, () => 
            {
                SetPubState(CouplesNight: true);
            });
            stateHandlers.Add(TestState.HappyHour, () =>
            {
                SetPubState(HappyHour: true);
            });
        }

        public void SetPubState(int pubTime = 120, int glasses = 8, int chairs = 9, int patronStayTime = 1000, 
            double waiterSpeed = 1.0, bool HappyHour = false, bool CouplesNight = false)
        {
            Time.SimulationTime = pubTime;
            pubSimulation.SumAmountGlasses = glasses;
            pubSimulation.SumAmountChairs = chairs;
            Patron.DebugTimeToStay = patronStayTime;
            Waiter.DebugSpeed = waiterSpeed;
            Bouncer.HappyHour = HappyHour;
            Bouncer.CouplesNight = CouplesNight;
            uiUpdater.UpdateGlassesLabel(pubSimulation.SumAmountGlasses);
            uiUpdater.UpdateChairLabel(pubSimulation.SumAmountChairs);
            uiUpdater.UpdatePatronLabel(pubSimulation.allPatrons.Count());
            uiUpdater.UpdateCountDownLabel(Time.SimulationTime);
        }

        public void SetTestState(Object selectedState)
        {
            TestState convertedState = (TestState)Enum.Parse(typeof(TestState), selectedState.ToString());
            stateHandlers[convertedState]();
        }

        public void SetSimulationSpeed(object selectedSpeed)
        {
            if (simSpeed[(string)selectedSpeed] == 2)
            {
                Agent.SimulationSpeed = 0.5;
                Time.NewSimulationSpeed = 0.5;
                Time.ChangePubHours();
            }
            if (simSpeed[(string)selectedSpeed] == 4)
            {
                Agent.SimulationSpeed = 0.25;
                Time.NewSimulationSpeed = 0.25;
                Time.ChangePubHours();
            }
            if (simSpeed[(string)selectedSpeed] != 2 && simSpeed[(string)selectedSpeed] != 4)
            {
                Agent.SimulationSpeed = 1;
                Time.NewSimulationSpeed = 1;
                Time.ChangePubHours();
            }
        }
    }
}
