using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2020
{

    class Day12 : ASolution
    {
        readonly List<string> Lines;
        public Day12() : base(12, 2020, "Rain Risk")
        {
            Lines = new List<string>(Input.SplitByNewline());

        }

        protected override string SolvePartOne()
        {
            (int x, int y) curPos = (0, 0);
            CompassDirection curDirection = CompassDirection.E;

            foreach(var line in Lines)
            {
                char instruction = line[0];
                int val = int.Parse(line[1..]);
                switch(instruction)
                {
                    case 'N': curPos = curPos.MoveDirection(CompassDirection.N, val); break;
                    case 'S': curPos = curPos.MoveDirection(CompassDirection.S, val); break;
                    case 'E': curPos = curPos.MoveDirection(CompassDirection.E, val); break;
                    case 'W': curPos = curPos.MoveDirection(CompassDirection.W, val); break;
                    case 'F': curPos = curPos.MoveDirection(curDirection, val); break;
                    case 'L': curDirection = (CompassDirection)(((int)curDirection - val + 360) % 360); break;
                    case 'R': curDirection = (CompassDirection)(((int)curDirection + val) % 360); break;
                }
            }
            return ManhattanDistance((0,0), curPos).ToString();
        }

        protected override string SolvePartTwo()
        {
            (int x, int y) shipPos = (0, 0);
            (int x, int y) wayPoint = (10, 1);

            foreach (var line in Lines)
            {
                char instruction = line[0];
                int val = int.Parse(line[1..]);
                switch (instruction)
                {
                    case 'N': wayPoint = wayPoint.MoveDirection(CompassDirection.N, val); break;
                    case 'S': wayPoint = wayPoint.MoveDirection(CompassDirection.S, val); break;
                    case 'E': wayPoint = wayPoint.MoveDirection(CompassDirection.E, val); break;
                    case 'W': wayPoint = wayPoint.MoveDirection(CompassDirection.W, val); break;
                    case 'F': 
                        shipPos = shipPos.Add((val * wayPoint.x, val * wayPoint.y));
                        break;
                    case 'L':
                        switch(val)
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
            return ManhattanDistance((0, 0), shipPos).ToString();
        }
    }
}