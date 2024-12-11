using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(11, 2024, "Plutonian Pebbles")]
    class Day11 : ASolution
    {
        List<long> initialVals;
        LinkedList<long> stones = new();
        DefaultDictionary<long, long> defaultDict= new();
        
        public Day11() : base()
        {
            initialVals = Input.ExtractLongs().ToList();

            foreach (var item in initialVals)
            {
                stones.AddLast(item);
                defaultDict[item]++;
            }
        }

        protected override object SolvePartOne()
        {
            for(int i = 0; i < 25; i++)
            {
                var curNode = stones.First;
                while(true)
                {
                    if (curNode.Value == 0) curNode.Value = 1;
                    else if (curNode.Value.ToString().Length % 2 == 0)
                    {
                        var tmp = curNode.Value.ToString();
                        curNode.Value = int.Parse(tmp.Skip(tmp.Length / 2).JoinAsStrings());
                        stones.AddBefore(curNode, int.Parse(tmp.Take(tmp.Length / 2).JoinAsStrings()));
                    }
                    else curNode.Value *= 2024;

                    if (curNode.Next is null) break;
                    curNode = curNode.Next;
                }
            }
            return stones.Count;
        }

        protected override object SolvePartTwo()
        {
            for(int i = 0; i< 75; i++)
            {
                DefaultDictionary<long, long> newData = new();
                foreach(var kvp in defaultDict)
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
            }
            return defaultDict.Sum(a => a.Value);
        }

    }
}
