using AdventOfCode.UserClasses;

using System.Collections.Generic;


namespace AdventOfCode.Solutions.Year2016
{

    [DayInfo(12, 2016, "")]
    class Day12 : ASolution
    {
        readonly AssembunnyComputer ab;
        public Day12() : base()
        {
            ab = new AssembunnyComputer(new List<string>(Input.SplitByNewline()));
        }

        protected override object SolvePartOne()
        {
            ab.Execute();

            return ab.registers["a"];
        }

        protected override object SolvePartTwo()
        {
            ab.Reset();
            ab.registers["c"] = 1;
            ab.Execute();
            return ab.registers["a"];
        }
    }
}
