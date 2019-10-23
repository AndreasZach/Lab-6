using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    public static class RandomIntGenerator
    {
        private static Random randomGenerator = new Random();

        public static int GetRandomInt(int from, int to)
        {
            int randomInt = randomGenerator.Next(from, to);
            return randomInt;
        }
    }
}
