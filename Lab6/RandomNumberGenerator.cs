using System;

namespace Lab6
{
    public static class RandomNumberGenerator
    {
        private static Random randomGenerator = new Random();

        public static double GetRandomDouble(int from, int to)
        {
            double randomDouble = randomGenerator.Next(from, to);
            return randomDouble;
        }
    }
}
