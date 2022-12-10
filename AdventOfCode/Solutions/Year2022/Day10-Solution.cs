using System;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(10, 2022, "Cathode-Ray Tube")]
    class Day10 : ASolution
    {
        readonly int part1;
        readonly string part2;
        public Day10() : base()
        {
            var instructions = Input.SplitByNewline();

            StringBuilder sb = new();
            int x = 1;
            int cycles = 0;
            int signalSum = 0;

            foreach (var ins in instructions)
            {
                var parts = ins.Split(' ');
                int xAtStart = x;
                if (parts.Length == 2)
                {
                    x += int.Parse(parts[^1]);
                }

                for (int i = 0; i < parts.Length; i++)
                {
                    if (new int[] { xAtStart, xAtStart + 1, xAtStart - 1 }.Contains(cycles % 40)) sb.Append('█');
                    else sb.Append(' ');
                    cycles++;
                    if (cycles % 40 == 0) sb.Append('\n');
                    if ((cycles - 20) % 40 == 0) signalSum += cycles * xAtStart;
                }
            }

            part1 = signalSum;
            part2 = sb.ToString();

        }

        protected override object SolvePartOne()
        {
            return part1;
        }

        protected override object SolvePartTwo()
        {
            return part2;
        }
    }
}
