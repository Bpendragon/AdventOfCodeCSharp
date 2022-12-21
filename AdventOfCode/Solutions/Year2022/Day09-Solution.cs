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
using System.Xml.Linq;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(09, 2022, "Rope Bridge")]
    class Day09 : ASolution
    {
        readonly List<string> steps;
        public Day09() : base()
        {
            steps = Input.SplitByNewline();
        }

        protected override object SolvePartOne()
        {
            Coordinate2D head = (0, 0);
            Coordinate2D tail = (0, 0);
            Coordinate2D prevHead;
            HashSet<Coordinate2D> tailVisited = new() { tail };

            foreach (var step in steps)
            {
                CompassDirection travelDirection = (step[0]) switch
                {
                    'U' => CompassDirection.N,
                    'D' => CompassDirection.S,
                    'L' => CompassDirection.W,
                    'R' => CompassDirection.E,
                    _ => throw new ArgumentException("Invalid Direction")
                };
                int numSteps = int.Parse(step.Split(' ')[^1]);
                for (int i = 0; i < numSteps; i++)
                {
                    prevHead = head;
                    head = head.MoveDirection(travelDirection);
                    if (tail.ManDistance(head) == 2 && (tail.x == head.x || tail.y == head.y))
                    {
                        tail = tail.MoveDirection(travelDirection);
                    }
                    else if (tail.ManDistance(head) == 3)
                    {
                        tail = prevHead;
                    }
                    tailVisited.Add(tail);
                }
            }

            return tailVisited.Count;
        }

        protected override object SolvePartTwo()
        {
            LinkedList<Coordinate2D> rope = new();

            for (int i = 0; i < 10; i++) rope.AddLast((0, 0));

            HashSet<Coordinate2D> tailVisited = new() { rope.Last.Value };

            foreach (var step in steps)
            {
                CompassDirection travelDirection = (step[0]) switch
                {
                    'U' => CompassDirection.N,
                    'D' => CompassDirection.S,
                    'L' => CompassDirection.W,
                    'R' => CompassDirection.E,
                    _ => throw new ArgumentException("Invalid Direction")
                };
                int numSteps = int.Parse(step.Split(' ')[^1]);
                for (int i = 0; i < numSteps; i++)
                {
                    //Move head of rope
                    var curKnot = rope.First;
                    curKnot.Value = curKnot.Value.MoveDirection(travelDirection);
                    CompassDirection prevKnotDir = travelDirection;
                    //Rest of Rope follows
                    while (curKnot.Next != null)
                    {
                        var nextKnot = curKnot.Next;
                        if (nextKnot.Value.ManDistance(curKnot.Value) == 2 && (nextKnot.Value.x == curKnot.Value.x || nextKnot.Value.y == curKnot.Value.y))
                        {
                            nextKnot.Value = nextKnot.Value.Neighbors().FirstOrDefault(a => a.ManDistance(curKnot.Value) == 1);
                        }
                        else if (nextKnot.Value.ManDistance(curKnot.Value) >= 3)
                        {
                            //Dumb Homing, we know that we're at least a diagonal away, so move closer in x, then closer in y
                            if (curKnot.Value.x > nextKnot.Value.x) nextKnot.Value = nextKnot.Value.MoveDirection(CompassDirection.E);
                            else nextKnot.Value = nextKnot.Value.MoveDirection(CompassDirection.W);

                            if (curKnot.Value.y > nextKnot.Value.y) nextKnot.Value = nextKnot.Value.MoveDirection(CompassDirection.N);
                            else nextKnot.Value = nextKnot.Value.MoveDirection(CompassDirection.S);
                        }
                        curKnot = nextKnot;
                    }
                    tailVisited.Add(curKnot.Value);
                }
            }
            return tailVisited.Count;
        }
    }
}
