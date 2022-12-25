using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(08, 2022, "Treetop Tree House")]
    class Day08 : ASolution
    {
        readonly Dictionary<Coordinate2D, int> trees = new();
        readonly int maxX, maxY;
        public Day08() : base()
        {
            var lines = Input.SplitByNewline();
            maxY = lines.Count - 1;
            maxX = lines[0].Length - 1;
            for (int y = 0; y < lines.Count; y++)
            {
                var nums = lines[y].ToIntList();
                for (int x = 0; x < nums.Count; x++)
                {
                    trees[(x, y)] = nums[x];
                }
            }
        }

        protected override object SolvePartOne()
        {
            HashSet<Coordinate2D> visibleTrees = new();

            for (int y = 0; y <= maxY; y++)
            {
                int tallest;
                Coordinate2D next;
                if (y == 0 || y == maxY) //top and bottom edges
                {

                    for (int x = 0; x <= maxX; x++)
                    {
                        tallest = trees[(x, y)];
                        visibleTrees.Add((x, y));
                        //Search North
                        next = (x, y).MoveDirection(N);
                        while (trees.TryGetValue(next, out int height))
                        {
                            if (height > tallest)
                            {
                                tallest = height;
                                visibleTrees.Add(next);
                            }
                            next = next.MoveDirection(N);
                        }
                        //Search South
                        tallest = trees[(x, y)];
                        next = (x, y).MoveDirection(S);
                        while (trees.TryGetValue(next, out int height))
                        {
                            if (height > tallest)
                            {
                                tallest = height;
                                visibleTrees.Add(next);
                            }
                            next = next.MoveDirection(S);
                        }
                    }
                }
                else // vertical edges
                {
                    //Search East
                    tallest = trees[(0, y)];
                    visibleTrees.Add((0, y));
                    next = (0, y).MoveDirection(E);
                    while (trees.TryGetValue(next, out int height))
                    {
                        if (height > tallest)
                        {
                            tallest = height;
                            visibleTrees.Add(next);
                        }
                        next = next.MoveDirection(E);
                    }

                    //Search West
                    tallest = trees[(maxX, y)];
                    visibleTrees.Add((maxX, y));
                    next = (maxX, y).MoveDirection(W);
                    while (trees.TryGetValue(next, out int height))
                    {
                        if (height > tallest)
                        {
                            tallest = height;
                            visibleTrees.Add(next);
                        }
                        next = next.MoveDirection(W);
                    }
                }
            }

            return visibleTrees.Count;
        }

        protected override object SolvePartTwo()
        {
            int maxScore = 0;
            for (int y = 0; y <= maxY; y++)
            {
                for (int x = 0; x <= maxX; x++)
                {
                    int maxAllowed = trees[(x, y)];
                    int scenicScore = 1;
                    int visTrees = 0;
                    Coordinate2D next = (x, y).MoveDirection(N);
                    //North
                    while(trees.TryGetValue(next, out int height))
                    {
                        if (height < maxAllowed) visTrees++;
                        if(height == maxAllowed)
                        {
                            visTrees++;
                            break;
                        }
                        next = next.MoveDirection(N);
                    }
                    scenicScore *= visTrees;
                    if (scenicScore == 0) continue;
                    visTrees = 0;
                    //South
                    next = (x, y).MoveDirection(S);
                    while (trees.TryGetValue(next, out int height))
                    {
                        if (height < maxAllowed) visTrees++;
                        if (height >= maxAllowed)
                        {
                            visTrees++;
                            break;
                        }
                        next = next.MoveDirection(S);
                    }
                    scenicScore *= visTrees;
                    if (scenicScore == 0) continue;
                    visTrees = 0;

                    //East
                    next = (x, y).MoveDirection(E);
                    while (trees.TryGetValue(next, out int height))
                    {
                        if (height < maxAllowed) visTrees++;
                        if (height == maxAllowed)
                        {
                            visTrees++;
                            break;
                        }
                        next = next.MoveDirection(E);
                    }
                    scenicScore *= visTrees;
                    if (scenicScore == 0) continue;
                    visTrees = 0;

                    //West
                    next = (x, y).MoveDirection(W);
                    while (trees.TryGetValue(next, out int height))
                    {
                        if (height < maxAllowed) visTrees++;
                        if (height == maxAllowed)
                        {
                            visTrees++;
                            break;
                        }
                        next = next.MoveDirection(W);
                    }
                    scenicScore *= visTrees;
                    if (scenicScore > maxScore) maxScore = scenicScore;

                }
            }

            return maxScore;
        }
    }
}
