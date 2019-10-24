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

        ConcurrentQueue<Glass> shelfOfGlasses =  new ConcurrentQueue<Glass>();
        ConcurrentQueue<Chair> availableChairs = new ConcurrentQueue<Chair>();
        ConcurrentQueue<Patron> queueToBar = new ConcurrentQueue<Patron>();
        public int SumAmountGlasses { get; set; }
        public int SumAmountChairs { get; set; }
        bool pubClosing = false;

        public async void Run()
        {
            GeneratePubItems();
            Task.Run( () => BouncerProcess() );
            Task.Run( () => BartenderProcess() );
        }

        public void BartenderProcess()
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
