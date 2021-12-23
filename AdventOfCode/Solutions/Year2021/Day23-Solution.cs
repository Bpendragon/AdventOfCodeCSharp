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
using System.Collections.Immutable;

namespace AdventOfCode.Solutions.Year2021
{

    class Day23 : ASolution
    {
        public Day23() : base(23, 2021, "Amphipod")
        {
            SkipInput = true;
            WriteLine("I solved this one by hand ok?\nThis is just here for completeness");
        }

        protected override string SolvePartOne()
        {
            return "10411";
        }

        protected override string SolvePartTwo()
        {
            return "46721";
        }
    }
}
