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
        private List<string> patronNames = new List<string> {
            "Anders", "Andreas", "Pontus", "Charlotte", "Tommy", "Petter", "Khosro", "Luna", "Nicklas", "Nils", "Robin",
            "Alexander", "Andreé", "Andreea", "Daniel", "Elvis", "Emil", "Fredrik", "Johan", "John", "Jonas", "Karo",
            "Simon", "Sofia", "Tijana", "Toni", "Wilhelm"
        };
        enum State { CheckingID, LeavingWork, HappyHour, CouplesNight };
        State currentState = default;
        private int minInterval = 3;
        private int maxInterval = 10;
        static public bool CouplesNight { get; set; }
        static public bool HappyHour { get; set; }
        private bool completedHappyHourEvent = false;
        public Bouncer(UIUpdater uiUpdater)
        {
            this.uiUpdater = uiUpdater;
        }
        
        public void Work(ConcurrentDictionary<int, Patron> allPatrons, Action<Patron> createPatronTask)
        {
            while (!LeftPub)
            {
                SetState();
                switch (currentState)
                {
                    case State.CheckingID:
                        ActionDelay(RandomNumberGenerator.GetRandomDouble(minInterval, maxInterval));
                        GeneratePatron(allPatrons, createPatronTask);
                        break;
                    case State.LeavingWork:
                        LeavePub();
                        break;
                    case State.HappyHour:
                        HappyHourGeneratePatrons(allPatrons, createPatronTask);
                        break;
                    case State.CouplesNight:
                        ActionDelay(RandomNumberGenerator.GetRandomDouble(minInterval, maxInterval));
                        for (int i = 0; i < 2; i++)
                        {
                            GeneratePatron(allPatrons, createPatronTask);
                        }
                        break;
                }
            }
        }

        private void HappyHourGeneratePatrons(ConcurrentDictionary<int, Patron> allPatrons, Action<Patron> createPatronTask)
        {
            double countdownUntilRegularEntry = Time.countdown - (RandomNumberGenerator.GetRandomDouble(minInterval, (maxInterval + 1)) * 2);
            while (Time.countdown > countdownUntilRegularEntry)
            {
                if (!completedHappyHourEvent && Time.countdown <= 100)
                {
                    completedHappyHourEvent = true;
                    for (int i = 0; i < 15; i++)
                    {
                        GeneratePatron(allPatrons, createPatronTask);
                    }
                }
            }
            GeneratePatron(allPatrons, createPatronTask);
        }

        private void GeneratePatron(ConcurrentDictionary<int, Patron> allPatrons, Action<Patron> createPatronTask)
        {
            string name = patronNames[(int)RandomNumberGenerator.GetRandomDouble(0, (patronNames.Count() - 1))];
            patronID++;
            Patron tempPatron = new Patron(name, patronID, uiUpdater);
            createPatronTask(tempPatron);
            allPatrons.TryAdd(patronID, tempPatron);
            uiUpdater.UpdatePatronLabel(allPatrons.Count());
        }

        private void LeavePub()
        {
            LogStatus("Bouncer leaves the pub");
            LeftPub = true;
        }

        public override void LogStatus(string newStatus)
        {
            uiUpdater.LogPatronAction(newStatus);
        }

        private void SetState()
        {
            if(!PubClosing && HappyHour)
            {
                currentState = State.HappyHour;
                return;
            }
            if (!PubClosing && CouplesNight)
            {
                currentState = State.CouplesNight;
                return;
            }
            if (PubClosing)
            {
                currentState = State.LeavingWork;
                return;
            }
            if (!PubClosing)
            {
                currentState = State.CheckingID;
            }
        }
    }
}
