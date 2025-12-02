using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2025
{
    [DayInfo(02, 2025, "Gift Shop")]
    class Day02 : ASolution
    {
        List<(long s, long e)> ranges = new(); 

        public Day02() : base()
        {
            var groups = Input.ExtractPosLongs().ToArray();
            for(int i = 0; i < groups.Length; i+=2)
            {
                ranges.Add((groups[i], groups[i + 1]));
            }
        }

        protected override object SolvePartOne()
        {
            long res = 0;
            foreach (var g in ranges)
            {
                for (long i = g.s; i <= g.e; i++)
                {
                    var cur = i.ToString();
                    if (cur.Length % 2 == 1) continue;
                    if (cur.Substring(0, cur.Length / 2) == cur.Substring(cur.Length / 2)) res += i;
                }
            }
            return res;
        }

        protected override object SolvePartTwo()
        {
            long res = 0;
            foreach (var g in ranges)
            {
                for (long i = g.s; i <= g.e; i++)
                {
                    var cur = i.ToString();
                    for (int j = 1; j <= cur.Length / 2; j++)
                    {
                        var chunks = cur.StringChunks(j);
                        if (chunks.All(x => x == chunks.First()))
                        {
                            res += i;
                            break;
                        }
                    }
                }
            }

            return res;
        }
    }
}
