using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    class Patron : Agent
    {
        public string Name { get; set; }
        public Glass Beer { get; set; }
        public Chair Chair { get; set; }

        public Patron(string name)
        {
            Name = name;
        }

        public void FindChair()
        {

        }

        public void DrinkBeer()
        {
            
        }
    }
}
