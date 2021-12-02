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
        List<string> commands;
        public Day02() : base(02, 2021, "Dive!")
        {
            commands = Input.SplitByNewline();
        }

        protected override string SolvePartOne()
        {
            int x = 0;
            int depth = 0;
            foreach(var c in commands)
            {
                var sp = c.Split();
                int dis = int.Parse(sp[1]);
                switch(sp[0])
                {
                    case "forward": x += dis;
                        break;
                    case "down": depth += dis;
                        break;
                    case "up": depth -= dis;
                        break;
                }
            }
            return (depth * x).ToString();
        }

        protected override string SolvePartTwo()
        {
            int x = 0;
            int depth = 0;
            int aim = 0;
            foreach (var c in commands)
            {
                var sp = c.Split();
                int dis = int.Parse(sp[1]);
                switch (sp[0])
                {
                    case "forward":
                        x += dis;
                        depth += (dis * aim);
                        break;
                    case "down":
                        aim += dis;
                        break;
                    case "up":
                        aim -= dis;
                        break;
                }
            }
            return (depth * x).ToString();
        }
    }
}
