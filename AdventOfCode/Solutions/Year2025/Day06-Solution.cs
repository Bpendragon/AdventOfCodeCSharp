using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2025
{
    [DayInfo(06, 2025, "Trash Compactor")]
    class Day06 : ASolution
    {
        List<List<string>> rows = new();
        public Day06() : base()
        {
            foreach(var r in Input.SplitByNewline())
            {
                rows.Add(new());
                foreach(var s in r.Split(' ', StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries))
                {
                    rows[^1].Add(s);
                }
            }
        }

        protected override object SolvePartOne()
        {
            long res = 0;
            for(int i = 0; i < rows[0].Count(); i++)
            {
                long tmp;
                if (rows[^1][i] == "+")
                {
                    tmp = 0;
                    for(int j = 0; j < rows.Count - 1; j++)
                    {
                        tmp += long.Parse(rows[j][i]);
                    }
                } else
                {
                    tmp = 1;
                    for (int j = 0; j < rows.Count - 1; j++)
                    {
                        tmp *= long.Parse(rows[j][i]);
                    }
                }
                res += tmp;
            }
            return res;
        }

        protected override object SolvePartTwo()
        {
            var asCols = Input.SplitIntoColumns();

            long res = 0;
            long pAns = 0;

            bool isAdd = true;

            foreach(var c in asCols)
            {
                if (string.IsNullOrWhiteSpace(c)) continue;
                var tc = c.Trim();
                if (!char.IsDigit(tc[^1]))
                {
                    res += pAns;
                    if (tc[^1] == '+') isAdd = true;
                    else isAdd = false;

                    pAns = tc.ExtractPosLongs().First();
                    continue;
                }

                if (isAdd) pAns += long.Parse(tc);
                else pAns *= long.Parse(tc);
            }

            return res + pAns;
        }
    }
}
