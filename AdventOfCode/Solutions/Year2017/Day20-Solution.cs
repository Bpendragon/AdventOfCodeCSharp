using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2017
{

    class Day20 : ASolution
    {
        readonly List<Particle> Particles = new List<Particle>();
        readonly List<Particle> Particles2 = new List<Particle>();
        public Day20() : base(20, 2017, "")
        {
            int i = 0;
            foreach (var line in Input.SplitByNewline())
            {
                var nums = line.Split("p=<>, va".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                Particles.Add(new Particle()
                {
                    ID = i,
                    x = long.Parse(nums[0]),
                    y = long.Parse(nums[1]),
                    z = long.Parse(nums[2]),
                    dX = long.Parse(nums[3]),
                    dY = long.Parse(nums[4]),
                    dZ = long.Parse(nums[5]),
                    aX = long.Parse(nums[6]),
                    aY = long.Parse(nums[7]),
                    aZ = long.Parse(nums[8])
                });
                Particles2.Add(new Particle() //I don't remember if a list created from a list of reference types also clones all items of the list
                {
                    ID = i,
                    x = long.Parse(nums[0]),
                    y = long.Parse(nums[1]),
                    z = long.Parse(nums[2]),
                    dX = long.Parse(nums[3]),
                    dY = long.Parse(nums[4]),
                    dZ = long.Parse(nums[5]),
                    aX = long.Parse(nums[6]),
                    aY = long.Parse(nums[7]),
                    aZ = long.Parse(nums[8])
                });
                i++;
            }
        }

        protected override string SolvePartOne()
        {
            while(Particles.Where(x => x.MovingAway).Count() < Particles.Count)
            {
                Particles.ForEach(p => p.Update());
                for (int i = 0; i < Particles.Count; i++)
                {
                    if (Particles[i].Destroyed) continue;
                    for (int j = i + 1; j < Particles.Count; j++)
                    {
                        if (Particles[j].Destroyed) continue;
                        if (Particles[i].CurPos == Particles[j].CurPos)
                        {
                            Particles[i].Destroyed = true;
                            Particles[j].Destroyed = true;
                        }
                    }
                }
            }

            foreach (int _ in Enumerable.Range(0, 3000)) //Everyone is moving away from the origin, let the quick ones take the slow ones
            {
                Particles.ForEach(p => p.Update());
                for (int i = 0; i < Particles.Count; i++)
                {
                    if (Particles[i].Destroyed) continue;
                    for (int j = i + 1; j < Particles.Count; j++)
                    {
                        if (Particles[j].Destroyed) continue;
                        if (Particles[i].CurPos == Particles[j].CurPos)
                        {
                            Particles[i].Destroyed = true;
                            Particles[j].Destroyed = true;
                        }
                    }
                }
            }

            Particles.Sort((a,b) => a.Distance.CompareTo(b.Distance));

            return Particles[0].ID.ToString();
        }

        protected override string SolvePartTwo()
        {

            return Particles.Where(p => !p.Destroyed).Count().ToString();
        }
    }

    class Particle
    {
        public long ID;

        public long x;
        public long y;
        public long z;

        public long dX;
        public long dY;
        public long dZ;

        public long aX;
        public long aY;
        public long aZ;

        public (long x, long y, long z) CurPos => (x, y, z);
        public (long x, long y, long z) CurVel => (dX, dY, dZ);
        public (long x, long y, long z) Accel => (aX, aY, aZ);
        public long Distance => GetDistance(CurPos);

        public (long x, long y, long z) lastPos = (100000, 100000, 100000);

        public bool MovingAway => (Distance > GetDistance(lastPos));
        public bool Destroyed = false;

        public static long GetDistance((long x, long y, long z) v)
        {
            return Math.Abs(v.x) + Math.Abs(v.y) + Math.Abs(v.z);
        }

        public void Update()
        {
            lastPos = CurPos;

            dX += aX;
            dY += aY;
            dZ += aZ;

            x += dX;
            y += dY;
            z += dZ;
        }

    }
}