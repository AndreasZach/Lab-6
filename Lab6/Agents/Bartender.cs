using System;
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

        public void FetchGlass()
        {
            Thread.Sleep(); //Call method for correct time.
        }

        public void ServeBeer()
        {
            Thead.Sleep(); //Call method for correct time.
        }
    }
}
