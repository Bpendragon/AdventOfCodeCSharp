using System.Collections.Generic;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2020
{

    [DayInfo(12, 2020, "Rain Risk")]
    class Day12 : ASolution
    {
        readonly List<string> Lines;
        public Day12() : base()
        {
            Lines = new List<string>(Input.SplitByNewline());

        }

        protected override object SolvePartOne()
        {
            (int x, int y) curPos = (0, 0);
            CompassDirection curDirection = E;

            foreach (var line in Lines)
            {
                char instruction = line[0];
                int val = int.Parse(line[1..]);
                switch (instruction)
                {
                    case 'N': curPos = curPos.Move(N, distance: val); break;
                    case 'S': curPos = curPos.Move(S, distance: val); break;
                    case 'E': curPos = curPos.Move(E, distance: val); break;
                    case 'W': curPos = curPos.Move(W, distance: val); break;
                    case 'F': curPos = curPos.Move(curDirection, distance: val); break;
                    case 'L': curDirection = (CompassDirection)(((int)curDirection - val + 360) % 360); break;
                    case 'R': curDirection = (CompassDirection)(((int)curDirection + val) % 360); break;
                }
            }
            return Utilities.ManhattanDistance((0, 0), curPos);
        }

        protected override object SolvePartTwo()
        {
            (int x, int y) shipPos = (0, 0);
            (int x, int y) wayPoint = (10, 1);

            foreach (var line in Lines)
            {
                char instruction = line[0];
                int val = int.Parse(line[1..]);
                switch (instruction)
                {
                    case 'N': wayPoint = wayPoint.Move(N, distance: val); break;
                    case 'S': wayPoint = wayPoint.Move(S, distance: val); break;
                    case 'E': wayPoint = wayPoint.Move(E, distance: val); break;
                    case 'W': wayPoint = wayPoint.Move(W, distance: val); break;
                    case 'F':
                        shipPos = shipPos.Add((val * wayPoint.x, val * wayPoint.y));
                        break;
                    case 'L':
                        switch (val)
                        {
                            case 90: wayPoint = (-wayPoint.y, wayPoint.x); break;
                            case 180: wayPoint = (-wayPoint.x, -wayPoint.y); break;
                            case 270: wayPoint = (wayPoint.y, -wayPoint.x); break;
                        }
                        break;
                    case 'R':
                        switch (val)
                        {
                            case 270: wayPoint = (-wayPoint.y, wayPoint.x); break;
                            case 180: wayPoint = (-wayPoint.x, -wayPoint.y); break;
                            case 90: wayPoint = (wayPoint.y, -wayPoint.x); break;
                        }
                        break;
                }
            }
            return Utilities.ManhattanDistance((0, 0), shipPos);
        }
    }
}
