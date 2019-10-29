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
        public ConcurrentBag<Chair> availableChairs = new ConcurrentBag<Chair>();
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
            int debugGlassesCount = glassesInShelf.Count();
            int debugChairsCount = availableChairs.Count();
            int debugPatronCount = allPatrons.Count();
            GeneratePubItems();
            Time.CountdownComplete += ClosePub;
            Task.Run(() => BouncerProcess()); 
            Task.Run(() => BartenderProcess());
            Task.Run(() => WaiterProcess());
        }

        public void ClosePub()
        {
            pubClosing = true;
            Agent.EndWork = true;
        }

        public void BartenderProcess()
        {
            while (true)
            {
                bartender.Work(glassesInShelf, queueToBar);
                bartender.FetchGlass();
                bartender.ServeBeer();
                if (pubClosing && allPatrons.IsEmpty)
                    break;
            }
            bartender.CleanUp(glassesInShelf);
            bartender.LogStatus("Bartender leaves the pub");
        }

        public void BouncerProcess()
        {
            while(true)
            {
                bouncer.AllowPatronEntry(allPatrons, PatronProcess);
                if (pubClosing)
                    break;
            }
            bouncer.LogStatus("Bouncer leaves the pub");
        }

        public void WaiterProcess()
        {
            while (true)
            {
                waiter.GatherDirtyGlasses(glassesOnTables);
                waiter.CleanAndStoreGlasses(glassesInShelf);
                if (pubClosing && allPatrons.IsEmpty && glassesOnTables.IsEmpty)
                    break;
            }
            waiter.LogStatus("Waiter leaves the pub");
            uiUpdater.UpdateGlassesLabel(glassesInShelf.Count());
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
