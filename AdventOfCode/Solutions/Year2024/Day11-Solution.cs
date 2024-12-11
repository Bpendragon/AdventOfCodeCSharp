using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(11, 2024, "Plutonian Pebbles")]
    class Day11 : ASolution
    {
        DefaultDictionary<long, long> defaultDict= new();
        long p1 = 0;


        public Day11() : base()
        {
            foreach (var item in Input.ExtractLongs())
            {
                defaultDict[item]++;
            }
        }

        protected override object SolvePartOne()
        {
            for (int i = 1; i <= 75; i++)
            {
                DefaultDictionary<long, long> newData = new();
                foreach (var kvp in defaultDict)
                {
                    if (kvp.Key == 0) newData[1] += kvp.Value;
                    else if (kvp.Key.ToString().Length % 2 == 0)
                    {
                        var tmp = kvp.Key.ToString();
                        newData[int.Parse(tmp.Skip(tmp.Length / 2).JoinAsStrings())] += kvp.Value;
                        newData[int.Parse(tmp.Take(tmp.Length / 2).JoinAsStrings())] += kvp.Value;
                    }
                    else newData[kvp.Key * 2024] += kvp.Value;
                }
                defaultDict = newData;
                if (i == 25) p1 = defaultDict.Sum(a => a.Value);
            }

            return p1;
        }

        protected override object SolvePartTwo()
        {
            return defaultDict.Sum(a => a.Value);
        }
    }
}
