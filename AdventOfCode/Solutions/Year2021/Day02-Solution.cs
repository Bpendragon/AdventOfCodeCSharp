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

namespace AdventOfCode.Solutions.Year2021
{

    class Day02 : ASolution
    {
        readonly int x1 = 0;
        readonly int depth1 = 0;
        readonly int x2 = 0;
        readonly int depth2 = 0;
        readonly int aim = 0;

        public Day02() : base(02, 2021, "Dive!")
        {
            foreach (var c in Input.SplitByNewline())
            {
                var sp = c.Split();
                int dis = int.Parse(sp[1]);
                switch (sp[0])
                {
                    case "forward":
                        x1 += dis;
                        x2 += dis;
                        depth2 += (dis * aim);
                        break;
                    case "down":
                        depth1 += dis;
                        aim += dis;
                        break;
                    case "up":
                        depth1 -= dis;
                        aim -= dis;
                        break;
                }
            }

        }

        protected override string SolvePartOne()
        {
            return (depth1 * x1).ToString();
        }

        protected override string SolvePartTwo()
        {
            return (depth2 * x2).ToString();
        }
    }
}
