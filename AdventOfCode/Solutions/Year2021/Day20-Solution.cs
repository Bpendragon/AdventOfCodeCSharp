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

namespace AdventOfCode.Solutions.Year2021
{

    class Day20 : ASolution
    {
        private readonly string enhancementString;
        private readonly Dictionary<Coordinate2D, string> initialImage;
        public Day20() : base(20, 2021, "Trench Map")
        {
            //UseDebugInput = true;
            initialImage = new();
            var halves = Input.Split("\n\n");
            enhancementString = halves[0];
            var imageLines = halves[1].SplitByNewline();
            for(int y = 0; y < imageLines.Count; y++)
            {

                for(int x = 0; x < imageLines[0].Length; x++)
                {
                    string val = imageLines[y][x] == '#' ? "1" : "0";
                    initialImage[(x, y)] = val;
                }
            }
            
        }

        protected override string SolvePartOne()
        {
            Dictionary<Coordinate2D, string> working = new(initialImage);
            foreach(int i in Enumerable.Range(1,2))
            {
                working = NextImage(working, enhancementString, i);
            }
            return working.Values.Count(a => a == "1").ToString();
        }

        protected override string SolvePartTwo()
        {
            Dictionary<Coordinate2D, string> working = new(initialImage);
            foreach (int i in Enumerable.Range(1, 50))
            {
                working = NextImage(working, enhancementString, i);
            }
            return working.Values.Count(a => a == "1").ToString();
        }

        private Dictionary<Coordinate2D, string> NextImage(Dictionary<Coordinate2D, string> CurrentImage, string EnhancementString, int passNum)
        {
            Dictionary<Coordinate2D, string> res = new();
            int minX = CurrentImage.Min(a => a.Key.x);
            int minY = CurrentImage.Min(a => a.Key.y);
            int maxX = CurrentImage.Max(a => a.Key.x);
            int maxY = CurrentImage.Max(a => a.Key.y);

            StringBuilder sb = new();

            for(int x = minX - 2; x < maxX + 2; x++)
            {
                for(int y = minY - 2; y < maxY + 2; y++)
                {
                    foreach(var n in OrderedNeighbors)
                    {
                        var tmp = passNum % 2 == 1 || UseDebugInput ? CurrentImage.GetValueOrDefault((x, y) + n, "0") : CurrentImage.GetValueOrDefault((x, y) + n, "1");
                        sb.Append(tmp);
                    }

                    var binString = sb.ToString();
                    sb.Clear();
                    int index = Convert.ToInt32(binString, 2);
                    res[(x, y)] = EnhancementString[index] == '#' ? "1" : "0";
                }
            }

            return res;
        }

        private static List<Coordinate2D> OrderedNeighbors => new() {
            (-1,-1), (0,-1), (1,-1),
            (-1,0), (0,0), (1,0),
            (-1, 1), (0,1), (1,1)
        };
    }
}
