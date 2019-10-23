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
        PubManager manager = new PubManager();

        Bartender bartender = new Bartender();
        Bouncer bouncer = new Bouncer();
        Waiter waiter = new Waiter();

        ConcurrentQueue<Patron> queueToBar = new ConcurrentQueue<Patron>();

        public async void Run()
        {
            Task.Run(bartender.WaitForPatron);
            Task.Run(bouncer.AllowPatronEntry);
        }
    }
}
