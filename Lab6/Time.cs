using System;

namespace Lab6
{
    public static class Time
    {
        private static DateTime openTime;
        private static DateTime closeTime = DateTime.Now;
        private static double oldSimulationSpeed = 1;
        private static double countdown;
        private static string timeStamp;
        private static double simulationSpeed;

        public static int SimulationTime { get; set; }

        public static void SetPubHours()
        {
            openTime = DateTime.Now;
            closeTime = openTime.AddSeconds(SimulationTime * simulationSpeed);
            countdown = SimulationTime;
        }

        public static void SetNewSimulationSpeed(double newSpeed)
        {
            simulationSpeed = newSpeed;
            closeTime = DateTime.Now.AddMilliseconds((closeTime.Subtract(DateTime.Now).TotalMilliseconds) / oldSimulationSpeed * simulationSpeed);
            oldSimulationSpeed = simulationSpeed;
        }

        public static double Countdown()
        {
            countdown = (closeTime.Subtract(DateTime.Now).TotalSeconds / simulationSpeed);
            return countdown;
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
