using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(22, 2024, "Monkey Market")]
    class Day22 : ASolution
    {
        DefaultDictionary<string, long> p2Groups = new();

        public Day22() : base()
        {
            
        }

        protected override object SolvePartOne()
        {
            var sum = 0L;
            foreach(var n in Input.ExtractLongs())
            {
                long curNumber = n;
                string changes = string.Empty;
                HashSet<string> seen = new();
                long prevNumber = 0;
                foreach (int i in Enumerable.Range(1, 2000))
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
                    if (i > 4 && changes[0] == '-') changes = changes[2..];
                    else if (i > 4) changes = changes[1..];

                    prevNumber = curNumber;

                    if (i >= 4 && seen.Add(changes)) 
                    {
                        p2Groups[changes] += curNumber % 10;
                    }
                }
                sum += curNumber;

            }
            return sum;
        }

        protected override object SolvePartTwo()
        {
            return p2Groups.Max(a => a.Value);
        }
    }
}
