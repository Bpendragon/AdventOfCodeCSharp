using Microsoft.VisualBasic;

using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode.Solutions.Year2015
{

    [DayInfo(05, 2015, "Doesn't He Have Intern-Elves For This?")]
    class Day05 : ASolution
    {
        List<string> n = new();
        const string vowels = "aeiou";
        readonly string[] p1Naughty = { "ab", "cd", "pq", "xy" };

        public Day05() : base()
        {
            n = Input.SplitByNewline();
        }

        protected override object SolvePartOne()
        {
            int count = 0;
            foreach(var s in n)
            {
                if (p1Naughty.Any(a => s.IndexOf(a) >= 0)) continue;
                if (vowels.Sum(a => s.AllIndexesOf(a).Count()) < 3) continue;
                if (s.SlidingWindow(2).Any(a => a.First() == a.Last())) count++;
            }
            return count;
        }

        protected override object SolvePartTwo()
        {
            int count = 0;

            foreach(var s in n)
            {
                if (s.SlidingWindow(3).Any(a => a.First() == a.Last()))
                {
                    foreach (var p in s.SlidingWindow(2))
                    {
                        var ps = p.JoinAsStrings();
                        var t = s.AllIndexesOf(ps);
                        if (t.Count() > 1 && t.Last() - t.First() > 1)
                        {
                            count++;
                            goto nextLine;
                        }
                    }
                }
            nextLine:;
            }

            return count;
        }
    }
}
