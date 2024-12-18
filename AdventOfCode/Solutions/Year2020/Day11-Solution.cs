using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    [DayInfo(11, 2020, "Seating System")]
    class Day11 : ASolution
    {
        Dictionary<(int, int), bool> Seats = new();
        readonly Dictionary<(int, int), bool> Seats2;
        private readonly List<(int, int)> Neighbors = new()
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

        public Day11() : base()
        {
            List<string> lines = Input.SplitByNewline().ToList();
            maxY = lines.Count;
            for (int j = 0; j < lines.Count; j++)
            {
                for (int i = 0; i < lines[j].Length; i++)
                {
                    if (lines[j][i] == 'L') Seats[(i, j)] = false;
                    else if (lines[j][i] == '#') Seats[(i, j)] = false;
                    if (i > maxX) maxX = i;
                }
            }

            Seats2 = new Dictionary<(int, int), bool>(Seats);
        }

        protected override object SolvePartOne()
        {
            int seatsChanged = int.MaxValue;
            do
            {
                seatsChanged = 0;
                Dictionary<(int, int), bool> nextSeats = new(Seats);
                foreach (KeyValuePair<(int, int), bool> seat in Seats)
                {
                    bool nextVal = AliveNext(seat.Key);
                    if (nextVal != seat.Value) seatsChanged++;
                    nextSeats[seat.Key] = nextVal;
                }

                Seats = new Dictionary<(int, int), bool>(nextSeats);
            } while (seatsChanged != 0);

            return Seats.Count(x => x.Value);
        }

        protected override object SolvePartTwo()
        {
            Seats = new Dictionary<(int, int), bool>(Seats2);
            int seatsChanged = int.MaxValue;
            do
            {
                seatsChanged = 0;
                Dictionary<(int, int), bool> nextSeats = new(Seats);
                foreach (KeyValuePair<(int, int), bool> seat in Seats)
                {
                    bool nextVal = AliveNext(seat.Key, true);
                    if (nextVal != seat.Value) seatsChanged++;
                    nextSeats[seat.Key] = nextVal;
                }

                Seats = new Dictionary<(int, int), bool>(nextSeats);
            } while (seatsChanged != 0);

            return Seats.Count(x => x.Value);
        }

        private bool AliveNext((int x, int y) c, bool part2 = false)
        {
            int livingNeighbors = 0;
            List<(int, int)> locNeighbors = new();
            List<(int, int)> extendedNeighbors = new();

            foreach ((int, int) n in Neighbors)
            {
                locNeighbors.Add(c.Add(n));

                if (part2)
                {
                    (int, int) tmp = c.Add(n);
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
                foreach ((int, int) n in extendedNeighbors)
                {
                    if (!Seats.ContainsKey(n)) continue;
                    if (Seats[n]) livingNeighbors++;
                }
                if (Seats[c])
                {
                    return livingNeighbors < 5;
                }
                else
                {
                    return livingNeighbors == 0;
                }
            }
            else
            {
                foreach ((int, int) n in locNeighbors)
                {
                    if (!Seats.ContainsKey(n)) continue;
                    if (Seats[n]) livingNeighbors++;
                }

                if (Seats[c])
                {
                    return (livingNeighbors < 4);
                }
                else
                {
                    return (livingNeighbors == 0);
                }
            }
        }
    }
}
