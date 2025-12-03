using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2025
{
    [DayInfo(03, 2025, "Lobby")]
    class Day03 : ASolution
    {
        List<string> battBanks = new();
        int offset = '0';

        public Day03() : base()
        {
            battBanks = Input.SplitByNewline();
        }

        protected override object SolvePartOne()
        {
            long res = 0;

            foreach (var battBank in battBanks)
            {
                res += GetMaxJoltage(battBank, 2);
            }

            return res;
        }

        protected override object SolvePartTwo()
        {
            long res = 0;

            foreach(var battBank in battBanks)
            {
                res += GetMaxJoltage(battBank, 12);
            }
            
            return res;
        }

        private long GetMaxJoltage(string battBank, int slots)
        {
            long total = 0;
            string bank = battBank;
            while (slots > 0)
            {
                string b = slots == 1 ? bank : bank.Substring(0, bank.Length - slots + 1);
                var c = b.Max();
                slots--;
                total = (total * 10) + c - offset;
                bank = bank.Substring(bank.IndexOf(c) + 1);
            }

            return total;
        }
    }
}
