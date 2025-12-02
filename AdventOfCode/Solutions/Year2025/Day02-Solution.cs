using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using System.Data;
using System.Threading;
using System.Security;
using static AdventOfCode.Solutions.Utilities;
using System.Runtime.CompilerServices;

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
            List<long> invalidIDs = new();
            foreach (var g in ranges)
            {
                for (long i = g.s; i <= g.e; i++)
                {
                    var cur = i.ToString();
                    if (cur.Substring(0, cur.Length / 2) == cur.Substring(cur.Length / 2)) invalidIDs.Add(i);
                }
            }
            return invalidIDs.Sum();
        }

        protected override object SolvePartTwo()
        {
            List<long> invalidIDs = new();
            foreach (var g in ranges)
            {
                for (long i = g.s; i <= g.e; i++)
                {
                    var cur = i.ToString();
                    for (int j = 1; j <= cur.Length / 2; j++)
                    {
                        var chunks = cur.StringChunks(j);
                        if (chunks.Distinct().Count() == 1)
                        {
                            invalidIDs.Add(i);
                            break;
                        }
                    }
                }
            }

            return invalidIDs.Sum();
        }
    }
}
