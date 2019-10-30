using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    public class Pub
    {
        private UIUpdater uiUpdater;
        bool pubClosing = false;
        public ConcurrentQueue<Glass> glassesInShelf =  new ConcurrentQueue<Glass>();
        public ConcurrentQueue<Chair> availableChairs = new ConcurrentQueue<Chair>();
        public ConcurrentDictionary<int, Patron> allPatrons = new ConcurrentDictionary<int, Patron>();
        ConcurrentBag<Glass> glassesOnTables = new ConcurrentBag<Glass>();
        ConcurrentQueue<Patron> queueToBar = new ConcurrentQueue<Patron>();
        ConcurrentQueue<Patron> queueToChairs = new ConcurrentQueue<Patron>();
        ConcurrentBag<Task> patronTasks = new ConcurrentBag<Task>();
        Bouncer bouncer;
        Bartender bartender;
        Waiter waiter;

        public Pub(UIUpdater uiUpdater)
        {
            this.uiUpdater = uiUpdater;
            bouncer = new Bouncer(uiUpdater);
            bartender = new Bartender(uiUpdater);
            waiter = new Waiter(uiUpdater);
        }

        public int SumAmountGlasses { get; set; }
        public int SumAmountChairs { get; set; }

        public void OpenPub()
        {
            GeneratePubItems();
            Time.CountdownComplete += ClosePub;
            Task.Run(() => BouncerProcess()); 
            Task.Run(() => BartenderProcess());
            Task.Run(() => WaiterProcess());
        }

        public void ClosePub()
        {
            pubClosing = true;
            bartender.PubClosing = true;
            bouncer.PubClosing = true;
            waiter.PubClosing = true;
        }

        public void BartenderProcess()
        {
            bartender.Work(glassesInShelf, queueToBar, allPatrons);
        }

        public void BouncerProcess()
        {
            bouncer.Work(allPatrons, PatronProcess);
        }

        public void WaiterProcess()
        { 
            waiter.Work(glassesOnTables, glassesInShelf, allPatrons);
        }

        public void PatronProcess(Patron patron)
        {
            patronTasks.Add(Task.Run(() => 
            {
                patron.DrownSorrows(queueToBar, queueToChairs, availableChairs, allPatrons, glassesOnTables);
            }));
        }

        public void GeneratePubItems()
        {
            for (int i = 0; i < SumAmountGlasses; i++)
            {
                glassesInShelf.Enqueue(new Glass());
            }
            for (int i = 0; i < SumAmountChairs; i++)
            {
                availableChairs.Enqueue(new Chair());
            }
        }
    }
}
