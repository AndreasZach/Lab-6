using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6.Agents
{
    class Waiter : Agent
    {
        List<Glass> dirtyGlassesCarried = new List<Glass>();

        public void GatherDirtyGlasses(List<Glass> dirtyGlasses)
        {
            foreach (var dirtyGlass in dirtyGlasses)
            {
                dirtyGlassesCarried.Add(dirtyGlass);
                dirtyGlasses.Remove(dirtyGlass);
            }
        }

        public void WashGlasses()
        {
            // Wash x glasses in r time
            // Put x glasses on shelf

            //foreach (var glass in dirtyGlassesCarried)
            //{
            //    glass.CleanGlass();
            //}
        }
    }
}
