namespace AdventOfCode.Solutions.Year2018
{

    class Day17 : ASolution
    {
        public char[,] grid;
        int x = 2000;
        int y = 2000;
        readonly int maxY = 0;
        readonly int minY = int.MaxValue; //get our vertical slice

        readonly int springX = 500;
        readonly int springY = 0;
        public Day17() : base(17, 2018, "")
        {
            grid = new char[x, y];
            foreach (var line in Input.SplitByNewline())
            {
                var l = line.Split(new[] { '=', ',', '.' });

                if (l[0] == "x")
                {
                    x = int.Parse(l[1]);
                    y = int.Parse(l[3]);
                    var len = int.Parse(l[5]);
                    for (var i = y; i <= len; i++)
                    {
                        grid[x, i] = '#';
                    }
                }
                else
                {
                    y = int.Parse(l[1]);
                    x = int.Parse(l[3]);
                    var len = int.Parse(l[5]);
                    for (var i = x; i <= len; i++)
                    {
                        grid[i, y] = '#';
                    }
                }

                if (y > maxY)
                {
                    maxY = y;
                }

                if (y < minY)
                {
                    minY = y;
                }
            }
            DropWater(springX, springY);
        }

        protected override object SolvePartOne()
        {
            var waterCount = 0;
            for (y = minY; y < grid.GetLength(1); y++)
            {
                for (x = 0; x < grid.GetLength(0); x++)
                {
                    if (grid[x, y] == '~' || grid[x, y] == '|') 
                    {
                        waterCount++;
                    }
                }
            }
            return waterCount;
        }

        protected override object SolvePartTwo()
        {
            var waterCount = 0;
            for (y = minY; y < grid.GetLength(1); y++)
            {
                for (x = 0; x < grid.GetLength(0); x++)
                {
                    if (grid[x, y] == '~')
                    {
                        waterCount++;
                    }
                }
            }
            return waterCount;
        }

        private void DropWater(int x, int y)
        {
            grid[x, y] = '|';
            while (!SpaceTaken(x, y+1))
            {
                y++;
                if (y > maxY) return; //we've reached the end of the scan.
                grid[x, y] = '|';
            }

            while(true)
            {
                bool TravelLeft = false;
                bool TravelRight = false;
                int minX;
                for(minX = x; minX >= 0; minX--)
                {
                    if(!SpaceTaken(minX, y+1)) //check for empty space below, remember +y is down
                    {
                        TravelLeft = true;
                        break;
                    }

                    grid[minX, y] = '|'; //assume flowing water until we can be told it's still

                    if (SpaceTaken(minX - 1, y)) break;
                }

                int maxX;
                for (maxX = x; maxX < 2000; maxX++)
                {
                    if (!SpaceTaken(maxX, y + 1))
                    {
                        TravelRight = true;
                        break;
                    }

                    grid[maxX, y] = '|';

                    if (SpaceTaken(maxX + 1, y)) break;
                }

                if (TravelLeft)
                {
                    if (grid[minX, y] != '|')
                        DropWater(minX, y); //o boi recursion
                }

                if (TravelRight)
                {
                    if (grid[maxX, y] != '|')
                        DropWater(maxX, y);
                }

                if (TravelLeft || TravelRight) return; //the ~~spice~~ water must flow! (break here if the water continues to fall, the top later is considered flowing)

                for (int i = minX; i <= maxX; i++) grid[i, y] = '~';//replace the row with still water
                y--; //step back up a layer in the container
            }
        }

        private bool SpaceTaken(int x, int y)
        {
            return grid[x, y] == '#' || grid[x, y] == '~';
        }
    }
}