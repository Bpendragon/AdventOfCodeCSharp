using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2018
{

    class Day15 : ASolution
    {
        private readonly Game game;
        readonly List<string> lines;
        public Day15() : base(15, 2018, "")
        {
            lines = Input.SplitByNewline();
            game = new Game(lines);
        }

        protected override object SolvePartOne()
        {
            return game.RunGame();
        }

        protected override object SolvePartTwo()
        {
            for(int elfAttackPower = 4; ; elfAttackPower++)
            {
                Game game = new(lines, elfAttackPower);

                int? res = game.RunGame(true);

                if (res != null) return res;
            }
        }

    }


    public class Game
    {
        private readonly string[] map;
        private List<Unit> units = new();
        public Game(List<string> initialMap, int elfAttackPower = 3)
        {
            for (int y = 0; y < initialMap.Count; y++)
            {
                for (int x = 0; x < initialMap[y].Length; x++)
                {
                    if (initialMap[y][x] == 'G')
                        units.Add(new Unit { X = x, Y = y, IsGoblin = true, Health = 200, AttackPower = 3 });
                    else if (initialMap[y][x] == 'E')
                        units.Add(new Unit { X = x, Y = y, IsGoblin = false, Health = 200, AttackPower = elfAttackPower });
                }
            }

            map = initialMap.Select(l => l.Replace('G', '.').Replace('E', '.')).ToArray();
        }

        // Returns outcome of game.
        public int? RunGame(bool failOnElfDeath = false)
        {
            for (int rounds = 0; ; rounds++)
            {
                //reorder the units based on where they've moved from last round
                units = units.OrderBy(u => u.Y).ThenBy(u => u.X).ToList();
                for (int i = 0; i < units.Count; i++) //don't use foreach because if unit dies it needs removed from list
                {
                    Unit u = units[i];
                    //get all possible targets for the attacking unit
                    List<Unit> targets = units.Where(t => t.IsGoblin != u.IsGoblin).ToList();
                    
                    //If no enemy, game is over
                    if (targets.Count == 0) return rounds * units.Sum(ru => ru.Health);

                    //Are we already next to a target? If so skip search/move phase
                    if (!targets.Any(t => IsAdjacent(u, t)))
                        TryMove(u, targets); //try to move otehrwise

                    Unit bestAdjacent =
                        targets
                        .Where(t => IsAdjacent(u, t)) //can only attack adjacent, must attack weakest, or reading order if tied
                        .OrderBy(t => t.Health)
                        .ThenBy(t => t.Y)
                        .ThenBy(t => t.X)
                        .FirstOrDefault();

                    if (bestAdjacent == null)
                        continue;

                    bestAdjacent.Health -= u.AttackPower;
                    if (bestAdjacent.Health > 0)
                        continue;

                    if (failOnElfDeath && !bestAdjacent.IsGoblin)
                        return null;

                    int index = units.IndexOf(bestAdjacent);
                    units.RemoveAt(index);
                    if (index < i)
                        i--;
                }
            }
        }

        private static readonly (int dx, int dy)[] s_neis = { (0, -1), (-1, 0), (1, 0), (0, 1) };
        private void TryMove(Unit u, List<Unit> targets)
        {
            HashSet<(int x, int y)> inRange = new();
            foreach (Unit target in targets)
            {
                foreach ((int dx, int dy) p in s_neis)
                {
                    var check = target.Position.Add(p);
                    if (IsOpen(check)) inRange.Add(check);
                }
            }

            Queue<(int x, int y)> queue = new();
            Dictionary<(int x, int y), (int px, int py)> prevs = new();
            queue.Enqueue((u.X, u.Y));
            prevs[(u.X, u.Y)] = (-1, -1);
            while (queue.Count > 0)
            {
                (int x, int y) = queue.Dequeue();
                foreach ((int dx, int dy) in s_neis)
                {
                    (int x, int y) nei = (x + dx, y + dy);
                    if (prevs.ContainsKey(nei) || !IsOpen(nei))
                        continue;

                    queue.Enqueue(nei);
                    prevs[nei] =  (x, y);
                }
            }

            List<(int x, int y)> getPath(int destX, int destY)
            {
                if (!prevs.ContainsKey((destX, destY)))
                    return null;
                List<(int x, int y)> path = new();
                (int x, int y) = (destX, destY);
                while (x != u.X || y != u.Y)
                {
                    path.Insert(0,(x, y));
                    (x, y) = prevs[(x, y)];
                }

                return path;
            }

            List<(int tx, int ty, List<(int x, int y)> path)> paths =
                inRange
                .Select(t => (t.x, t.y, path: getPath(t.x, t.y)))
                .Where(t => t.path != null)
                .OrderBy(t => t.path.Count)
                .ThenBy(t => t.y)
                .ThenBy(t => t.x)
                .ToList();

            List<(int x, int y)> bestPath = paths.FirstOrDefault().path;
            if (bestPath != null)
                (u.X, u.Y) = bestPath[0];
        }

        private bool IsOpen((int x, int y) loc) => map[loc.y][loc.x] == '.' && units.All(u => u.Position != loc);
        private static bool IsAdjacent(Unit u1, Unit u2) => Math.Abs(u1.X - u2.X) + Math.Abs(u1.Y - u2.Y) == 1;

        private class Unit
        {
            public int X, Y;
            public bool IsGoblin;
            public int Health = 200;
            public int AttackPower;
            public (int x, int y) Position => (X, Y);
        }
    }
}
