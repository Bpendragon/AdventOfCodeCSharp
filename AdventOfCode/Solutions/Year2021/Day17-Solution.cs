using System;
using System.Linq;

namespace AdventOfCode.Solutions.Year2021
{

    class Day17 : ASolution
    {
        //Direct from input
        readonly int minX = 137, maxX = 171, minY = -98, maxY = -73;
        int smallestPossibleXVelo;
        int largestPossibleYVelo = 0;
        public Day17() : base(17, 2021, "Trick Shot")
        {
            smallestPossibleXVelo = (int)(Math.Sqrt(1 + (8.0 * minX)) / 2) + 1;
        }

        protected override string SolvePartOne()
        {
            int bestY = 0;
            foreach (int y in Enumerable.Range(0, 100))
            {
                if (SimulateShot((0, 0), smallestPossibleXVelo, y, out int bY))
                {
                    if(bY > bestY) bestY = bY;
                    if (y > largestPossibleYVelo) largestPossibleYVelo = y;
                }
            }
            return bestY.ToString();
        }

        protected override string SolvePartTwo()
        {
            int succesfulShots = 0;
            for(int x = smallestPossibleXVelo; x <= maxX; x++)
            {
                for(int y = minY; y <= largestPossibleYVelo; y++)
                {
                    if (SimulateShot((0, 0), x, y, out int _)) succesfulShots++;
                }
            }
            return succesfulShots.ToString();
        }

        private bool SimulateShot(Coordinate2D initialPos, int initX, int initY, out int maxYAchieved)
        {
            var curPos = initialPos;
            var velX = initX;
            var velY = initY;
            maxYAchieved = 0;
            while(true)
            {
                curPos += (velX, velY);
                if (curPos.y > maxYAchieved) maxYAchieved = curPos.y;
                if (minX <= curPos.x && curPos.x <= maxX && minY <= curPos.y && curPos.y <= maxY) return true;
                if (curPos.y < minY || curPos.x > maxX) return false;

                if (velX > 0) velX--;
                velY--;
            }
        }
    }
}
