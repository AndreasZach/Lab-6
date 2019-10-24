using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    class Pub
    {
        public int NumberOfPatrons { get; set; }
        ConcurrentQueue<Patron> queueToBar = new ConcurrentQueue<Patron>();
        ConcurrentQueue<Patron> queueToChairs = new ConcurrentQueue<Patron>();
        Queue<Glass> glassesOnShelf = new Queue<Glass>();
        Stack<Chair> availibleChairs = new Stack<Chair>();

        public Pub()
        {
            PubManager manager = new PubManager();

            Bartender bartender = new Bartender();
            Bouncer bouncer = new Bouncer();
            Waiter waiter = new Waiter();
        }

        public async void Run()
        {
            Task.Run(bartender.WaitForPatron);
            Task.Run(bouncer.AllowPatronEntry);
        }
    }
}
