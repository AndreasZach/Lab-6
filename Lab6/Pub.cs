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
        bool pubClosing = false;
        ConcurrentQueue<Glass> shelfOfGlasses =  new ConcurrentQueue<Glass>();
        ConcurrentQueue<Chair> availableChairs = new ConcurrentQueue<Chair>();
        ConcurrentQueue<Patron> queueToBar = new ConcurrentQueue<Patron>();
        ConcurrentQueue<Patron> queueToChairs = new ConcurrentQueue<Patron>();
        public int SumAmountGlasses { get; set; }
        public int SumAmountChairs { get; set; }

        Bouncer bouncer = new Bouncer();
        Bartender bartender = new Bartender();
        Waiter waiter = new Waiter();

        public async void Run()
        {
            GeneratePubItems();
            Task.Run( () => BouncerProcess() );
            Task.Run( () => BartenderProcess() );
        }

        public async void BartenderProcess()
        {
            bartender.Work(shelfOfGlasses, queueToBar);
        }

        public async void BouncerProcess()
        {
            while(!pubClosing)
            {
                await Task.Run( () => bouncer.AllowPatronEntry(queueToBar) );
            }
        }

        public async void WaiterProcess()
        {

        }

        public void GeneratePubItems()
        {
            for (int i = 0; i < SumAmountGlasses; i++)
            {
                shelfOfGlasses.Enqueue(new Glass());
            }
            for (int i = 0; i < SumAmountChairs; i++)
            {
                availableChairs.Enqueue(new Chair());
            }
        }
    }
}
