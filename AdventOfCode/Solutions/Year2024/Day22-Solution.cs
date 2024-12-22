using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(22, 2024, "Monkey Market")]
    class Day22 : ASolution
    {
        List<SecretNumber> secretNumbers = new();
        DefaultDictionary<string, long> p2Groups = new();
        long p2Max = 0;
        public Day22() : base()
        {
            foreach(var n in Input.ExtractLongs())
            {
                secretNumbers.Add(new(n));
            }
        }

        protected override object SolvePartOne()
        {
            var sum = 0L;
            foreach(var n in secretNumbers)
            {
                long curVal = 0;
                foreach(int i in Enumerable.Range(1, 2000))
                {
                    (curVal, var k, var add) = n.GenerateNext(i);
                    if (add)
                    {
                        p2Groups[k] += curVal % 10;
                        if (p2Groups[k] > p2Max) p2Max = p2Groups[k];
                    }
                }
                sum += curVal;

            }
            return sum;
        }

        protected override object SolvePartTwo()
        {
            return p2Max;
        }

        private class SecretNumber
        {
            long curNumber = 0;
            long prevNumber = 0;
            string changes = string.Empty;
            private HashSet<string> seen = new();

            public SecretNumber(long seed)
            {
                curNumber = seed;
                prevNumber = seed;
            }

            public (long curVal, string changes, bool isFirstTime) GenerateNext(int counter)
            {
                var tmp = curNumber * 64;
                curNumber = curNumber ^ tmp;
                curNumber %= 16777216;
                tmp = curNumber / 32;
                curNumber = curNumber ^ tmp;
                curNumber %= 16777216;
                tmp = curNumber * 2048;
                curNumber = curNumber ^ tmp;
                curNumber %= 16777216;

                changes += ((curNumber % 10) - (prevNumber % 10));

                if (counter > 4 && changes[0] == '-') changes = changes[2..];
                else if (counter > 4) changes = changes[1..];

                prevNumber = curNumber;

                if(counter >= 4)
                {
                    return (curNumber, changes, seen.Add(changes));
                } else
                {
                    return (curNumber, string.Empty, false);
                }
            }
        }
    }
}
