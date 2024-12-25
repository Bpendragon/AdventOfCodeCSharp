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
                List<int> t = new();
                foreach (var c in schema.SplitIntoColumns())
                {
                    t.Add(c.Count(a => a == '#') - 1);
                }

                if (schema[0] == '.') keys.Add(t); //It's a key
                else locks.Add(t); //It's a Lock
            }
        }

        protected override object SolvePartOne()
        {
            int count = 0;
            foreach( var c in locks.Combinations(keys))
            {
                if (c.First.Zip(c.Second).All(a => a.First + a.Second <= 5)) count++;
            }
            return count;
        }

        protected override object SolvePartTwo()
        {
            return "❄️🎄Happy Advent of Code🎄❄️";
        }
    }
}
