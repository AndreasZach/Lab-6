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
        bool pubClosing = false;
        ConcurrentQueue<Glass> glassesInShelf =  new ConcurrentQueue<Glass>();
        ConcurrentBag<Glass> glassesOnTables = new ConcurrentBag<Glass>();
        ConcurrentBag<Chair> availableChairs = new ConcurrentBag<Chair>();
        ConcurrentQueue<Patron> queueToBar = new ConcurrentQueue<Patron>();
        ConcurrentQueue<Patron> queueToChairs = new ConcurrentQueue<Patron>();
        ConcurrentDictionary<int, Patron> allPatrons = new ConcurrentDictionary<int, Patron>();
        ConcurrentBag<Task> patronTasks = new ConcurrentBag<Task>();

        public int SumAmountGlasses { get; set; } = 8;
        public int SumAmountChairs { get; set; } = 9;

        Bouncer bouncer = new Bouncer();
        Bartender bartender = new Bartender();
        Waiter waiter = new Waiter();

        public void Run()
        {
            GeneratePubItems();
            Task.Run(() => BouncerProcess()); 
            Task.Run(() => BartenderProcess());
            Task.Run(() => WaiterProcess());
        }

        public void BartenderProcess()
        {
            while (!pubClosing && !allPatrons.IsEmpty)
            {
                bartender.Work(glassesInShelf, queueToBar);
                bartender.FetchGlass();
                bartender.ServeBeer();
            }
            bartender.SendStatusToLog("Bartender leaves the pub");
        }

        public void BouncerProcess()
        {
            while(!pubClosing)
            {
                bouncer.AllowPatronEntry(allPatrons, PatronProcess);
            }
            bouncer.SendStatusToLog("Bouncer leaves the pub");
        }

        public void WaiterProcess()
        {
            while (!pubClosing && !allPatrons.IsEmpty && glassesOnTables.IsEmpty)
            {
                waiter.GatherDirtyGlasses(glassesOnTables);
                waiter.CleanAndStoreGlasses(glassesInShelf);
            }
            waiter.SendStatusToLog("Waiter leaves the pub");
        }

        public void PatronProcess(Patron patron)
        {
            patronTasks.Add(Task.Run(() => 
            {
                patron.GoToBar(queueToBar);
                patron.FindChair(queueToChairs, availableChairs);
                patron.DrinkBeer();
                patron.LeaveBar(allPatrons, availableChairs, glassesOnTables);
            }));
        }

        public void ClosePub()
        {
            pubClosing = true;
            bouncer.OnPubClosing();
        }

        public void GeneratePubItems()
        {
            for (int i = 0; i < SumAmountGlasses; i++)
            {
                glassesInShelf.Enqueue(new Glass());
            }
            for (int i = 0; i < SumAmountChairs; i++)
            {
                availableChairs.Add(new Chair());
            }
        }
    }
}
