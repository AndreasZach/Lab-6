using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
