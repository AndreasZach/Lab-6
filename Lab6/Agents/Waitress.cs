using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6.Agents
{
    class Waitress : Agent
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

        public void WashDishes()
        {
            foreach (var glass in dirtyGlassesCarried)
            {
                glass.CleanGlass();
            }
        }
    }
}
