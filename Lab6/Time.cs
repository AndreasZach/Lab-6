using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    public static class Time
    {
        static public event Action CountdownComplete;
        private static DateTime openTime;
        private static DateTime closeTime = DateTime.Now;
        private static double oldSimulationSpeed = 1;
        public static double countdown;
        private static string timeStamp;
        

        public static double NewSimulationSpeed { get; set; }
        public static int SimulationTime { get; set; }

        public static void SetPubHours()
        {
            openTime = DateTime.Now;
            closeTime = openTime.AddSeconds(SimulationTime * NewSimulationSpeed);
            countdown = SimulationTime;
        }

        public static void ChangePubHours()
        {
            closeTime = DateTime.Now.AddMilliseconds((closeTime.Subtract(DateTime.Now).TotalMilliseconds) / oldSimulationSpeed * NewSimulationSpeed);
            oldSimulationSpeed = NewSimulationSpeed;
        }
        public static void Countdown(MainWindow pubWindow)
        {
            Task.Run(() =>
            {
                bool countdownSubZero = false;
                while (true)
                {
                    countdown = (closeTime.Subtract(DateTime.Now).TotalSeconds / NewSimulationSpeed);
                    if (countdown >= 0)
                    {
                        pubWindow.Dispatcher.Invoke(() => pubWindow.CountDownLabel.Content = $"{(int)countdown} s");
                    }
                    if (!countdownSubZero && countdown < 0)
                    {
                        pubWindow.Dispatcher.Invoke(() => pubWindow.CountDownLabel.Content = $"Pub Closing");
                        CountdownComplete();
                        countdownSubZero = true;
                    }
                    Thread.Sleep(100);
                }
            });
        }

        public static string GetTimeStamp()
        {
            int timeDiff = (int)(SimulationTime - countdown);
            int minutes = timeDiff / 60;
            int seconds = timeDiff - (minutes * 60);
            timeStamp = String.Format("{0:D2}.{1:D2}", minutes, seconds);
            return timeStamp;
        }
    }
}
