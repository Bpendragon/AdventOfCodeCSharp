using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2018
{

    class Day22 : ASolution
    {
        private const int SystemDepth = 11739; //Inserted directly from input. 
        private readonly (int x, int y) TargetLocation = (11, 718);
        Dictionary<(int x, int y), int> cave = new Dictionary<(int x, int y), int>(); //specifically value is Erosion level
        Dictionary<(int x, int y), SoilType> cave2 = new Dictionary<(int x, int y), SoilType>();

        public Day22() : base(22, 2018, "")
        {
            cave[(0, 0)] = SystemDepth % 20183; //By Definition
            cave2[(0, 0)] = SoilType.Rocky; //By Definition
            cave[TargetLocation] = SystemDepth % 20183;
            cave2[TargetLocation] = SoilType.Rocky;
            int erosionLevel;
            for (int x = 0; x <= TargetLocation.x + 1000; x++)
            {
                for(int y = 0; y <= SystemDepth; y++)
                {
                    if ((x, y) == (0, 0) || (x, y) == TargetLocation) continue; //these are given by definition
                     else if(x == 0)
                    {
                        erosionLevel = (((y * 48271) + SystemDepth) % 20183);
                    }else if (y == 0)
                    {
                        erosionLevel =  (((x * 16807) + SystemDepth) % 20183);
                    } else
                    {
                        erosionLevel = (((cave[(x - 1, y)] * cave[(x, y - 1)]) + SystemDepth) % 20183);
                    }
                    cave[(x, y)] = erosionLevel;
                    cave2[(x, y)] = (SoilType)(erosionLevel % 3);
                }
            }
        }

        protected override string SolvePartOne()
        { 
            return cave.Where(kvp => kvp.Key.x <= TargetLocation.x && kvp.Key.y <= TargetLocation.y).Sum(a=> a.Value % 3).ToString();
        }

        protected override string SolvePartTwo()
        {
            return TraversalTime((0,0), TargetLocation).ToString();
        }

        //A*, Hueristic is Manhattan + cost of gear change, this is why the sets also include an item for gear
        private int TraversalTime((int x, int y) start, (int x, int y) tgt) 
        {
            List<(int dX, int dY)> cardinalMoves = new List<(int dX, int dY)>() { (-1, 0), (1, 0), (0, -1), (0, 1) };
            List<((int x, int y) loc, Gear gear)> openSet = new List<((int x, int y) loc, Gear gear)>(); // I wish ordered list allowed key collisions
            Dictionary<((int x, int y) loc, Gear gear), ((int x, int y) loc, Gear gear)> cameFrom = new Dictionary<((int x, int y) loc, Gear gear), ((int x, int y) loc, Gear gear)>();
            Dictionary<((int x, int y) loc, Gear gear), int> gScore = new Dictionary<((int x, int y) loc, Gear gear), int>();
            Dictionary<((int x, int y) loc, Gear gear), int> fScore = new Dictionary<((int x, int y) loc, Gear gear), int>();

            openSet.Add(((start), Gear.torch));
            gScore[(start, Gear.torch)] = 0;
            fScore[(start, Gear.torch)] = start.ManhattanDistance(tgt); //sinc they're both rockiy in a perfect world it's a straight shot

            while(openSet.Count > 0)
            {
                var cur = openSet.OrderBy(a => fScore[a]).First();
                if (cur.loc == tgt)
                {
                    if (cur.gear == Gear.torch) return gScore[cur];
                    else return gScore[cur] + 7;
                }

                openSet.Remove(cur);
                foreach(var dir in cardinalMoves)
                {
                    var neighbor = cur.loc.Add(dir);
                    if (neighbor.x < 0 || neighbor.y < 0) continue; //out of bounds
                    int tentGScore = gScore[cur];
                    Gear nextTool = cur.gear;
                    switch(cur.gear)
                    {
                        case Gear.climbing:
                            switch(cave2[cur.loc])
                            {
                                case SoilType.Rocky:
                                    switch(cave2[neighbor])
                                    {
                                        case SoilType.Rocky:
                                            tentGScore++;
                                            break;
                                        case SoilType.Wet:
                                            tentGScore++;
                                            break;
                                        case SoilType.Narrow:
                                            tentGScore += 8;
                                            nextTool = Gear.torch;
                                            break;
                                    }
                                    break;
                                case SoilType.Wet:
                                    switch (cave2[neighbor])
                                    {
                                        case SoilType.Rocky:
                                            tentGScore++;
                                            break;
                                        case SoilType.Wet:
                                            tentGScore++;
                                            break;
                                        case SoilType.Narrow:
                                            tentGScore += 8;
                                            nextTool = Gear.neither;
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case Gear.torch:
                            switch(cave2[cur.loc])
                            {
                                case SoilType.Rocky:
                                    switch (cave2[neighbor])
                                    {
                                        case SoilType.Rocky:
                                            tentGScore++;
                                            break;
                                        case SoilType.Wet:
                                            tentGScore += 8;
                                            nextTool = Gear.climbing;
                                            break;
                                        case SoilType.Narrow:
                                            tentGScore++;
                                            break;
                                    }
                                    break;
                                case SoilType.Narrow:
                                    switch (cave2[neighbor])
                                    {
                                        case SoilType.Rocky:
                                            tentGScore++;
                                            break;
                                        case SoilType.Wet:
                                            tentGScore += 8;
                                            nextTool = Gear.neither;
                                            break;
                                        case SoilType.Narrow:
                                            tentGScore++;
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case Gear.neither:
                            switch (cave2[cur.loc])
                            {
                                case SoilType.Wet:
                                    switch (cave2[neighbor])
                                    {
                                        case SoilType.Rocky:
                                            tentGScore += 8;
                                            nextTool = Gear.climbing;
                                            break;
                                        case SoilType.Wet:
                                            tentGScore++;
                                            break;
                                        case SoilType.Narrow:
                                            tentGScore++;
                                            break;
                                    }
                                    break;
                                case SoilType.Narrow:
                                    switch (cave2[neighbor])
                                    {
                                        case SoilType.Rocky:
                                            tentGScore += 8;
                                            nextTool = Gear.torch;
                                            break;
                                        case SoilType.Wet:
                                            tentGScore++;
                                            break;
                                        case SoilType.Narrow:
                                            tentGScore++;
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }

                    if (tentGScore < gScore.GetValueOrDefault((neighbor, nextTool), int.MaxValue))
                    {
                        cameFrom[(neighbor, nextTool)] = cur;
                        gScore[(neighbor, nextTool)] = tentGScore;
                        fScore[(neighbor, nextTool)] = tentGScore + neighbor.ManhattanDistance(tgt);
                        if (!openSet.Contains((neighbor, nextTool))) openSet.Add((neighbor, nextTool));
                    }
                }
            }

            return int.MinValue;
        }

        private enum SoilType
        {
            Rocky = 0,
            Wet = 1,
            Narrow = 2
        }

        private enum Gear
        {
            climbing, 
            torch, 
            neither
        }
    }
}