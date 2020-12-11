using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    class Day11 : ASolution
    {
        Dictionary<(int, int), bool> Seats = new Dictionary<(int, int), bool>();
        readonly Dictionary<(int, int), bool> Seats2;
        private readonly List<(int, int)> Neighbors = new List<(int x, int y)>()
        {
            (1,0),
            (1,1),
            (0,1),
            (-1,1),
            (-1,0),
            (-1,-1),
            (0,-1),
            (1,-1)
        };
        readonly int maxX = 0;
        readonly int maxY = 0;

        public Day11() : base(11, 2020, "Seating System")
        {
            var lines = Input.SplitByNewline().ToList();
            maxY = lines.Count;
            for(int j = 0; j < lines.Count; j++)
            {
                for(int i = 0; i < lines[j].Length; i++)
                {
                    if (lines[j][i] == 'L') Seats[(i, j)] = false;
                    else if(lines[j][i] == '#') Seats[(i, j)] = false;
                    if (i > maxX) maxX = i;
                }
            }

            Seats2 = new Dictionary<(int, int), bool>(Seats);
        }

        protected override string SolvePartOne()
        {
            int seatsChanged = int.MaxValue;
            do
            {
                seatsChanged = 0;
                var nextSeats = new Dictionary<(int, int), bool>(Seats);
                foreach(var seat in Seats)
                {
                    bool nextVal = AliveNext(seat.Key);
                    if (nextVal != seat.Value) seatsChanged++;
                    nextSeats[seat.Key] = nextVal;
                }

                Seats = new Dictionary<(int, int), bool>(nextSeats);
            } while (seatsChanged != 0);

            return Seats.Count(x => x.Value).ToString();
        }

        protected override string SolvePartTwo()
        {
            Seats = new Dictionary<(int, int), bool>(Seats2);
            int seatsChanged = int.MaxValue;
            do
            {
                seatsChanged = 0;
                var nextSeats = new Dictionary<(int, int), bool>(Seats);
                foreach (var seat in Seats)
                {
                    bool nextVal = AliveNext(seat.Key, true);
                    if (nextVal != seat.Value) seatsChanged++;
                    nextSeats[seat.Key] = nextVal;
                }

                Seats = new Dictionary<(int, int), bool>(nextSeats);
            } while (seatsChanged != 0);

            return Seats.Count(x => x.Value).ToString();
        }

        private bool AliveNext((int x, int y) c, bool part2 = false)
        {
            int livingNeighbors = 0;
            List<(int, int)> locNeighbors = new List<(int x, int y)>();
            List<(int, int)> extendedNeighbors = new List<(int x, int y)>();

            foreach (var n in Neighbors)
            {
                locNeighbors.Add(c.Add(n));
                
                if (part2)
                {
                    var tmp = c.Add(n);
                    while (!Seats.ContainsKey(tmp))
                    {
                        if (tmp.Item1 < 0 || tmp.Item1 > maxX || tmp.Item2 < 0 || tmp.Item2 > maxY) break;
                        tmp = tmp.Add(n);
                    }
                    extendedNeighbors.Add(tmp);
                }
            }


            if (part2)
            {
                foreach (var n in extendedNeighbors)
                {
                    if (!Seats.ContainsKey(n)) continue;
                    if (Seats[n]) livingNeighbors++;
                }
                if (Seats[c])
                {
                    if (livingNeighbors < 5) return true;
                }
                else
                {
                    if (livingNeighbors == 0) return true;
                }
            } else
            {
                foreach (var n in locNeighbors)
                {
                    if (!Seats.ContainsKey(n)) continue;
                    if (Seats[n]) livingNeighbors++;
                }

                if (Seats[c])
                {
                    if (livingNeighbors < 4) return true;
                }
                else
                {
                    if (livingNeighbors == 0) return true;
                }
            }
                
            return false;
        }
    }
}