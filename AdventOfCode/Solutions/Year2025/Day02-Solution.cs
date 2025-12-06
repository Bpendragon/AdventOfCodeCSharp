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

        List<long> p1Matches = new();
        List<long> p2Matches = new();
        Dictionary<int, HashSet<long>> invalidIDsByLength= new();

        public Day02() : base()
        {
            // Pre-calculate all invalid IDs of lengths 1 - 10
            invalidIDsByLength[1] = new();
            invalidIDsByLength[2] = repeatDigit(2);
            invalidIDsByLength[3] = repeatDigit(3);
            invalidIDsByLength[4] = new(new MyRange(10, 100, false).Select(r => (r * 100) + r));
            invalidIDsByLength[5] = repeatDigit(5);
            invalidIDsByLength[6] = new((new MyRange(10, 100, false).Select(r => ((r * 10_000) + (r * 100) + r)))
                                            .Union(new MyRange(100, 1000, false).Select(r => (r * 1000) + r)));
            invalidIDsByLength[7] = repeatDigit(7);
            invalidIDsByLength[8] = new(new MyRange(1000, 10_000, false).Select(r => (r * 10_000) + r));
            invalidIDsByLength[9] = new(new MyRange(100, 1000, false).Select(r => ((long)((r * Math.Pow(10, 6)) + (r * Math.Pow(10, 3)) + r))));
            invalidIDsByLength[10] = new((new MyRange(10, 100, false).Select(r => ((long)((r * Math.Pow(10, 8)) + (r * Math.Pow(10, 6)) + (r * Math.Pow(10, 4)) + (r * Math.Pow(10, 2)) + r))))
                                            .Union(new MyRange(10_000, 100_000, false).Select(r => (long)((r * Math.Pow(10, 5)) + r))));


            var groups = Input.ExtractPosLongs();
            var _enum = groups.GetEnumerator();
            while (_enum.MoveNext())
            {
                long s = _enum.Current;
                _enum.MoveNext();
                long e = _enum.Current;

                HashSet<long> possibles = new(invalidIDsByLength[s.ToString().Length].Union(invalidIDsByLength[e.ToString().Length]));
                foreach(long i in new MyRange(s, e))
                {
                    if(possibles.Contains(i))
                    {
                        p2Res += i;
                        var l = i.ToString().Length;
                        long h = (long)Math.Pow(10, i.ToString().Length / 2);
                        if (i / h == i % h) p1Res += i;
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
            return p2Res;
        }

        private HashSet<long> repeatDigit(int n)
        {
            HashSet<long> res = new();

            foreach(long i in new MyRange(1,9))
            {
                long sum = 0;
                foreach(long j in new MyRange(0, n, false))
                {
                    sum += (long)(i * Math.Pow(10, j));
                }
                res.Add(sum);
            }

            return res;
        }
    }
}
