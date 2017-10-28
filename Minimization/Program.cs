using System;
using System.Linq;

namespace Minimization
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            var values = Console.ReadLine().Select(x => int.Parse(x.ToString())).ToArray();

            for (int i = 0; i < Math.Pow(2, n); ++i)
            {
                Console.WriteLine(Convert.ToString(i, 2).PadLeft(n, '0') + " = " + values[i]);
            }
        }
    }
}
