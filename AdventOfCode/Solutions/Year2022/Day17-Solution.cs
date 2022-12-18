using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(17, 2022, "Pyroclastic Flow")]
    class Day17 : ASolution
    {
        static readonly Coordinate2D[][] shapes = new Coordinate2D[][] {
                new Coordinate2D[] {(0,0), (1,0), (2,0), (3,0) },
                new Coordinate2D[] {(0,1), (1,0), (1,1), (1,2), (2,1) },
                new Coordinate2D[] {(0,0), (1,0), (2,0), (2,1), (2,2)},
                new Coordinate2D[] {(0,0), (0,1), (0,2), (0,3)},
                new Coordinate2D[] {(0,0), (0,1), (1,0), (1,1)},
            };

        Dictionary<(ulong topRows, long shapeIndex, long jetIndex), int> states = new(); //Using a bitmask I can store the top 9 rows of rocks in a base 64 number
        Dictionary<long, long[]> cache = new();
        Dictionary<Coordinate2D, int> tower;
        CompassDirection drop = CompassDirection.S;

        long part1;
        long part2;
        long part2Count = 1_000_000_000_000;


        public Day17() : base()
        {
            long[] heights = new long[] { 0, 0, 0, 0, 0, 0, 0 };
            tower = new() { //Create floor at height 0
                { new(0, 0), 1 }, 
                { new(1, 0), 1 }, 
                { new(2, 0), 1 }, 
                { new(3, 0), 1 }, 
                { new(4, 0), 1 }, 
                { new(5, 0), 1 }, 
                { new(6, 0), 1 },
            };

            states[(0, 0, 0)] = 0;
            cache[0] = heights;
            long jetIndex = 0;
            long i = 0;
            long cycleStart;
            long[] newHeights;
            while (!DropRock(i, ref jetIndex, heights, out newHeights, out cycleStart))
            {
                cache[i] = newHeights.ToArray();
                heights = newHeights.ToArray();
                i++;
            }

            var cycleLength = i - cycleStart;
            var offset = cache[cycleStart].Max();
            var growth = newHeights.Max() - offset;
            var p2cycleOffset = ((part2Count - cycleStart) % cycleLength) -1;
            var p1cycleOffset = ((2022 - cycleStart) % cycleLength) - 1;
            var p1numCycles = (2022 - cycleStart) / cycleLength; 
            var p2numCycles = (part2Count - cycleStart) / cycleLength;

            part1 = offset + (p1numCycles * growth) + (cache[cycleStart + p1cycleOffset].Max() - offset);
            part2 = offset + (p2numCycles * growth) + (cache[cycleStart + p2cycleOffset].Max() - offset);

        }

        protected override object SolvePartOne()
        {
            return part1;
        }

        protected override object SolvePartTwo()
        {
            return part2;
        }

        private bool DropRock(long shapeIndex, ref long jetIndex, long[] heights, out long[] newHeights, out long cycleStart)
        {
            cycleStart = 0;
            List<Coordinate2D> shape = new(shapes[shapeIndex % shapes.Length]);
            long maxHeight = heights.Max();
            newHeights = heights;
            long jetAtStart = jetIndex;

            shape = shape.Select(a => a.MoveDirection(CompassDirection.E, distance: 2)).Select(a => a.MoveDirection(CompassDirection.N, distance: (int)(maxHeight + 4))).ToList();

            while(true)
            {
                var moveDir = Input[(int)(jetIndex % Input.Length)] switch
                {
                    '<' => CompassDirection.W,
                    '>' => CompassDirection.E,
                    _ => throw new ArgumentException("Bad Input"),
                };

                //Check if sideways movement allowed and if so do it.
                if (shape.All(a => a.MoveDirection(moveDir).x is >= 0 and < 7 && !tower.ContainsKey(a.MoveDirection(moveDir))))
                {
                    shape = shape.Select(a => a.MoveDirection(moveDir)).ToList();
                }
                jetIndex++; //Always increase JetIndex

                //Attempt to drop 1 tile
                if(shape.Any(a => tower.ContainsKey(a.MoveDirection(drop))))
                {
                    //Couldn't drop, cement in place
                    foreach(var s in shape) tower[s] = 1;
                    //if (UseDebugInput) DrawTower((int)(maxHeight + 4), 10);
                    break;
                } else shape = shape.Select(a => a.MoveDirection(drop)).ToList();
            }

            //Save state, update cache, retrun new maxHeights

            //Find new max heights by walking up from the old spots to 4 higher than the current overall max, in case the tall skinny one landed on top of the highest point.

            for(int i = 0; i < 7; i++)
            {
                for(int j = 0; j < maxHeight + 5; j++)
                {
                    if (tower.ContainsKey((i, j))) newHeights[i] = j;
                }
            }

            //Build Caching long

            ulong toCache = 0;
            long topRow = newHeights.Max();

            for(int i = 0; i < 63; i++) //63 cells is exactly 9 rows 
            {
                toCache |= (ulong)(tower.GetValueOrDefault(new Coordinate2D(i % 7, (int)topRow - (i / 7)), 0)) << i;
            }

            cycleStart = states.GetValueOrDefault((toCache, shapeIndex % shapes.Length, jetAtStart % Input.Length), -1);

            if(cycleStart == -1)
            {
                states[(toCache, shapeIndex % shapes.Length, jetAtStart % Input.Length)] = (int)shapeIndex;
            }

            return cycleStart > 0;
        }

        private void DrawTower(int maxHeight, int numRows)
        {
            StringBuilder sb = new();
            //'█'
            for(int y = 0; y < numRows; y++)
            {
                if (maxHeight - y == 0)
                {
                    sb.AppendLine("+-------+");
                    break;
                }
                sb.Append('|');
                for(int x = 0; x < 7; x++)
                {
                    if (tower.ContainsKey((x, maxHeight - y))) sb.Append('#');
                    else sb.Append(' ');
                }
                sb.AppendLine("|");
            }

            Console.WriteLine(sb.ToString());
        }

    }
}
