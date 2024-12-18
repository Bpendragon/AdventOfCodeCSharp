using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2017
{

    [DayInfo(02, 2017, "Corruption Checksum")]
    class Day02 : ASolution
    {
        List<List<int>> rows = new();
        public Day02() : base()
        {
            foreach (var r in Input.SplitByNewline())
            {
                rows.Add(new(r.ExtractInts()));
            }
        }

        protected override object SolvePartOne()
        {
            return rows.Sum(a => a.Max() - a.Min());
        }

        protected override object SolvePartTwo()
        {
            int sum = 0;
            foreach (var r in rows)
            {
                r.Sort((a, b) => b.CompareTo(a));
                for (int i = 0; i < r.Count; i++)
                {
                    int p = r.Skip(i).Take(1).First();
                    foreach (var q in r.Skip(i + 1))
                    {
                        if (p % q == 0)
                        {
                            sum += p / q;
                            i = int.MaxValue - 1;
                            break;
                        }
                    }
                }
            }
            return sum;
        }
    }
}
