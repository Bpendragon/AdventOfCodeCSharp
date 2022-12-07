using System;
using System.Collections.Generic;

using AdventOfCode.UserClasses;

namespace AdventOfCode.Solutions.Year2019
{

    [DayInfo(09, 2019, "")]
    class Day09 : ASolution
    {
        readonly List<long> program;
        public Day09() : base()
        {
            program = Input.ToLongList(",");
        }

        protected override object SolvePartOne()
        {
            IntCode2 cpu = new(program);
            long lastItem = long.MinValue;
            cpu.ReadyInput(1);
            foreach (long item in cpu.RunProgram())
            {
                Console.WriteLine(item);
                lastItem = item;
            }
            return lastItem;
        }

        protected override object SolvePartTwo()
        {


            IntCode2 cpu = new(program);
            long lastItem = long.MinValue;
            cpu.ReadyInput(2);
            foreach (long item in cpu.RunProgram())
            {
                Console.WriteLine(item);
                lastItem = item;
            }
            return lastItem;
        }

    }
}
