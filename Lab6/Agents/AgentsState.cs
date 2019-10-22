using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6.Agents
{
    class AgentsState
    {
        // Change Lists location to a Pub manager class, that can manage both PubItems and Agents, Asynchronously, at the same time?"
        List<Glass> shelfContainer = new List<Glass>();
        List<Chair> unoccupiedChairs = new List<Chair>();
        List<Patron> currentPatrons = new List<Patron>();

        Bartender bartender = new Bartender();
        Bouncer bouncer = new Bouncer();
        Waiter waitress = new Waiter();
    }
}
