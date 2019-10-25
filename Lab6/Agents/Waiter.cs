using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    class Waiter : Agent
    {
        ConcurrentBag<Glass> dirtyGlassedCarried = new ConcurrentBag<Glass>();

        public void GatherDirtyGlasses(ConcurrentBag<Glass> glassesToGather)
        {
            
        }

        public void CleanAndStoreGlasses()
        {
            
        }
    }
}
