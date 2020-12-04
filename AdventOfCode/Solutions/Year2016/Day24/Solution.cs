using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2016
{

    class Day24 : ASolution
    {
        bool[][] maze;
        Dictionary<int, (int x, int y)> wires = new Dictionary<int, (int x, int y)>();
        public Day24() : base(24, 2016, "")
        {
            string[] lines = Input.SplitByNewline();
            maze = new bool[lines[0].Length][];
            foreach (int i in Enumerable.Range(0, maze.Length))
            {
                maze[i] = new bool[lines.Length];
            }

        }

        protected override string SolvePartOne()
        {
            return null;
        }

        protected override string SolvePartTwo()
        {
            return null;
        }
    }
}