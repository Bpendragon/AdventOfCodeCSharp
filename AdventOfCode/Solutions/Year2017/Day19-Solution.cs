using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2017
{

    class Day19 : ASolution
    {
        List<char> lettersSeen = new List<char>();
        int steps = 1;
        List<string> Lines;
        public Day19() : base(19, 2017, "")
        {
            Lines = new List<string>(Input.SplitByNewline());
        }

        protected override string SolvePartOne()
        {
            return lettersSeen.JoinAsStrings();
        }

        protected override string SolvePartTwo()
        {
            return null;
        }
    }
}