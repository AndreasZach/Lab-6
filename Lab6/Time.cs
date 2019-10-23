using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    public static class Time
    {
        private static DateTime openTime;
        private static DateTime closeTime;
        private static string timeStamp;

        public static void SetPubHours(int seconds)
        {
            openTime = DateTime.Now;
            closeTime = openTime.AddSeconds(seconds);
        }

        public static string GetCountDown()
        {
            string countDown = (closeTime - DateTime.Now).ToString();
            return countDown;
        }
        public static string GetTimeStamp()
        {
            TimeSpan timeDiff = DateTime.Now - openTime;
            timeStamp = String.Format("{0:D2}.{1:D2}", timeDiff.Minutes, timeDiff.Seconds);
            return timeStamp;
        }
    }
}
