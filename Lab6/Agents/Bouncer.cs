using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Lab6
{
    public class Bouncer : Agent
    {
        private int patronID = 0;
        private List<string> patronNames = new List<string> {
            "Anders", "Andreas", "Pontus", "Charlotte", "Tommy", "Petter", "Khosro", "Luna", "Nicklas", "Nils", "Robin",
            "Alexander", "Andreé", "Andreea", "Daniel", "Elvis", "Emil", "Fredrik", "Johan", "John", "Jonas", "Karo",
            "Simon", "Sofia", "Tijana", "Toni", "Wilhelm"
        };
        private enum State { CheckingID, LeavingWork, HappyHour, CouplesNight };
        private State currentState = default;
        private int minInterval = 3;
        private int maxInterval = 10;
        public static bool CouplesNight { get; set; }
        public static bool HappyHour { get; set; }
        private bool completedHappyHourEvent;
        ConcurrentDictionary<int, Patron> allPatrons;
        Action<Patron> createPatronTask;

        public Bouncer(UIUpdater uiUpdater,
            ConcurrentDictionary<int, Patron> allPatrons,
            Action<Patron> createPatronTask)
            : base (uiUpdater)
        {
            this.allPatrons = allPatrons;
            this.createPatronTask = createPatronTask;
        }
        public void Work()
        {
            while (!LeftPub)
            {
                SetState();
                switch (currentState)
                {
                    case State.CheckingID:
                        ActionDelay(RandomNumberGenerator.GetRandomDouble(minInterval, maxInterval), bouncer: this);
                        GeneratePatron();
                        break;
                    case State.LeavingWork:
                        LeavePub();
                        break;
                    case State.HappyHour:
                        HappyHourGeneratePatrons();
                        break;
                    case State.CouplesNight:
                        ActionDelay(RandomNumberGenerator.GetRandomDouble(minInterval, maxInterval), bouncer: this);
                        for (int i = 0; i < 2; i++)
                        {
                            GeneratePatron();
                        }
                        break;
                }
            }
        }

        private void HappyHourGeneratePatrons()
        {
            double countdownUntilRegularEntry = Time.GetCountdown() - (RandomNumberGenerator.GetRandomDouble(minInterval, (maxInterval + 1)) * 2);
            while (Time.GetCountdown() > countdownUntilRegularEntry)
            {
                if (PubClosing)
                    return;
                if (!completedHappyHourEvent && Time.GetCountdown() <= 100)
                {
                    completedHappyHourEvent = true;
                    for (int i = 0; i < 15; i++)
                    {
                        GeneratePatron();
                    }
                }
                Thread.Sleep(100);
            }
            GeneratePatron();
        }

        private void GeneratePatron()
        {
            if (PubClosing)
                return;
            string name = patronNames[(int)RandomNumberGenerator.GetRandomDouble(0, (patronNames.Count() - 1))];
            patronID++;
            Patron tempPatron = new Patron(name, patronID, uiUpdater);
            createPatronTask(tempPatron);
            allPatrons.TryAdd(patronID, tempPatron);
            uiUpdater.UpdatePatronLabel(allPatrons.Count());
        }

        private void LeavePub()
        {
            LogStatus("Bouncer leaves the pub", this);
            LeftPub = true;
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
