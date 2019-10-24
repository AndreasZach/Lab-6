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
        private static DateTime openTime;
        private static DateTime closeTime;
        private static int countDown;
        private static bool pubPaused;
        private static string timeStamp;

        public static void SetPubHours(int seconds)
        {
            openTime = DateTime.Now;
            closeTime = openTime.AddSeconds(seconds);
            countDown = seconds;
            //Timer timer;
        }

        public static void PrintCountdown()
        {
            while (countDown > 0 && pubPaused)
            {
                countDown = (int)closeTime.Subtract(DateTime.Now).TotalSeconds;
                Console.WriteLine($"{countDown} s");
                Thread.Sleep(100);
            }
        }

        public static string GetTimeStamp()
        {
            TimeSpan timeDiff = DateTime.Now.Subtract(openTime);
            timeStamp = String.Format("{0:D2}.{1:D2}_", timeDiff.Minutes, timeDiff.Seconds);
            return timeStamp;
        }

        public static void PausePub()
        {

        }
        public static void ExtendPubHour(int pauseTime)
        {
            closeTime = closeTime.AddSeconds(pauseTime);
        }
    }
}
