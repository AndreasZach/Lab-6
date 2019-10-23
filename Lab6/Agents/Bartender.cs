using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    class Bartender : Agent
    {
        public Glass CarriedGlass { get; set; }
        public int MyProperty { get; set; }

        public void WaitForPatron()
        {
            //TODO: Match with async
        }

        public void FetchGlass(ConcurrentQueue<Glass> glass)
        {
            Glass toHold;
            glass.TryDequeue(out toHold);
            CarriedGlass = toHold;
            Thread.Sleep(); //Call method for correct time.
            ServeBeer();
        }

        public Glass ServeBeer()
        {
            Thread.Sleep(); //Call method for correct time.
            return CarriedGlass;
        }
    }
}
