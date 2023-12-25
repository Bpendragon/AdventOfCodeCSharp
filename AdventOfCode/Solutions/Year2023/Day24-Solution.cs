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
using System.Numerics;
using System.Net.Http.Headers;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(24, 2023, "Never Tell Me The Odds")]
    class Day24 : ASolution
    {
        List<Hailstone> Hailstones = new();

        const long min = 200000000000000L;
        const long max = 400000000000000L;

        public Day24() : base()
        {
            foreach(var l in Input.SplitByNewline())
            {
                var nums = l.ExtractLongs().ToList();

                Hailstones.Add(new(nums[0], nums[1], nums[2], nums[3], nums[4], nums[5]));
            }
        }

        protected override object SolvePartOne()
        {

            long valid = 0;
            foreach(var combo in Hailstones.Combinations(2)) {
                Hailstone h1 = combo.First();
                Hailstone h2 = combo.Last();

                (var x1, var y1, var z1) = (h1.X, h1.Y, h1.Z);
                (var x2, var y2, var z2) = (h1.p2.X, h1.p2.Y, h1.p2.Z);
                (var x3, var y3, var z3) = (h2.X, h2.p2.Y, h2.p2.Z) ;
                (var x4, var y4, var z4) = (h2.p2.X, h2.p2.Y, h2.p2.Z);

                var xNumerator = ((x1 * y2 - y1 * x2) * (x3 - x4)) - ((x1 - x2) * (x3 * y4 - y3 * x4));
                var xDenominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

                var yNumerator = ((x1 * y2 - y1 * x2) * (y3 - y4)) - ((y1 - y2) * (x3 * y4 - y3 * x4));
                var yDenominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

                if (xDenominator == 0 || yDenominator == 0) continue; //Lines are parralel
                else
                {
                    decimal xPos = (decimal)xNumerator / (decimal)xDenominator;
                    decimal yPos = (decimal)yNumerator / (decimal)yDenominator;

                    if (min <= xPos && xPos <= max && min <= yPos && yPos <= max)
                    {
                        if (h1.TimeAtXYPosition(xPos, yPos) < 0 || h2.TimeAtXYPosition(xPos, yPos) < 0 )
                        {
                            continue; //Occurred in someones past
                        }
                        else
                        {
                            valid++;
                        }
                    }
                }
            }
            return valid;
        }

        protected override object SolvePartTwo()
        {
            HashSet<long> possibleXV = null;
            HashSet<long> possibleYV = null;
            HashSet<long> possibleZV = null;

            foreach(var c in Hailstones.Combinations(2).Where(a => a.First().dX == a.Last().dX))
            {
                HashSet<long> tmp = new();
                long hailVelocity = (long)c.First().dX;
                long distanceDiff = (long)(c.Last().X - c.First().X);
                for(long i = -1000; i <= 1000; i++)
                {
                    try
                    {
                        if (distanceDiff % (i - hailVelocity) == 0) tmp.Add(i);
                    } catch (Exception e) { }
                }


                if(possibleXV == null)
                {
                    possibleXV = new(tmp);
                } else
                {
                    possibleXV = possibleXV.Intersect(tmp).ToHashSet<long>();
                }

                if (possibleXV.Count == 1) break;
            }

            foreach (var c in Hailstones.Combinations(2).Where(a => a.First().dY == a.Last().dY))
            {
                HashSet<long> tmp = new();
                long hailVelocity = (long)c.First().dY;
                long distanceDiff = (long)(c.Last().Y - c.First().Y);
                for (long i = -1000; i <= 1000; i++)
                {
                    try
                    {
                        if (distanceDiff % (i - hailVelocity) == 0) tmp.Add(i);
                    }
                    catch (Exception e) { }
                }


                if (possibleYV == null)
                {
                    possibleYV = new(tmp);
                }
                else
                {
                    possibleYV = possibleYV.Intersect(tmp).ToHashSet<long>();
                }

                if (possibleYV.Count == 1) break;
            }

            foreach (var c in Hailstones.Combinations(2).Where(a => a.First().dZ == a.Last().dZ))
            {
                HashSet<long> tmp = new();
                long hailVelocity = (long)c.First().dZ;
                long distanceDiff = (long)(c.Last().Z - c.First().Z);
                for (long i = -1000; i <= 1000; i++)
                {
                    try
                    {
                        if (distanceDiff % (i - hailVelocity) == 0) tmp.Add(i);
                    }
                    catch (Exception e) { }
                }


                if (possibleZV == null)
                {
                    possibleZV = new(tmp);
                }
                else
                {
                    possibleZV = possibleZV.Intersect(tmp).ToHashSet<long>();
                }

                if (possibleZV.Count == 1) break;
            }

            long dXi = possibleXV.Single();
            long dYi = possibleYV.Single();
            long dZi = possibleZV.Single();

            Hailstone h1 = Hailstones.First();
            Hailstone h2 = Hailstones.Skip(10).Take(1).Single();

            h1.dX = h1.dX - dXi;
            h2.dX = h2.dX - dXi;
            h1.dY = h1.dY - dXi;
            h2.dY = h2.dY - dXi;
            h1.dZ = h1.dZ - dXi;
            h2.dZ = h2.dZ - dXi;


            var slopA = (decimal)h1.dY / (decimal)h1.dX;
            var slopeB = (decimal)h2.dY / (decimal)h2.dX;

            var intersectA = (decimal)h1.Y - (slopA * (decimal)h1.X);
            var interesectB = (decimal)h2.Y - (slopeB * (decimal)h2.X);

            long xPos = ((long)((interesectB - intersectA) / (slopA - slopeB)));
            long yPos = (long)(slopA * xPos + intersectA);
            long time = (long)((xPos - h1.X) / h1.dX);

            long zPos = (long)(h1.Z + (dZi * time));

            Console.WriteLine($"{{{dXi}}}, {{{dYi}}}, {{{dZi}}}");
            Console.WriteLine($"{{{xPos}}}, {{{yPos}}}, {{{zPos}}}");


            return xPos+yPos+zPos;
        }

        private class Hailstone
        {
            public BigInteger X;
            public BigInteger Y;
            public BigInteger Z;

            public BigInteger dX;
            public BigInteger dY;
            public BigInteger dZ;

            public Hailstone(long x, long y, long z, long dX, long dY, long dZ)
            {
                X = x;
                Y = y;
                Z = z;
                this.dX = dX;
                this.dY = dY;
                this.dZ = dZ;
            }

            public (BigInteger X, BigInteger Y, BigInteger Z) PositionAtTime(long t) => (this.X + (t * dX), this.Y + (t * dY), this.Z + (t * dZ));
            public (BigInteger X, BigInteger Y, BigInteger Z) p2 => (X + dX, Y + dY, Z + dZ);
            public decimal TimeAtXYPosition(decimal x, decimal y)
            {
                if (dX != 0)
                {
                    return (x - (decimal)X) / (decimal)dX;
                }

                return (y - (decimal)Y) / (decimal)dY;
            }
        }
    }
}
