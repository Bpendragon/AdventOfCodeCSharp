using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2019
{

    class Day09 : ASolution
    {
        long[] program;
        private static StringBuilder part1 = new StringBuilder();
        public Day09() : base(09, 2019, "")
        {
            program = Input.ToLongArray(",");
        }

        protected override string SolvePartOne()
        {
            IntCodeComputer pcA = new IntCodeComputer(program);
            pcA.ProgramOutput += Pc_ProgramOutput;
            pcA.ProgramFinish += Pc_ProgramFinish;
            pcA.AddInput(1);
            pcA.ProccessProgram();
            return part1.ToString();
        }

        protected override string SolvePartTwo()
        {
            part1.Clear();
            IntCodeComputer pcA = new IntCodeComputer(program);
            pcA.ProgramOutput += Pc_ProgramOutput;
            pcA.ProgramFinish += Pc_ProgramFinish;
            pcA.AddInput(2);
            pcA.ProccessProgram();
            return part1.ToString();
        }

        private static void Pc_ProgramFinish(object sender, EventArgs e)
        {

        }

        private static void Pc_ProgramOutput(object sender, OutputEventArgs e)
        {
            part1.Append(e.OutputValue);
        }
    }
}