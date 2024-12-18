using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(02, 2022, "Rock Paper Scissors")]
    class Day02 : ASolution
    {
        readonly List<string> rounds;
        public Day02() : base()
        {
            rounds = Input.SplitByNewline();
        }

        protected override object SolvePartOne()
        {
            long score = 0;
            foreach (var round in rounds)
            {
                switch (round[0])
                {
                    case 'A':
                        if (round[^1] == 'X') score += 4;
                        else if (round[^1] == 'Y') score += 8;
                        else score += 3;
                        break;
                    case 'B':
                        if (round[^1] == 'X') score += 1;
                        else if (round[^1] == 'Y') score += 5;
                        else score += 9;
                        break;
                    case 'C':
                        if (round[^1] == 'X') score += 7;
                        else if (round[^1] == 'Y') score += 2;
                        else score += 6;
                        break;
                }
            }
            return score;
        }

        protected override object SolvePartTwo()
        {
            long score = 0;
            foreach (var round in rounds)
            {
                switch (round[0])
                {
                    case 'A':
                        if (round[^1] == 'X') score += 3;
                        else if (round[^1] == 'Y') score += 4;
                        else score += 8;
                        break;
                    case 'B':
                        if (round[^1] == 'X') score += 1;
                        else if (round[^1] == 'Y') score += 5;
                        else score += 9;
                        break;
                    case 'C':
                        if (round[^1] == 'X') score += 2;
                        else if (round[^1] == 'Y') score += 6;
                        else score += 7;
                        break;
                }
            }
            return score;
        }
    }
}
