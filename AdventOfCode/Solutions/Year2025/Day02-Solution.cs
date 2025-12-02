using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2025
{
    [DayInfo(02, 2025, "Gift Shop")]
    class Day02 : ASolution
    {
        long p1Res = 0;
        long p2Res = 0;

        public Day02() : base()
        {
            var groups = Input.ExtractPosLongs();
            var _enum = groups.GetEnumerator();
            while (_enum.MoveNext())
            {
                long s = _enum.Current;
                _enum.MoveNext();
                long e = _enum.Current;
                for (long i = s; i <= e; i++)
                {
                    var cur = i.ToString();
                    for (int j = cur.Length / 2; j >= 1; j--)
                    {
                        var chunks = cur.StringChunks(j);
                        if (chunks.All(x => x == chunks.First()))
                        {
                            if (j == cur.Length / 2 && cur.Length % 2 == 0)
                            {
                                p1Res += i;
                            }
                            else
                            {
                                p2Res += i;
                            }
                            break;
                        }
                    }
                }
            }
        }

        protected override object SolvePartOne()
        {
            return p1Res;
        }

        protected override object SolvePartTwo()
        {
            return p1Res + p2Res;
        }
    }
}
