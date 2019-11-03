using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    public class Pub
    {
        private UIUpdater uiUpdater;
        ConcurrentQueue<Glass> glassesInShelf =  new ConcurrentQueue<Glass>();
        ConcurrentQueue<Chair> availableChairs = new ConcurrentQueue<Chair>();
        ConcurrentBag<Glass> glassesOnTables = new ConcurrentBag<Glass>();
        ConcurrentQueue<Patron> queueToBar = new ConcurrentQueue<Patron>();
        ConcurrentQueue<Patron> queueToChairs = new ConcurrentQueue<Patron>();
        ConcurrentBag<Task> patronTasks = new ConcurrentBag<Task>();
        ConcurrentDictionary<int, Patron> allPatrons = new ConcurrentDictionary<int, Patron>();
        Bouncer bouncer;
        Bartender bartender;
        Waiter waiter;

        public Pub(UIUpdater uiUpdater)
        {
            this.uiUpdater = uiUpdater;
            bouncer = new Bouncer(uiUpdater, allPatrons, PatronProcess);
            bartender = new Bartender(uiUpdater, glassesInShelf, queueToBar, allPatrons);
            waiter = new Waiter(uiUpdater, glassesInShelf, glassesOnTables, allPatrons);
        }

        public int SumAmountGlasses { get; set; }
        public int SumAmountChairs { get; set; }

        public void OpenPub()
        {
            GeneratePubItems();
            uiUpdater.CountdownComplete += ClosePub;
            Task.Run(() => BouncerProcess()); 
            Task.Run(() => BartenderProcess());
            Task.Run(() => WaiterProcess());
        }

        public void ClosePub()
        {
            bartender.PubClosing = true;
            bouncer.PubClosing = true;
            waiter.PubClosing = true;
            Task.Run(() =>
            {
                while (true)
                {
                    if (bartender.LeftPub && waiter.LeftPub && bouncer.LeftPub)
                    {
                        uiUpdater.StopCountdown = true;
                        uiUpdater.ShowEndMessage();
                        break;
                    }
                    Thread.Sleep(1000);
                }
            });
        }

        public void BartenderProcess()
        {
            bartender.Work();
        }

        public void BouncerProcess()
        {
            bouncer.Work();
        }

        public void WaiterProcess()
        { 
            waiter.Work();
        }

        public void PatronProcess(Patron patron)
        {
            patronTasks.Add(Task.Run(() => 
            {
                patron.VisitPub(allPatrons, queueToBar, queueToChairs, availableChairs, glassesOnTables);
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
