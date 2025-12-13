using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2015
{
    [DayInfo(18, 2015, "")]
    internal class Day18 : ASolution
    {
        Dictionary<Coordinate2D, char> BaseLights = new();
        int MaxX, MaxY;

        public Day18() : base()
        {
            (BaseLights, MaxX, MaxY) = Input.GenerateMap(false);
        }

        protected override object SolvePartOne()
        {
            Dictionary<Coordinate2D, char> modLights = new(BaseLights);

            for (int k = 0; k < 100; k++) //number of iterations
            {
                Dictionary<Coordinate2D, char> nextLights = new();

                foreach(var l in modLights)
                {
                    int cnt;
                    if (l.Value == '#') {
                        cnt = l.Key.Neighbors(true).Count(a => modLights.TryGetValue(a, out char c) && c == '#');
                        switch(cnt)
                        {
                            case 2:
                            case 3:
                                nextLights[l.Key] = '#';
                                break;
                            default:
                                nextLights[l.Key] = '.';
                                break;
                        }
                    } else
                    {
                        cnt = l.Key.Neighbors(true).Count(a => modLights.TryGetValue(a, out char c) && c == '#');
                        switch (cnt)
                        {
                            case 3:
                                nextLights[l.Key] = '#';
                                break;
                            default:
                                nextLights[l.Key] = '.';
                                break;
                        }
                    }
                }

                modLights = new(nextLights);

            }

            return modLights.Count();
        }

        protected override object SolvePartTwo()
        {

            return 0;
            //Dictionary<(int, int), char> modLights = new(BaseLights);

            //for (int k = 0; k < 100; k++) //number of iterations
            //{
            //    Dictionary<(int, int), char> nextLights = new(modLights);

            //    foreach ((int, int) i in modLights.Keys)
            //    {
            //        if (AliveNext(i, modLights, true))
            //        {
            //            nextLights[i] = '#';
            //        }
            //        else nextLights[i] = '.';
            //    }

            //    modLights = new Dictionary<(int, int), char>(nextLights);

            //}

            //Draw(modLights);
            //return modLights.Values.Count(x => x == '#');
        }
    }
}
