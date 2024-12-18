using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(03, 2024, "Mull It Over")]
    class Day03 : ASolution
    {
        IEnumerable<Match> muls;
        public Day03() : base()
        {
            muls = Regex.Matches(Input, "mul\\(\\d+,\\d+\\)");
        }

        protected override object SolvePartOne()
        {
            long sum = 0;
            foreach (Match m in muls)
            {
                var i = m.Value.ExtractLongs().ToArray();
                sum += i[0] * i[1];
            }
            return sum;
        }

        protected override object SolvePartTwo()
        {
            var enables = Regex.Matches(Input, "(do\\(\\)|don\\'t\\(\\))");

            long sum = 0;

            foreach (Match m in muls)
            {
                string lastInstruction = enables.LastOrDefault(a => a.Index < m.Index)?.Value ?? "do()";
                if (lastInstruction == "do()")
                {
                    var i = m.Value.ExtractLongs().ToArray();
                    sum += i[0] * i[1];
                }
            }
            return sum;
        }
    }
}
