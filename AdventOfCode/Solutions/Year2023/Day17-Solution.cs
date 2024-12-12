using System.Collections.Generic;
using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(17, 2023, "Clumsy Crucible")]
    class Day17 : ASolution
    {
        Dictionary<Coordinate2D, int> map;
        int maxX;
        int maxY;
        List<CompassDirection> checkDirs = new() { N, E, S, W };

        public Day17() : base()
        {
            (map, maxX, maxY) = Input.GenerateIntMap();
        }

        //(position, last_move_dir, run_length)
        protected override object SolvePartOne()
        {
            Dictionary<(Coordinate2D loc, CompassDirection heading, int runL), int> distances = new();
            Dictionary<(Coordinate2D loc, CompassDirection heading, int runL), (Coordinate2D loc, CompassDirection heading, int runL)> prev = new();

            PriorityQueue<(Coordinate2D loc, CompassDirection heading, int runL), int> Q = new();
            distances[((0, 0), E, 0)] = 0;
            Q.Enqueue(((0, 0), E, 0), 0);

            while(Q.TryDequeue(out var res, out int CostToGet))
            {
                (var loc, var heading, var runL) = res;
                if (loc == (maxX, maxY)) return CostToGet;
                foreach(var n in checkDirs) //I need to build a tuple generator.
                {
                    if (n == heading.Flip()) continue; //Disallow 180s
                    if (n == heading && runL == 3) continue; //Disallow long runs
                    var next = loc.Move(n);
                    if (!map.ContainsKey(next)) continue; //Bounds check

                    (Coordinate2D loc, CompassDirection heading, int runL) nextState = (next, n, n == heading ? runL + 1 : 1);
                    int cost = distances[res] + map[next];

                    if(cost < distances.GetValueOrDefault(nextState, int.MaxValue))
                    {
                        distances[nextState] = cost;
                        prev[nextState] = res;
                        Q.Enqueue(nextState, cost + next.ManDistance((maxX, maxY)));
                    }

                }
            }

            return null;
        }

        protected override object SolvePartTwo()
        {
            Dictionary<(Coordinate2D loc, CompassDirection heading, int runL), int> distances = new();
            Dictionary<(Coordinate2D loc, CompassDirection heading, int runL), (Coordinate2D loc, CompassDirection heading, int runL)> prev = new();

            PriorityQueue<(Coordinate2D loc, CompassDirection heading, int runL), int> Q = new();
            distances[((0, 0), E, 0)] = 0;
            Q.Enqueue(((0, 0), E, 0), 0); 
            distances[((0, 0), N, 0)] = 0;
            Q.Enqueue(((0, 0), N, 0), 0);

            while (Q.TryDequeue(out var res, out int CostToGet))
            {
                (var loc, var heading, var runL) = res;
                if (loc == (maxX, maxY)) return CostToGet;
                foreach (var n in checkDirs) //I need to build a tuple generator.
                {
                    if (n == heading.Flip()) continue; //Disallow 180s
                    if (n != heading && runL < 4) continue; //Disallow short runs
                    if (n == heading && runL == 10) continue; //Disallow long runs
                    var next = loc.Move(n);
                    if (!map.ContainsKey(next)) continue; //Bounds check

                    (Coordinate2D loc, CompassDirection heading, int runL) nextState = (next, n, n == heading ? runL + 1 : 1);
                    int cost = distances[res] + map[next];

                    if (cost < distances.GetValueOrDefault(nextState, int.MaxValue))
                    {
                        distances[nextState] = cost;
                        prev[nextState] = res;
                        Q.Enqueue(nextState, cost + next.ManDistance((maxX, maxY)));
                    }

                }
            }

            return null;
        }
    }
}
