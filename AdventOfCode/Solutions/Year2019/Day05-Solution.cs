using System;
using System.Collections.Generic;

using AdventOfCode.UserClasses;

namespace AdventOfCode.Solutions.Year2019
{

    [DayInfo(05, 2019, "")]
    class Day05 : ASolution
    {
        readonly List<long> BaseProgram;
        readonly IntCode2 cpu;
        public Day05() : base()
        {
            BaseProgram = Input.ToLongList(",");
            cpu = new IntCode2(BaseProgram);
        }

        protected override object SolvePartOne()
        {
            long lastItem = long.MinValue;
            cpu.ReadyInput(1);
            foreach(long item in cpu.RunProgram())
            {
                Console.WriteLine(item);
                lastItem = item;
            }
            return lastItem;
        }

        protected override object SolvePartTwo()
        {
            Console.WriteLine("Begin Part 2");
            cpu.ClearInputs();
            cpu.ReadyInput(5);
            long lastItem = long.MinValue;
            foreach (long item in cpu.RunProgram())
            {
                Console.WriteLine(item);
                lastItem = item;
            }
            return lastItem;
        }
    }
}
