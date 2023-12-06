using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(06, 2023, "Wait For It")]
    class Day06 : ASolution
    {
        List<(int time, int dist)> races;

        public Day06() : base()
        {
            var nums = Input.ExtractInts();
            var parts = nums.Split(nums.Count() / 2);

            races = parts.First().Zip(parts.Last()).ToList();
        }

        protected override object SolvePartOne()
        {
            int winMult = 1;
            //distance = timeHeld  * (totalTime - timeHeld)//is that a PARABOLA?
            foreach(var r in races)
            {
                int numWins = 0;
                for(int i = 0; i <= r.time; i++)
                {
                    int d = (r.time - i) * i;
                    if (d > r.dist) numWins++;
                }
                winMult *= numWins;
            }
            return winMult;
        }

        protected override object SolvePartTwo()
        {
            //ALL HAIL PARABOLAS

            // -tH^2 + tH*totalTime - R > 0 //find all integer values this holds true., which is the equivalent of finding the roots. 
            // Max is at totalTime/2 (negatives cancel), then just find positive root.

            long lTime = long.Parse(string.Concat(races.Select(r => r.time)));
            long lDist = long.Parse(string.Concat(races.Select(r => r.dist)));

            double midPoint = lTime / 2.0;
            double root = (-lTime + Math.Sqrt((lTime * lTime) - (4.0 * lDist))) / (-2.0);
            long firstWin = (long)Math.Ceiling(root);

            var t = 2 * (midPoint - firstWin);
            return lTime % 2 == 0 ? t + 1 : t;
        }
    }
}
