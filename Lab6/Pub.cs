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
        ConcurrentBag<Chair> availableChairs = new ConcurrentBag<Chair>();
        ConcurrentQueue<Patron> queueToBar = new ConcurrentQueue<Patron>();
        ConcurrentQueue<Patron> queueToChairs = new ConcurrentQueue<Patron>();
        SynchronizedColl<Patron> allPatrons = new ConcurrentBag<Patron>();

        public int SumAmountGlasses { get; set; }
        public int SumAmountChairs { get; set; }

        Bouncer bouncer = new Bouncer();
        Bartender bartender = new Bartender();
        Waiter waiter = new Waiter();

        public void Run()
        {
            GeneratePubItems();
            Task.Run(() => BouncerProcess());
            Task.Run(() => BartenderProcess());
            Task.Run(() => BouncerProcess());
        }

        public void BartenderProcess()
        {
            //bartender.Work(shelfOfGlasses, queueToBar);
            //bartender.FetchGlass();
            //bartender.ServeBeer();
        }

        public void BouncerProcess()
        {
            while(!pubClosing)
            {
                //bouncer.AllowPatronEntry(allPatrons, PatronProcess);
            }
        }

        public void WaiterProcess()
        {

        }

        public void PatronProcess(Patron patron)
        {
            Task.Run(() => 
            {
                patron.GoToBar(queueToBar);
                patron.FindChair(queueToBar, queueToChairs, availableChairs);
                patron.DrinkBeer(allPatrons, availableChairs);
            });
        }

        public void GeneratePubItems()
        {
            for (int i = 0; i < SumAmountGlasses; i++)
            {
                shelfOfGlasses.Enqueue(new Glass());
            }
            for (int i = 0; i < SumAmountChairs; i++)
            {
                availableChairs.Add(new Chair());
            }
        }
    }
}
