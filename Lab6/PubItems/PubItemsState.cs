using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    class PubItemsState
    {
        // Change Lists location to a Pub manager class, that can manage both PubItems and Agents, Asynchronously, at the same time?"
        List<Chair> availableChairs = new List<Chair>();
        List<Chair> occupiedChairs = new List<Chair>();
        List<Glass> cleanGlasses = new List<Glass>();
        List<Glass> dirtyGlasses = new List<Glass>();

        public PubItemsState(int sumAmountChairs, int sumAmountGlasses)
        {
            SumAmountChairs = sumAmountChairs;
            SumAmountGlasses = sumAmountGlasses;
        }

        public int SumAmountChairs { get; set; } = 9;
        public int SumAmountGlasses { get; set; } = 8;

        public void GeneratePubItemObjects()
        {
            for (int chair = 0; chair < SumAmountChairs; chair++)
            {
                availableChairs.Add(new Chair());
            }
            for (int glass = 0; glass < SumAmountGlasses; glass++)
            {
                cleanGlasses.Add(new Glass());
            }
        }
    }
}
