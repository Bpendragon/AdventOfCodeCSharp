using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(12, 2023, "Hot Springs")]
    class Day12 : ASolution
    {
        Dictionary<(string pattern, int size, string brokens), long> cache = new();
        public Day12() : base()
        {

        }

        protected override object SolvePartOne()
        {
            int sum = 0;
            foreach (var r in Input.SplitByNewline())
            {
                string states = r.Split()[0];
                List<int> badCounts = r.ExtractInts().ToList();
                int minSpaces = badCounts.Count + badCounts.Sum() - 1;
                int wiggleRoom = states.Length - minSpaces;

                List<List<int>> possibleCombos = new();
                List<int> currentPositions = new();

                int curPoint = 0;
                foreach (var c in badCounts)
                {
                    currentPositions.Add(curPoint);
                    curPoint += c + 1;
                }


                bool blockMoved = true; //turns this into a bit of a do-while
                while (blockMoved)
                {
                    if (IsPatternValid(states, badCounts, currentPositions)) 
                        possibleCombos.Add(new(currentPositions));


                    //Move outermost block along, if block cannot be moved, try next block in (and reset later blocks if needed), if no blocks can be moved, blockMoved = false;
                    blockMoved = false;

                    for (int i = 1; i <= currentPositions.Count && !blockMoved; i++)
                    {
                        //If we can move the last block, do it.
                        if (i == 1)
                        {
                            if (badCounts[^i] + currentPositions[^i] + 1 > states.Length) continue;
                            currentPositions[^i]++;
                            blockMoved = true;
                            continue;
                        }

                        if (badCounts[^i] + currentPositions[^i] + 2 > currentPositions[^(i - 1)]) continue;
                        blockMoved = true;
                        currentPositions[^i]++;
                        i--;
                        while (i > 0)
                        {
                            currentPositions[^i] = currentPositions[^(i + 1)] + badCounts[^(i + 1)] + 1;
                            i--;
                        }
                    }
                }

                sum += possibleCombos.Count;
            }

            return sum;
        }

        protected override object SolvePartTwo()
        {
            long sum = 0;

            foreach(var l in Input.SplitByNewline())
            {
                var halves = l.Split();
                string pattern = halves[0].Repeat(5, "?");
                var blocks = halves[1].Repeat(5, ",").ExtractInts().ToList();

                sum += CountMatches(pattern, pattern.Length, blocks);
            }

            return sum;
        }



        private long CountMatches(string pattern, int size, List<int> brokens)
        {
            string brokenString = brokens.JoinAsStrings(",");
            if (cache.TryGetValue((pattern, size, brokenString), out long cVal)) return cVal;

            if(brokens.Count == 0)
            {
                if(pattern.All(x => ".?".Contains(x))) //If there are no more brokens to be add, and all remaining parts of the pattern are not springs or unknown
                {
                    cache[(pattern, size, brokenString)] = 1;
                    return 1;
                } else //There are no more broken springs to add, but the pattern expects a broken spring
                {
                    cache[(pattern, size, brokenString)] = 0;
                    return 0;
                }
            }

            int nextSpringGroup = brokens[0];
            List<int> remainingSpringGroups = new();
            if(brokens.Count > 1)
            {
                remainingSpringGroups = brokens.Skip(1).ToList();
            }
            int remainingTiles = remainingSpringGroups.Count + remainingSpringGroups.Sum();

            long count = 0;

            int maxLead = size - remainingTiles - nextSpringGroup;

            for(int i = 0; i <= maxLead; i++ )
            {
                string nextGroup = new string('.', i) + new string('#', nextSpringGroup) + ".";
                if(nextGroup.Zip(pattern).All(a => a.First == a.Second || a.Second == '?'))
                {
                    if (nextGroup.Length > pattern.Length) continue;
                    count += CountMatches(pattern[nextGroup.Length..], size - nextSpringGroup - i - 1, remainingSpringGroups);
                }
            }

            cache[(pattern, size, brokenString)] = count;
            return count;

        }


        private bool IsPatternValid(string states, List<int> badCounts, List<int> currentPositions)
        {
            //Generate the pattern from teh positions
            StringBuilder sb = new();
            int posCounter = 0;
            sb.Append('.', currentPositions[0]);

            for (; posCounter < badCounts.Count; posCounter++)
            {
                sb.Append('#', badCounts[posCounter]);
                sb.Append('.', (posCounter + 1 < badCounts.Count ? currentPositions[posCounter + 1] : states.Length) - sb.Length);
            }

            var testLocs = sb.ToString();
            //Do the Comparison

            bool res = true;
            for (int i = 0; i < states.Length; i++)
            {
                switch (states[i])
                {
                    case '.':
                        if (testLocs[i] == '#') return false;
                        break;
                    case '#':
                        if (testLocs[i] == '.') return false;
                        break;
                    default: break;
                }
            }

            return res;
        }
    }
}
