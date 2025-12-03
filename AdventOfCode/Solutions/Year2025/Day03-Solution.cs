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
using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode.Solutions.Year2025
{
    [DayInfo(03, 2025, "Lobby")]
    class Day03 : ASolution
    {
        List<string> battBanks = new();

        public Day03() : base()
        {
            battBanks = Input.SplitByNewline();
        }

        protected override object SolvePartOne()
        {
            var res = 0;

            foreach(var b in battBanks)
            {
                for(char i = '9'; i > '0'; i--)
                {
                    var locs = b.AllIndexesOf(i);
                    if (locs.Count() >= 1)
                    {
                        if (locs.First() == b.Length - 1) continue;
                        for (char j = '9'; j > '0'; j--)
                        {
                            var locs2 = b.AllIndexesOf(j);
                            if ((i != j && locs2.Any(k => k > locs.First())) || (i == j && locs.Count() > 1))
                            {
                                res += int.Parse($"{i}{j}");
                                break;
                            } 
                        }
                        break;
                    }
                }
            }

            return res;
        }

        protected override object SolvePartTwo()
        {
            long res = 0;

            foreach(var battBank in battBanks)
            {
                var slots = 12;
                long total = 0;
                string bank = battBank;
                while (slots > 0)
                {
                    string b = slots == 1 ? bank : bank.Substring(0, bank.Length - slots + 1);
                    var c = b.Max();
                    slots--;
                    total = (total * 10) + c - 48;
                    bank = bank.Substring(bank.IndexOf(c) + 1);
                }
                res += total;
            }

            return res;
        }
    }
}
