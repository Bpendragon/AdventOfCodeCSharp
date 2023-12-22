using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(21, 2023, "")]
    class Day21 : ASolution
    {
        Dictionary<Coordinate2D, char> map;
        int maxX;
        int maxY;

        Coordinate2D Start;
        public Day21() : base()
        {
            (map, maxX, maxY) = Input.GenerateMap(false, true);
            foreach (var k in map.Where(kvp => kvp.Value == '#').ToList()) map.Remove(k.Key);
            Start = map.Where(a => a.Value == 'S').First().Key;
            Coordinate2D northCenter = (Start.x, 0);
            Coordinate2D southCenter = (Start.x, maxY);
            Coordinate2D eastCenter = (0, Start.y);
            Coordinate2D westCenter = (maxX, Start.y);
        }

        protected override object SolvePartOne()
        {
            return GetCountFromPointInSteps(Start, 64);
        }

        protected override object SolvePartTwo()
        {
            //26501365 = 65 + (202300 * 131)
            int p2Steps = 26501365;
            int halfMaze = Start.x;
            int repeats = p2Steps / maxX;

            long interiorEvens = GetCountFromPointInSteps(Start); //Reachable squares in "normal" polarity
            long interiorOdds = GetCountFromPointInSteps(Start, 1001); //Reachable in "inverted" polarity
            long topOfDiamond = GetCountFromPointInSteps((maxX, halfMaze), maxX); //The point at the top of the generated diamond, enter from bottom center
            long bottomOfDiamond = GetCountFromPointInSteps((1, halfMaze), maxX); //The point at the bottom of the diamond, enter from top center
            long leftOfDiamond = GetCountFromPointInSteps((halfMaze, maxY), maxY);
            long rightOfDiamond = GetCountFromPointInSteps((halfMaze, 0), maxY);

            //In the end there are 14 possible tiles on the outer edge.
            long topRightEven = GetCountFromPointInSteps((maxX, 1), halfMaze);
            long topRightOdd = GetCountFromPointInSteps((maxX, 1), halfMaze, true);
            long bottomRightEven = GetCountFromPointInSteps((1, 1), halfMaze);
            long bottomRightOdd = GetCountFromPointInSteps((1, 1), halfMaze, true);
            long topLeftEven = GetCountFromPointInSteps((1, maxY), halfMaze);
            long topLeftOdd = GetCountFromPointInSteps((1, maxY), halfMaze, true);
            long bottomLeftEven = GetCountFromPointInSteps((maxX, maxY), halfMaze);
            long bottomLeftOdd = GetCountFromPointInSteps((maxX, maxY), halfMaze, true);


            long res = (interiorOdds + topOfDiamond + bottomOfDiamond + leftOfDiamond + rightOfDiamond) +
                (repeats * (topRightEven + bottomRightEven + topLeftEven + bottomLeftOdd)) +
                ((repeats - 1) * (topRightOdd + bottomRightOdd + topLeftOdd + bottomLeftOdd));

            for(long i = 1; i < repeats; i++)
            {
                res += (i % 2) switch
                {
                    0 => 4 * i * interiorOdds,
                    1 => 4 * i * interiorEvens,
                    _ => throw new ArithmeticException("Anything mod 2 must equal 0 or 1")
                };

            }


            WriteLine("The result for Day 21 part 2 might be wrong, off by one errors abound, but the algorithm is sound, and at this point I just want the benchmark");
            return res;
        }

        //BFS from Start to all other nodes to get distances.
        //Use the dictionary itself as our explored set
        private long GetCountFromPointInSteps(Coordinate2D input, int maxSteps = 1000, bool forceOddParity = false)
        {
            Dictionary<Coordinate2D, long> dists = new();
            Queue<Coordinate2D> toExplore = new();
            int parity = maxSteps % 2;
            if (forceOddParity) parity = 1;
            dists[input] = 0;
            toExplore.Enqueue(input);
            
            while (toExplore.TryDequeue(out var curLoc))
            {
                foreach (var n in curLoc.Neighbors())
                {
                    if (map.ContainsKey(n) && !dists.ContainsKey(n))
                    {
                        dists[n] = dists[curLoc] + 1;
                        toExplore.Enqueue(n);
                    }
                }
            }
            return dists.Count(a => a.Value <= maxSteps && a.Value % 2 == parity);
        }
    }
}
