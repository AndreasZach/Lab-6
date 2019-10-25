using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    public class SimulationManager
    {
        public MainWindow pubWindow;
        Pub pubSimulation = new Pub();
        public enum DebugTestState
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
        Dictionary<Enum, Action> stateHandlers = new Dictionary<Enum, Action>();


        public SimulationManager(MainWindow window)
        {
            pubWindow = window;
        }

        public void Run()
        {
            Time.SetPubHours(120);
            Time.PrintCountdown(pubWindow);
        }
        
        public void SetTestStateData()
        {
            
        }
    }
}
