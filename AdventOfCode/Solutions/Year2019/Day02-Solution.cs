using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2019
{

    class Day02 : ASolution
    {
        private IntCode2 pc;

        public Day02() : base(02, 2019, "")
        {
            pc = new IntCode2(Input.ToLongArray(","));
        }

        protected override string SolvePartOne()
        {
            pc.Program[1] = 12;
            pc.Program[2] = 2;
            foreach (var _ in pc.RunProgram()) { }
            return pc.PreviousRunState[0].ToString();
        }

        protected override string SolvePartTwo()
        {
            for(long i = 0; i < 100; i++)
            {
                for(long j = 0; j < 100; j++)
                {
                    pc.Program[1] = i;
                    pc.Program[2] = j;
                    foreach (var _ in pc.RunProgram()){ };

                    if (pc.PreviousRunState[0] == 19690720) return ((100 * i) + j).ToString();
                }
            }


            return null;
        }
    }
}