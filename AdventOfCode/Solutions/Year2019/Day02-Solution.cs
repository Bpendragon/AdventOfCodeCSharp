using AdventOfCode.UserClasses;

namespace AdventOfCode.Solutions.Year2019
{

    [DayInfo(02, 2019, "")]
    class Day02 : ASolution
    {
        private readonly IntCode2 pc;

        public Day02() : base()
        {
            pc = new IntCode2(Input.ToLongList(","));
        }

        protected override object SolvePartOne()
        {
            pc.Program[1] = 12;
            pc.Program[2] = 2;
            foreach (long _ in pc.RunProgram()) { }
            return pc.PreviousRunState[0];
        }

        protected override object SolvePartTwo()
        {
            for (long i = 0; i < 100; i++)
            {
                for (long j = 0; j < 100; j++)
                {
                    pc.Program[1] = i;
                    pc.Program[2] = j;
                    foreach (long _ in pc.RunProgram()) { };

                    if (pc.PreviousRunState[0] == 19690720) return ((100 * i) + j);
                }
            }


            return null;
        }
    }
}
