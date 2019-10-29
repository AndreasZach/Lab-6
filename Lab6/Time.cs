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
        private static DateTime closeTime;
        private static int countDown;
        private static bool pubPaused;
        private static DateTime pauseStartTime;
        private static DateTime pauseStopTime;
        private static string timeStamp;

        public static void SetPubHours(int seconds)
        {
            openTime = DateTime.Now;
            closeTime = openTime.AddSeconds(seconds);
            countDown = seconds;
            //Timer timer;
        }

        public static void PrintCountdown(MainWindow pubWindow)
        {
            Task.Run(() =>
            {
                while (countDown > 0 && !pubPaused)
                {
                        countDown = (int)closeTime.Subtract(DateTime.Now).TotalSeconds ;
                        pubWindow.Dispatcher.Invoke(() => pubWindow.CountDownLabel.Content = $"{countDown} s");
                        Task.Delay(200);
                }
                pubWindow.Dispatcher.Invoke(() => pubWindow.CountDownLabel.Content = $"Pub Closing");
                CountdownComplete();
            });
        }


        public static void PausePub(bool pause)
        {
            if (pause)
            {
                if (pauseStartTime <= openTime)
                {
                    pubPaused = pause;
                    pauseStartTime = DateTime.Now;
                }
            }
            if (!pause)
            {
                if (pauseStartTime > pauseStopTime)
                {
                    pubPaused = !pause;
                    pauseStopTime = DateTime.Now;
                    ExtendPubHour((int)(pauseStopTime.Subtract(pauseStartTime).TotalMilliseconds));
                    pauseStartTime = default;
                    pauseStopTime = default;
                }

            }
        }
        public static void ExtendPubHour(int pauseTime)
        {
            closeTime = closeTime.AddMilliseconds(pauseTime);
        }
        
        public static string GetTimeStamp()
        {
            TimeSpan timeDiff = DateTime.Now.Subtract(openTime);
            timeStamp = String.Format("{0:D2}.{1:D2}_", timeDiff.Minutes, timeDiff.Seconds);
            return timeStamp;
        }
    }
}
