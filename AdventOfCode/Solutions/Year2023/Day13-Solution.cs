using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(13, 2023, "Point of Incidence")]
    class Day13 : ASolution
    {
        List<string> maps = new();
        Dictionary<int, (int num, bool isRow)> initialMirrors = new();
        public Day13() : base()
        {
            maps = Input.SplitByDoubleNewline();
        }

        protected override object SolvePartOne()
        {
            int sum = 0;
            for (int i = 0; i < maps.Count; i++)
            {
                string block = maps[i];
                TryFindReflection(block, i, out int res);
                sum += res;
            }

            return sum;
        }

        protected override object SolvePartTwo()
        {
            int sum = 0;
            for (int j = 0; j < maps.Count; j++)
            {
                string block = maps[j];
                for (int i = 0; i < block.Length; i++)
                {
                    if (!".#".Contains(block[i])) continue;
                    StringBuilder sb = new StringBuilder(block);

                    sb[i] = sb[i] == '.' ? '#' : '.';
                    int res = 0;
                    if (TryFindReflection(sb.ToString(), j, out res))
                    {
                        sum += res;
                        break;
                    }

                }
            }

            return sum;
        }

        private bool TryFindReflection(string block, int Id, out int result)
        {
            var asRows = block.SplitByNewline();
            var asColumns = block.SplitIntoColumns().ToList();
            //Check rows
            for (int i = 1; i < asRows.Count; i++)
            {
                if (asRows.Take(i).Reverse().Zip(asRows.Skip(i)).All(x => x.First == x.Second))
                {
                    if (initialMirrors.TryGetValue(Id, out (int num, bool isRow) x))
                    {
                        if (x.isRow && x.num == i) continue;
                    }
                    initialMirrors[Id] = (i, true);
                    result = i * 100;
                    return true;
                }
            }

            for (int i = 1; i < asColumns.Count; i++)
            {
                if (asColumns.Take(i).Reverse().Zip(asColumns.Skip(i)).All(x => x.First == x.Second))
                {
                    if (initialMirrors.TryGetValue(Id, out (int num, bool isRow) x))
                    {
                        if (!x.isRow && x.num == i) continue;

                    }
                    initialMirrors[Id] = (i, false);
                    result = i;
                    return true;
                }
            }

            result = 0;
            return false;
        }
    }
}
