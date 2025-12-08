using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2025
{
    [DayInfo(06, 2025, "Trash Compactor")]
    class Day06 : ASolution
    {
        List<List<long>> rows = new();
        List<string> operators = new();
        string[] asCols;

        public Day06() : base()
        {
            var lines = Input.SplitByNewline();
            foreach(var r in lines.SkipLast(1))
            {
                rows.Add(r.ExtractPosLongs().ToList());
            }

            operators = lines.Last().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

            asCols = Input.SplitIntoColumns();
        }

        protected override object SolvePartOne()
        {
            long res = 0;
            for(int i = 0; i < rows[0].Count(); i++)
            {
                long tmp;
                if (operators[i] == "+")
                {
                    tmp = 0;
                    for(int j = 0; j < rows.Count; j++)
                    {
                        tmp += rows[j][i];
                    }
                } else
                {
                    tmp = 1;
                    for (int j = 0; j < rows.Count; j++)
                    {
                        tmp *= rows[j][i];
                    }
                }
                res += tmp;
            }
            return res;
        }

        protected override object SolvePartTwo()
        {
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
