using System;

using AdventOfCode.UserClasses;

namespace AdventOfCode.Solutions.Year2019
{

    class Day09 : ASolution
    {
        readonly long[] program;
        public Day09() : base(09, 2019, "")
        {
            program = Input.ToLongArray(",");
        }

        protected override string SolvePartOne()
        {
            IntCode2 cpu = new(program);
            long lastItem = long.MinValue;
            cpu.ReadyInput(1);
            foreach (long item in cpu.RunProgram())
            {
                Console.WriteLine(item);
                lastItem = item;
            }
            return lastItem.ToString();
        }

        protected override string SolvePartTwo()
        {


            IntCode2 cpu = new(program);
            long lastItem = long.MinValue;
            cpu.ReadyInput(2);
            foreach (long item in cpu.RunProgram())
            {
                Console.WriteLine(item);
                lastItem = item;
            }
            return lastItem.ToString();
        }

    }
}