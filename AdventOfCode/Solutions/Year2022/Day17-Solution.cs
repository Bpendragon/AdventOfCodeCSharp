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

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(17, 2022, "Pyroclastic Flow")]
    class Day17 : ASolution
    {
        public Day17() : base()
        {

        }

        protected override object SolvePartOne()
        {

            return Input.Count(a => a == '>') - Input.Count(a => a =='<');
        }

        protected override object SolvePartTwo()
        {
            return null;
        }
    }
}
