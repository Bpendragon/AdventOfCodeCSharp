using System.Collections.Generic;

using AdventOfCode.UserClasses;


namespace AdventOfCode.Solutions.Year2016
{

    [DayInfo(23, 2016, "")]
    class Day23 : ASolution
    {
        readonly AssembunnyComputer ab;
        public Day23() : base()
        {
            ab = new AssembunnyComputer(new List<string>(Input.SplitByNewline()));
        }

        protected override object SolvePartOne()
        {
            ab.registers["a"] = 7;
            ab.Execute();

            return ab.registers["a"];
        }

        protected override object SolvePartTwo()
        {
            ab.Reset();

            ab.registers["a"] = 12; //runs stupidly long. 
            ab.Execute();

            return ab.registers["a"];
        }
    }
}
