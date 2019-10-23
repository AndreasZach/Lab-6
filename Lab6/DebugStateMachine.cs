using Lab6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    /// <summary>
    /// Code for debugging, ignore remove for eventual release.
    /// </summary>
    class DebugState
    {
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

        Dictionary<Enum, Delegate> stateHandlers = new Dictionary<Enum, Delegate>();


    }
}