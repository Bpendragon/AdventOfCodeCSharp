using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(13, 2024, "Claw Contraption")]
    class Day13 : ASolution
    {
        List<ClawMachine> machines = new();

        public Day13() : base()
        {
            foreach(var p in Input.SplitByDoubleNewline())
            {
                var nums = p.ExtractPosInts().ToArray();
                machines.Add(new()
                {
                    buttonA  = (nums[0], nums[1]),
                    buttonB  = (nums[2], nums[3]),
                    prizeLoc = (nums[4], nums[5])
                });
            }
        }

        protected override object SolvePartOne()
        {
            long sum = 0;

            foreach(var cm in machines)
            {
                (var a, var b) = cm.GetPressCounts(true);

                if (a == -1 || b == -1) continue;
                sum += (3 * a) + b;
            }

            //Left in for historical reasons and I wanted the pannenkoek joke to live.
            //foreach(var cm in machines)
            //{
            //    long aPresses = 0; //Since it's an int, I'm sorry Pannenkoek, it can't be a half
            //    long bPresses = 0;
            //    Coordinate2DL curLoc = (0, 0);
            //    (var tx, var ty) = cm.prizeLoc;
            //    for(int i = 0; i < 100; i++)
            //    {
            //        bPresses++;
            //        curLoc = curLoc + cm.buttonB;
            //        if((tx - curLoc.x) % cm.buttonA.x == 0 && (ty - curLoc.y) % cm.buttonA.y == 0)
            //        {
            //            if((tx - curLoc.x) / cm.buttonA.x == (ty - curLoc.y) / cm.buttonA.y)
            //            {
            //                aPresses = (tx - curLoc.x) / cm.buttonA.x;
            //                sum += (3 * aPresses) + bPresses;
            //                break;
            //            }
            //        }
            //    }
            //}
            return sum;
        }

        protected override object SolvePartTwo()
        {
            long sum = 0;

            foreach (var cm in machines)
            {
                (var a, var b) = cm.GetPressCounts(true);

                if (a == -1 || b == -1) continue;
                sum += (3 * a) + b;
            }
            return sum;
        }

        private class ClawMachine
        {
            public Coordinate2DL buttonA { get; set; } = new();
            public Coordinate2DL buttonB { get; set; } = new();
            public Coordinate2DL prizeLoc { get; set; } = new();
            public Coordinate2DL p2Prize => prizeLoc + (10000000000000, 10000000000000);


            public (long bA, long bB) GetPressCounts(bool part2 = false)
            {
                Coordinate2DL t = part2 ? p2Prize : prizeLoc;

                long resA = -1, resB = -1;

                if ((t.x * buttonB.y - buttonB.x * t.y) % (buttonA.x * buttonB.y - buttonB.x * buttonA.y) == 0) 
                {
                    resA = (t.x * buttonB.y - buttonB.x * t.y) / (buttonA.x * buttonB.y - buttonB.x * buttonA.y);
                }
                if((buttonA.x * t.y - t.x * buttonA.y) % (buttonA.x * buttonB.y - buttonB.x * buttonA.y) == 0)
                {
                    resB = (buttonA.x * t.y - t.x * buttonA.y) / (buttonA.x * buttonB.y - buttonB.x * buttonA.y);
                }
                return (resA, resB);

                //Left in for historical reasons, this was my first attempt at the system of equations, it worked on part 1 but must have had an arithmetic error in relation to part 2
                ////Intersection of two lines in the form ax+by+c = 0, imagining button A starts at (0,0) and buttonB starts at prizeLoc and works backwards
                //// A = dY
                //// B - dX
                //// C = y1dX-x1dY
                //// for button A: bA.X + bA.Y = 0
                //// for button B: -bB.X - bB.Y + ((-bB.X*prizeLoc.y) - (-bB.Y*prizeLoc.X))
                //// intersect occurs at (b1c2-b2c1)/(a1b2-a2b1) , (c1a2-c2a1)/(a1b2-a2b1)
                

                //long a1 = buttonA.y;
                //long b1 = -buttonA.x;
                //long c1 = 0L; //Because x1, y1 = 0,0 and thus the multiples fall out

                //long a2 = -buttonB.y;
                //long b2 = buttonB.x;
                //long c2 = (t.y * ((t.x - buttonB.x) - t.x)) - ((t.x * ((t.y - buttonB.y) - t.y)));

                //if((b2 * c1 - b1 * c2) % (a1*b2 - a2*b1) == 0 && (c2 * a1 -c1 * a2) % (a1*b2 - a2*b1) == 0)
                //{
                //    Coordinate2DL intercept = ((b1 * c2 - b2 * c1) / (a1 * b2 - a2 * b1), (c1 * a2 - c2 * a1) / (a1 * b2 - a2 * b1));
                //    long bA = intercept.x / buttonA.x;
                //    long bB = (t.x - intercept.x) / buttonB.x;

                //    return (bA, bB);
                //} 

                //return (-1, -1);
            }
        }
    }
}
