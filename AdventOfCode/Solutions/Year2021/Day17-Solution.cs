using System;

namespace AdventOfCode.Solutions.Year2021
{

    [DayInfo(17, 2021, "Trick Shot")]
    class Day17 : ASolution
    {
        //Direct from input
        readonly int minX = 137, maxX = 171, minY = -98, maxY = -73;
        readonly int smallestPossibleXVelo;
        public Day17() : base()
        {
            smallestPossibleXVelo = (int)Math.Ceiling(-1 + Math.Sqrt(1 + (8.0 * minX)) / 2);
        }

        protected override object SolvePartOne()
        {
            var tmp = Math.Abs(minY);
            var res = tmp * (tmp - 1) / 2;
            return res;
        }

        protected override object SolvePartTwo()
        {
            int succesfulShots = 0;
            for(int x = smallestPossibleXVelo; x < minX; x++)
            {
                for(int y = maxY; y < Math.Abs(minY); y++)
                {
                    if (SimulateShot((0, 0), x, y)) succesfulShots++;
                }
            }
            succesfulShots += (maxX - minX + 1) * (maxY - minY + 1);
            return succesfulShots;
        }

        private bool SimulateShot(Coordinate2D initialPos, int initX, int initY)
        {
            var curPos = initialPos;
            var velX = initX;
            var velY = initY;
            while(true)
            {
                curPos += (velX, velY);
                if (minX <= curPos.x && curPos.x <= maxX && minY <= curPos.y && curPos.y <= maxY) return true;
                if (curPos.y < minY || curPos.x > maxX) return false;

                if (velX > 0) velX--;
                velY--;
            }
        }
    }
}
