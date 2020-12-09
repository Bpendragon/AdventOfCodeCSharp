using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2019
{

    class Day05 : ASolution
    {
        readonly long[] BaseProgram;
        readonly IntCode2 cpu;
        public Day05() : base(05, 2019, "")
        {
            BaseProgram = Input.ToLongArray(",");
            cpu = new IntCode2(BaseProgram);
        }

        protected override string SolvePartOne()
        {
            long lastItem = long.MinValue;
            cpu.ReadyInput(1);
            foreach(var item in cpu.RunProgram())
            {
                Console.WriteLine(item);
                lastItem = item;
            }
            return lastItem.ToString();
        }

        protected override string SolvePartTwo()
        {
            Console.WriteLine("Begin Part 2");
            cpu.ClearInputs();
            cpu.ReadyInput(5);
            long lastItem = long.MinValue;
            foreach (var item in cpu.RunProgram())
            {
                Console.WriteLine(item);
                lastItem = item;
            }
            return lastItem.ToString();
        }
    }
}