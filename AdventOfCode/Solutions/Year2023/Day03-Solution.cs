using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using System.Data;
using System.Threading;
using System.Security;
using static AdventOfCode.Solutions.Utilities;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(03, 2023, "Gear Ratios")]
    class Day03 : ASolution
    {
        Dictionary<Coordinate2D, char> engineMap = new();
        Dictionary<Coordinate2D, List<int>> gears = new(); 
        int maxX, maxY;

        public Day03() : base()
        {
            engineMap = Input.GenerateMap(out maxX, out maxY);
        }

        protected override object SolvePartOne()
        {
            int sum = 0;

            for(int y = 0; y <= maxY; y++)
            {
                for(int x = 0; x <= maxX; x++)
                {
                    if(engineMap.TryGetValue((x,y), out char c) && char.IsDigit(c))
                    {
                        bool nextToSymbol = false;
                        bool symbolIsGear = false;
                        Coordinate2D gearLoc = (-1, -1);
                        int num = 0;
                        while(engineMap.TryGetValue((x, y), out c) && char.IsDigit(c))
                        {
                            var neighbors = new Coordinate2D(x, y).Neighbors(true);
                            foreach(var n in neighbors)
                            {
                                if (engineMap.TryGetValue(n, out char d) && !char.IsDigit(d))
                                {
                                    nextToSymbol = true;
                                    if(d == '*')
                                    {
                                        symbolIsGear = true;
                                        gearLoc = n;
                                    }
                                }
                            }
                            num = (num * 10) + int.Parse(c.ToString());
                            x++;
                        }
                        if (nextToSymbol) sum += num;
                        if (symbolIsGear)
                        {
                            if (!gears.ContainsKey(gearLoc)) gears[gearLoc] = new();
                            gears[gearLoc].Add(num);
                        }
                    }
                }
            }

            return sum;
        }

        protected override object SolvePartTwo()
        {
            long sum = 0;

            foreach(var kvp in gears)
            {
                if(kvp.Value.Count == 2) sum += kvp.Value.Aggregate(1, (a, b) => a * b);
            }

            return sum;
        }
    }
}
