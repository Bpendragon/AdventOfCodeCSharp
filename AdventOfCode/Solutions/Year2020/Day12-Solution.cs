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
        List<string> Lines;
        public Day12() : base(12, 2020, "")
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
                    case 'N': curPos = curPos.MoveDirection(val, CompassDirection.N); break;
                    case 'S': curPos = curPos.MoveDirection(val, CompassDirection.S); break;
                    case 'E': curPos = curPos.MoveDirection(val, CompassDirection.E); break;
                    case 'W': curPos = curPos.MoveDirection(val, CompassDirection.W); break;
                    case 'F': curPos = curPos.MoveDirection(val, curDirection); break;
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
                    case 'N': wayPoint = wayPoint.MoveDirection(val, CompassDirection.N); break;
                    case 'S': wayPoint = wayPoint.MoveDirection(val, CompassDirection.S); break;
                    case 'E': wayPoint = wayPoint.MoveDirection(val, CompassDirection.E); break;
                    case 'W': wayPoint = wayPoint.MoveDirection(val, CompassDirection.W); break;
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
            return null;
        }
    }
}