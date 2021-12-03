using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using System.Data;
using System.Threading;
using System.Security;
using static AdventOfCode.Solutions.Utilities;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Solutions.Year2021
{

    class Day03 : ASolution
    {
        List<string> readings = new();
        List<string> columns = new();
        Dictionary<int, char> mostCommon = new();
        public Day03() : base(03, 2021, "Binary Diagnostic")
        {
            readings = Input.SplitByNewline();
            columns = new(Input.SplitIntoColumns());
        }

        protected override string SolvePartOne()
        {
            for(int i = 0; i < columns.Count; i++)
            {
                mostCommon[i] = columns[i].GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;
            }
            StringBuilder gamma = new();
            StringBuilder epsilon = new();

            for (int i = 0; i < columns.Count; i++)
            {
                if (mostCommon[i] == '1')
                {
                    gamma.Append("1");
                    epsilon.Append("0");
                }
                else
                {
                    gamma.Append("0");
                    epsilon.Append("1");
                }
            }

            int g = Convert.ToInt32(gamma.ToString(), 2);
            int e = Convert.ToInt32(epsilon.ToString(), 2);

            return (g*e).ToString();
        }

        protected override string SolvePartTwo()
        {
            List<string> oxCandidates = readings.Where(x => x[0] == mostCommon[0]).ToList();
            List<string> coCandidates = readings.Where(x => x[0] != mostCommon[0]).ToList();
            int stringlength = oxCandidates[0].Length;
            for (int i = 1; i < stringlength; i++)
            {
                int onesCount = 0;
                int zeroesCount = 0;
                foreach(var c in oxCandidates)
                {
                    if (c[i] == '1') onesCount++;
                    else zeroesCount++;
                }

                if(onesCount >= zeroesCount)
                {
                    oxCandidates = oxCandidates.Where(x => x[i] == '1').ToList();
                } else
                {
                    oxCandidates = oxCandidates.Where(x => x[i] == '0').ToList();
                }
                if (oxCandidates.Count < 2) break;
            }

            for (int i = 1; i < stringlength; i++)
            {
                int onesCount = 0;
                int zeroesCount = 0;
                foreach (var c in coCandidates)
                {
                    if (c[i] == '1') onesCount++;
                    else zeroesCount++;
                }

                if (onesCount >= zeroesCount)
                {
                    coCandidates = coCandidates.Where(x => x[i] == '0').ToList();
                }
                else
                {
                    coCandidates = coCandidates.Where(x => x[i] == '1').ToList();
                }
                if (coCandidates.Count < 2) break;
            }
            return (Convert.ToInt32(oxCandidates[0],2) * Convert.ToInt32(coCandidates[0],2)).ToString();
        }
    }
}
