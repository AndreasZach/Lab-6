using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    class Bouncer : Agent
    {
        //Add random name generator method to fill the List. Need to make it many names, so as to never let it get empty.
        public ConcurrentBag<string> patronNames = new ConcurrentBag<string>(); 

        public Patron AllowPatronEntry()
        {
            string name;
            patronNames.TryTake(out name);
            if (name == null)
                return null;
            return new Patron(name);
        }
    }
}
