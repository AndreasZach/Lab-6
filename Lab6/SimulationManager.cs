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
        // TODO: Make the Time class able to fast forward the countdown, in sync with the Speed setting. (Use a timer with 1000ms tick, that loops until)
        // all Agents have left the pub?
        // TODO: Look at static members and classes, see what we can make more object-oriented.
        // TODO: Add a second window at the end of the sim that shows the user a message, then closes then program when the user clicks a button

        Pub pubSimulation = new Pub();
        public Dictionary<TestState, Action> stateHandlers = new Dictionary<TestState, Action>();
        public Dictionary<string, int> simSpeed = new Dictionary<string, int>
        {
            { "Speed: x1", 1 },
            { "Speed: x2", 2 },
            { "Speed: x4", 4 }
        };
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
                // Sets the timer until Pub closes to 300 seconds, See App.xaml.cs/OpenCloseButton_Click for the code.
                SetPubState();
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

        public void SetPubState(int glasses = 8, int chairs = 9, int patronStayTime = 1000, 
            double waiterSpeed = 1.0, bool HappyHour = false, bool CouplesNight = false)
        {
            pubSimulation.SumAmountGlasses = glasses;
            pubSimulation.SumAmountChairs = chairs;
            Patron.DebugTimeToStay = patronStayTime;
            Waiter.DebugSpeed = waiterSpeed;
            Bouncer.HappyHour = HappyHour;
            Bouncer.CouplesNight = CouplesNight;
            UIUpdater.UpdateGlassesLabel(pubSimulation.SumAmountGlasses);
            UIUpdater.UpdateChairLabel(pubSimulation.SumAmountChairs);
            UIUpdater.UpdatePatronLabel(pubSimulation.allPatrons.Count());
        }

        public void SetTestState(Object selectedState)
        {
            TestState convertedState = (TestState)Enum.Parse(typeof(TestState), selectedState.ToString());
            stateHandlers[convertedState]();
        }

        public void SetSimulationSpeed(object selectedSpeed)
        {
            if (simSpeed[(string)selectedSpeed] == 2)
                Agent.simulationSpeed = 0.5;
            if (simSpeed[(string)selectedSpeed] == 4)
                Agent.simulationSpeed = 0.25;
            if (simSpeed[(string)selectedSpeed] != 2 && simSpeed[(string)selectedSpeed] != 4)
                Agent.simulationSpeed = 1;
        }
    }
}
