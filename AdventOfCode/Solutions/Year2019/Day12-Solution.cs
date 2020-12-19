using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2019
{

    class Day12 : ASolution
    {
        List<Moon> Moons = new List<Moon>();
        long? xCycle = null, 
              yCycle = null, 
              zCycle = null, 
              curSteps = 0;
        long part1Energy = 0;

        StringSplitOptions splitOps = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
        public Day12() : base(12, 2019, "")
        {
            foreach(var line in Input.SplitByNewline())
            {
                var nums = line.Split("<xyz=, >".ToCharArray(), splitOps);
                Moons.Add(new Moon(int.Parse(nums[0]), int.Parse(nums[1]), int.Parse(nums[2])));
            }

            while(true)
            {
                foreach(var moon in Moons)
                {
                    foreach(var other in Moons.Where(a => a != moon)) 
                    {
                        var deltaX = Math.Sign(other.location.x - moon.location.x);
                        var deltaY = Math.Sign(other.location.y - moon.location.y);
                        var deltaZ = Math.Sign(other.location.z - moon.location.z);

                        moon.velocity = moon.velocity.Add((deltaX, deltaY, deltaZ));
                    }
                }

                foreach (var moon in Moons) moon.location = moon.location.Add(moon.velocity);
                curSteps++;
                if(curSteps == 1000)
                {
                    part1Energy = Moons.Sum(a => a.TotalEnergy);
                }

                if (xCycle == null && Moons.All(a => a.velocity.x == 0)) xCycle = curSteps * 2; //first time we reach the zero velocity state we're only halfway through the cycle
                if (yCycle == null && Moons.All(a => a.velocity.y == 0)) yCycle = curSteps * 2;
                if (zCycle == null && Moons.All(a => a.velocity.z == 0)) zCycle = curSteps * 2;

                if (xCycle != null && yCycle != null && zCycle != null) break;
            }
        }

        protected override string SolvePartOne()
        {
            return part1Energy.ToString();
        }

        protected override string SolvePartTwo()
        {
            return (Utilities.FindLCM(Utilities.FindLCM((long)xCycle, (long)yCycle), (long)zCycle)).ToString();
        }

        private class Moon
        {
            public (long x, long y, long z) location;
            public (long x, long y, long z) velocity = (0,0,0);

            public Moon (int x, int y, int z)
            {
                location = new Coordinate3D(x, y, z);
            }

            public long PotentialEnergy => location.ManhattanMagnitude();
            public long KineticEnergy => velocity.ManhattanMagnitude();
            public long TotalEnergy => PotentialEnergy * KineticEnergy;
        }
    }
}