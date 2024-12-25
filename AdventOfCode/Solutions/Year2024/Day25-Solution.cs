using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(25, 2024, "Code Chronicle")]
    class Day25 : ASolution
    {
        List<List<int>> locks = new();
        List<List<int>> keys = new();
        public Day25() : base()
        {
            foreach(var schema in Input.SplitByDoubleNewline())
            {
                if (schema[0] == '.') //It's a key
                {
                    List<int> t = new();
                    foreach(var c in schema.SplitIntoColumns())
                    {
                        t.Add(c.Count(a => a == '#') - 1);
                    }
                    keys.Add(t);
                } else //it's a lock
                {
                    List<int> t = new();
                    foreach (var c in schema.SplitIntoColumns())
                    {
                        t.Add(c.Count(a => a == '#') - 1);
                    }
                    locks.Add(t);
                }
            }
        }

        protected override object SolvePartOne()
        {
            int count = 0;
            foreach(var k in keys)
            {
                foreach(var l in locks)
                {
                    if (l.Zip(k).All(a => a.First + a.Second < 6)) count++;
                }
            }
            return count;
        }

        protected override object SolvePartTwo()
        {
            return "❄️🎄Happy Advent of Code🎄❄️";
        }
    }
}
