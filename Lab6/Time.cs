using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private static int countDown;
        private static string timeStamp;

        public static double NewSimulationSpeed { get; set; }
        public static int SimulationTime { get; set; }

        public static void SetPubHours()
        {
            openTime = DateTime.Now;
            closeTime = openTime.AddSeconds(SimulationTime * NewSimulationSpeed);
            countDown = SimulationTime;
        }

        public static void ChangePubHours()
        {
            closeTime = DateTime.Now.AddMilliseconds((closeTime.Subtract(DateTime.Now).TotalMilliseconds) / oldSimulationSpeed * NewSimulationSpeed);
            oldSimulationSpeed = NewSimulationSpeed;
        }
        public static void PrintCountdown(MainWindow pubWindow)
        {
            Task.Run(() =>
            {
                while (countDown > 0)
                {

                    countDown = (int)(closeTime.Subtract(DateTime.Now).TotalSeconds / NewSimulationSpeed);
                    pubWindow.Dispatcher.Invoke(() => pubWindow.CountDownLabel.Content = $"{countDown} s");
                    Thread.Sleep(100);
                }
                pubWindow.Dispatcher.Invoke(() => pubWindow.CountDownLabel.Content = $"Pub Closing");
                CountdownComplete();
            });
        }

        public static string GetTimeStamp()
        {
            TimeSpan timeDiff = DateTime.Now.Subtract(openTime);
            timeStamp = String.Format("{0:D2}.{1:D2}_", timeDiff.Minutes, timeDiff.Seconds);
            return timeStamp;
        }
    }
}
