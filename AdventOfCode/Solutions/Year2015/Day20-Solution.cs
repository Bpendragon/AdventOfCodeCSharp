using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2015
{

    [DayInfo(20, 2015, "")]
    class Day20 : ASolution
    {
        readonly long target;
        List<long> Houses;
        public Day20() : base()
        {
            target = long.Parse(Input);
        }

        protected override object SolvePartOne()
        {
            Houses = new List<long>(new long[target / 10]);
            for (int i = 1; i < target / 10; i++)
            {
                for (int j = i; j < target / 10; j += i)
                {
                    Houses[j] += i * 10;
                }
            }

            int k = 0;
            while (true)
            {
                if (Houses[k] >= target) return (k);
                k++;
            }
        }

        protected override object SolvePartTwo()
        {
            Houses = new List<long>(new long[target / 10]);
            for (int i = 1; i < target / 10; i++)
            {
                int visited = 0;
                for (int j = i; j < target / 10; j += i)
                {
                    Houses[j] += i * 11;
                    if (++visited == 50) break;
                }
            }

            int k = 0;
            while (true)
            {
                if (Houses[k] >= target) return (k);
                k++;
            }
        }
    }
}
