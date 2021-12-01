using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2019
{

    class Day15 : ASolution
    {
        readonly IntCode2 bot;
        readonly Dictionary<(int x, int y), int> map = new();
        (int x, int y) botLocation;
        (int x, int y) oxyGenLocation;
        readonly Random rand = new(); //gonna just random walk this;
        public Day15() : base(15, 2019, "")
        {

            bot = new IntCode2(Input.ToLongList(","));
            map[(0, 0)] = 1;
            botLocation = (0, 0);
            bot.ClearInputs();
            bot.ReadyInput(1);
            long stepsTaken = 0;
            int lastInput = 1;
            foreach (var output in bot.RunProgram())
            {
                if (stepsTaken >= 2_000_000) break;
                UpdateMap(output, lastInput);
                lastInput = rand.Next(1, 5);

                bot.ReadyInput(lastInput);
                stepsTaken++;
            }

            DrawMap();
        }


        protected override string SolvePartOne()
        {        
          return (AStar((0, 0), oxyGenLocation, map).Count() - 1).ToString(); //-1 to account for the facct the path contains the start node, which is not a step, see also: fencepost problem
        }

       

        protected override string SolvePartTwo()
        {
            return map.Where(a => a.Value != 0).Max(a => AStar(oxyGenLocation, a.Key, map).Count() - 1).ToString();
        }

        readonly List<(int dX, int dY)> movementDirs = new() { (0, 1), (0, -1), (1, 0), (-1, 0) };
        //A-Star, heuristic is Manhattan
        private List<(int x, int y)> AStar((int, int) start, (int x, int y) goal, Dictionary<(int x, int y), int> map)
        {
            List<(int x, int y)> openSet = new();
            Dictionary<(int x, int y), (int x, int y)> cameFrom = new();
            Dictionary<(int x, int y), int> gScore = new();
            Dictionary<(int x, int y), int> fScore = new();

            openSet.Add(start);
            gScore[start] = 0;
            fScore[start] = 0;

            while(openSet.Count >0)
            {
                openSet.OrderBy(x => fScore.GetValueOrDefault(x, int.MaxValue));
                var current = openSet[0];
                if (current == goal) return reconstructPath(cameFrom, current);
                openSet.Remove(current);
                foreach(var dir in movementDirs)
                {
                    var tmp = current.Add(dir);
                    if(map.TryGetValue(tmp, out var val))
                    {
                        if (val == 0) continue; //this is a wall, skip it.
                        var tentGscore = gScore[current] + 1;
                        if(tentGscore < gScore.GetValueOrDefault(tmp, int.MaxValue))
                        {
                            cameFrom[tmp] = current;
                            gScore[tmp] = tentGscore;
                            fScore[tmp] = gScore[tmp] + tmp.ManhattanDistance(goal);
                            if (!openSet.Contains(tmp)) openSet.Add(tmp);
                        }
                    }
                }
            }

            return null;
        }

        private List<(int x, int y)> reconstructPath(Dictionary<(int x, int y), (int x, int y)> cameFrom, (int x, int y) current)
        {
            List<(int x, int y)> totalPath = new();
            totalPath.Add(current);
            while(cameFrom.TryGetValue(current, out var next))
            {
                current = next;
                totalPath.Add(current);
            }
            totalPath.Reverse();
            return totalPath;
        }

        private void UpdateMap(long output, long lastInput)
        {
            (int x, int y) nextLocation = (lastInput) switch
            {
                1 => botLocation.MoveDirection(Utilities.CompassDirection.N),
                2 => botLocation.MoveDirection(Utilities.CompassDirection.S),
                3 => botLocation.MoveDirection(Utilities.CompassDirection.W),
                4 => botLocation.MoveDirection(Utilities.CompassDirection.E),
                _ => throw new Exception()
            };

            map[nextLocation] = (int)output;

            if (output != 0) botLocation = nextLocation;
            if (output == 2) oxyGenLocation = nextLocation;
        }



        private void DrawMap()
        {
            StringBuilder sb = new("\n");
            for (int y = map.Keys.Max(a => a.y) + 1; y >= map.Keys.Min(a => a.y) - 1; y--)
            {
                for (int x = map.Keys.Min(a => a.x) -1 ; x <= map.Keys.Max(a => a.x) + 1; x++)
                {
                    var val = map.GetValueOrDefault((x, y), 4);
                    if ((x, y) == botLocation)
                    {
                        sb.Append('B');
                        continue;
                    }
                    if((x,y) == (0,0))
                    {
                        sb.Append('*');
                        continue;
                    }
                    switch (val)
                    {
                       
                        case 0: sb.Append('#'); break;
                        case 1: sb.Append(' '); break;
                        case 2: sb.Append('O'); break; 
                        case 4: sb.Append('?'); break;
                    }
                }
                sb.Append('\n');
            }
            Utilities.WriteLine(sb.ToString());
        }
    }
}