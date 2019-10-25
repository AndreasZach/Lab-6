using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    class Bouncer : Agent
    {
        private int patronID = 0;
        private string name = null;
        private List<string> patronNames = new List<string> { };
        private bool endWork = false;
        public bool CouplesNight { get; set; } = false;
        public bool HappyHour { get; set; } = false;
        private bool completedHappyHourEvent = false;
        public int minInterval { get; set; }
        public int maxInterval { get; set; }

        public void AllowPatronEntry(ConcurrentDictionary<int, Patron> allPatrons, Action<Patron> createPatronTask)
        {
            if (CouplesNight)
                DebugCouplesNight(allPatrons, createPatronTask);
            if (HappyHour)
                DebugHappyHour(allPatrons, createPatronTask);
            Thread.Sleep((RandomIntGenerator.GetRandomInt(minInterval, maxInterval)) * 1000);
            if (endWork)
                return;
            if (name != null)
            {
                name = patronNames[RandomIntGenerator.GetRandomInt(0, (patronNames.Count() - 1))];
                patronID++;
                Patron tempPatron = new Patron(name, patronID);
                createPatronTask(tempPatron);
                allPatrons.TryAdd(patronID, tempPatron);
                name = null;
            }
        }

        private void DebugHappyHour(ConcurrentDictionary<int, Patron> allPatrons, Action<Patron> createPatronTask)
        {
            if (!completedHappyHourEvent)
            {
                Task.Run(() =>
                {
                    Thread.Sleep(20000);
                    for (int i = 0; i < 15; i++)
                    {
                        string tempName;
                        tempName = patronNames[RandomIntGenerator.GetRandomInt(0, (patronNames.Count() - 1))];
                        patronID++;
                        Patron tempPatron = new Patron(name, patronID);
                        createPatronTask(tempPatron);
                        allPatrons.TryAdd(patronID, tempPatron);
                        tempName = null;
                    }
                    completedHappyHourEvent = true;
                });
            }
            Thread.Sleep((RandomIntGenerator.GetRandomInt(minInterval, maxInterval)) * 2000);
            if (endWork)
                return;
            if (name != null)
            {
                name = patronNames[RandomIntGenerator.GetRandomInt(0, (patronNames.Count() - 1))];
                patronID++;
                Patron tempPatron = new Patron(name, patronID);
                createPatronTask(tempPatron);
                allPatrons.TryAdd(patronID, tempPatron);
                name = null;
            }
        }

        private void DebugCouplesNight(ConcurrentDictionary<int, Patron> allPatrons, Action<Patron> createPatronTask)
        {
            Thread.Sleep((RandomIntGenerator.GetRandomInt(minInterval, maxInterval)) * 1000);
            if (endWork)
                return;
            if (name != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    name = patronNames[RandomIntGenerator.GetRandomInt(0, (patronNames.Count() - 1))];
                    patronID++;
                    Patron tempPatron = new Patron(name, patronID);
                    createPatronTask(tempPatron);
                    allPatrons.TryAdd(patronID, tempPatron);
                    name = null;
                }
            }
        }

        public void OnPubClosing()
        {
            endWork = true;
        }
    }
}
