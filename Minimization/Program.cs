using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Minimization
{
    internal static class Program
    {
        private static char[] _variableNames = {'X', 'Y', 'Z', 'E', 'F'};

        private static IEnumerable<int[]> GenerateAllCombinations(int cnt, int max_cnt)
        {
            return cnt == 0 ? new []{new int[0]} : Enumerable.Range(0, max_cnt).SelectMany(
                name => GenerateAllCombinations(cnt - 1, max_cnt)
                    .Where(x => !x.Contains(name))
                    .Select(r => r
                        .Concat(new []{name})
                        .OrderBy(x => x)
                        .ToArray())
                );
        }

        private static IEnumerable<int[]> GenerateTitles(int cnt)
        {
            var r = Enumerable.Range(1, cnt).SelectMany(i => GenerateAllCombinations(i, cnt));
            var used = new LinkedList<int[]>();
            foreach (var a in r)
            {
                if (used.All(x => !x.SequenceEqual(a))) yield return a;
                used.AddLast(a);
            }
        }

        private static char GetVariable(char variable, bool state)
        {
            return state ? char.ToUpper(variable) : char.ToLower(variable);
        }

        private static IEnumerable<string> PrintInitialTable(int n, IReadOnlyList<int> values)
        {
            Console.Write("".PadRight(n + 1, ' '));
            var primitives = GenerateTitles(n).ToArray();
            Console.Write(string.Join(" ", primitives.Select(PrimitiveToString).ToList()));
            Console.WriteLine(" F");
            var deletedPrimitives = new List<string>();

            for (var i = 0; i < Math.Pow(2, n); ++i)
            {
                Console.Write(Convert.ToString(i, 2).PadLeft(n, '0')+" ");
                var stringPrimitives = primitives.Select(p => CheckPrimitive(i, p, n)).ToArray();
                Console.Write(string.Join(" ", stringPrimitives));
                if (values[i] == 0)
                {
                    deletedPrimitives.AddRange(stringPrimitives);
                }
                Console.WriteLine(" "+values[i]);
            }
            return deletedPrimitives.Distinct();
        }

        private static string CheckPrimitive(int i, IEnumerable<int> primitive, int n)
        {
            var bits = new BitArray(new[]{i});
            return primitive.Aggregate("", (current, p) => current + GetVariable(_variableNames[p], bits[n - p - 1]));
        }

        private static string PrimitiveToString(int[] primitive)
        {
            return string.Join("", primitive.Select(x => _variableNames[x]));
        }

        public static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            var values = Console.ReadLine().Select(x => int.Parse(x.ToString())).ToArray();
            _variableNames = _variableNames.Take(n).ToArray();
            Console.WriteLine("Initial state:");
            var deletedPrimitivies = PrintInitialTable(n, values).ToArray();
            Console.WriteLine("Deleted primitivies: ");
            Console.WriteLine(string.Join(", ", deletedPrimitivies));
            PrintTableWithDeletions(n, values, deletedPrimitivies);
        }

        private static IEnumerable<string[]> PrintTableWithDeletions(int n, IReadOnlyList<int> values, IEnumerable<string> deletedPrimitivies)
        {
            var deletedSet = new HashSet<string>(deletedPrimitivies);
            Console.Write("".PadRight(n + 1, ' '));
            var primitives = GenerateTitles(n).ToArray();
            Console.Write(string.Join(" ", primitives.Select(PrimitiveToString).ToList()));
            Console.WriteLine(" F");

            for (var i = 0; i < Math.Pow(2, n); ++i)
            {
                Console.Write(Convert.ToString(i, 2).PadLeft(n, '0')+" ");
                var stringPrimitives = primitives
                    .Select(p => CheckPrimitive(i, p, n))
                    .Select(x =>deletedSet.Contains(x) ? "".PadLeft(x.Length, ' ') : x);
                Console.Write(string.Join(" ", stringPrimitives));
                Console.WriteLine(" "+values[i]);
            }
        }
    }
}
