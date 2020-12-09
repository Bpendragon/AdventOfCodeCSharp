using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using System.Text.RegularExpressions;


namespace AdventOfCode.Solutions.Year2016
{

    class Day12 : ASolution
    {
        readonly AssembunnyComputer ab;
        public Day12() : base(12, 2016, "")
        {
            ab = new AssembunnyComputer(new List<string>(Input.SplitByNewline()));
        }

        protected override string SolvePartOne()
        {
            ab.Execute();

            return ab.registers["a"].ToString();
        }

        protected override string SolvePartTwo()
        {
            ab.Reset();
            ab.registers["c"] = 1;
            ab.Execute();
            return ab.registers["a"].ToString();
        }
    }
}