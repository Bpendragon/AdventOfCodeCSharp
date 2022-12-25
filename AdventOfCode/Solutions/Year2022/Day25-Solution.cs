using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(25, 2022, "")]
    class Day25 : ASolution
    {
        Dictionary<char, long> SnafuDigits = new()
        {
            { '0', 0 },
            { '1', 1 },
            { '2', 2 },
            { '-', -1 },
            { '=', -2 },
        };
        List<string> SnafusLE = new();
        public Day25() : base()
        {
            foreach(var l in Input.SplitByNewline())
            {
                SnafusLE.Add(l.Reverse());
            }
        }

        protected override object SolvePartOne()
        {
            long runningSum = 0;
            foreach(var s in SnafusLE)
            {
                long tmp = 0;
                long pwr = 1;
                foreach(char c in s)
                {
                    tmp += c switch
                    {
                        '1' => pwr,
                        '2' => 2 * pwr,
                        '0' => 0,
                        '-' => -pwr,
                        '=' => -2 * pwr
                    };

                    pwr *= 5;
                }

                runningSum += tmp;
            }

            Console.WriteLine(runningSum);

            return ToSnafu(runningSum);
        }

        protected override object SolvePartTwo()
        {
            return "❄️🎄Happy Advent of Code🎄❄️";
        }

        string ToSnafu(long num)
        {
            if (num == 0) return "";

            var remainder = num % 5;
            foreach(var kvp in SnafuDigits)
            {
                var (k, v) = kvp;
                if((v + 5) % 5 == remainder)
                {
                    long nxt = (num - v) / 5;
                    return ToSnafu(nxt) + k;
                }
            }

            return "";
        }
    }
}
