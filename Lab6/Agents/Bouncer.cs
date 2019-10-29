using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    public class Bouncer : Agent
    {
        private UIUpdater uiUpdater;
        private int patronID = 0;
        private string name = null;
        private List<string> patronNames = new List<string> {
            "Anders", "Andreas", "Pontus", "Charlotte", "Tommy", "Petter", "Khosro", "Luna", "Nicklas", "Nils", "Robin",
            "Alexander", "Andreé", "Andreea", "Daniel", "Elvis", "Emil", "Fredrik", "Johan", "John", "Jonas", "Karo",
            "Simon", "Sofia", "Tijana", "Toni", "Wilhelm"
        };

        private int minInterval = 3;
        private int maxInterval = 11;
        static public bool CouplesNight { get; set; }
        static public bool HappyHour { get; set; }
        private bool completedHappyHourEvent = false;

        public Bouncer(UIUpdater uiUpdater)
        {
            this.uiUpdater = uiUpdater;
        }

        public void AllowPatronEntry(ConcurrentDictionary<int, Patron> allPatrons, Action<Patron> createPatronTask)
        {
            if (CouplesNight)
            {
                DebugCouplesNight(allPatrons, createPatronTask);
                return;
            }
            if (HappyHour)
            {
                DebugHappyHour(allPatrons, createPatronTask);
                return;
            }
            Thread.Sleep((RandomIntGenerator.GetRandomInt(minInterval, maxInterval)) * (int)(1000 * SimulationSpeed));
            if (EndWork)
                return;
            name = patronNames[RandomIntGenerator.GetRandomInt(0, (patronNames.Count() - 1))];
            if (name != null)
            {
                patronID++;
                Patron tempPatron = new Patron(name, patronID, uiUpdater);
                createPatronTask(tempPatron);
                allPatrons.TryAdd(patronID, tempPatron);
                uiUpdater.UpdatePatronLabel(allPatrons.Count());
                name = null;
            }
        }

        private void DebugHappyHour(ConcurrentDictionary<int, Patron> allPatrons, Action<Patron> createPatronTask)
        {
            if (!completedHappyHourEvent)
            {
                Task.Run(() =>
                {
                    Thread.Sleep((int)(20000 * SimulationSpeed));
                    for (int i = 0; i < 15; i++)
                    {
                        string tempName;
                        tempName = patronNames[RandomIntGenerator.GetRandomInt(0, (patronNames.Count()))];
                        patronID++;
                        Patron groupPatron = new Patron(tempName, patronID, uiUpdater);
                        createPatronTask(groupPatron);
                        allPatrons.TryAdd(patronID, groupPatron);
                        tempName = null;
                    }
                    uiUpdater.UpdatePatronLabel(allPatrons.Count()); // Inside the for-loop? Performance heavy.
                });
            }
            completedHappyHourEvent = true;
            Thread.Sleep((RandomIntGenerator.GetRandomInt(minInterval, maxInterval)) * (int)(2000 * SimulationSpeed));
            if (EndWork)
                return;
            name = patronNames[RandomIntGenerator.GetRandomInt(0, (patronNames.Count() - 1))];
            patronID++;
            Patron tempPatron = new Patron(name, patronID, uiUpdater);
            createPatronTask(tempPatron);
            allPatrons.TryAdd(patronID, tempPatron);
            uiUpdater.UpdatePatronLabel(allPatrons.Count());
            name = null;
        }

        private void DebugCouplesNight(ConcurrentDictionary<int, Patron> allPatrons, Action<Patron> createPatronTask)
        {
            if (EndWork)
                return;
            Thread.Sleep((RandomIntGenerator.GetRandomInt(minInterval, maxInterval)) * (int)(1000 * SimulationSpeed));
            for (int i = 0; i < 2; i++)
            {
                string tempName;
                tempName = patronNames[RandomIntGenerator.GetRandomInt(0, (patronNames.Count() - 1))];
                patronID++;
                Patron tempPatron = new Patron(tempName, patronID, uiUpdater);
                createPatronTask(tempPatron);
                allPatrons.TryAdd(patronID, tempPatron);
                uiUpdater.UpdatePatronLabel(allPatrons.Count());
                name = null;
            }
        }

        public override void LogStatus(string newStatus)
        {
            uiUpdater.LogPatronAction(newStatus);
        }
    }
}
