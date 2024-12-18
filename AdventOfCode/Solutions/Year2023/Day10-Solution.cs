using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(10, 2023, "Pipe Maze")]
    class Day10 : ASolution
    {
        Dictionary<Coordinate2D, char> map = new();
        int maxX;
        int maxY;

        HashSet<Coordinate2D> visited = new();
        List<CompassDirection> dirs = new() { N, W, S, E };
        char CorrectS = char.MinValue;
        Coordinate2D start;


        Dictionary<(char pipe, CompassDirection inFrom), CompassDirection> pipeDirChange = new()
        {
            {('|', N), S },
            {('|', S), N },
            {('-', E), W },
            {('-', W), E },
            {('L', N), E },
            {('L', E), N },
            {('J', N), W },
            {('J', W), N },
            {('7', S), W },
            {('7', W), S },
            {('F', E), S },
            {('F', S), E },
        };

        public Day10() : base()
        {
            (map, maxX, maxY) = Input.GenerateMap(false);
            start = map.FirstOrDefault(x => x.Value == 'S').Key;
        }

        protected override object SolvePartOne()
        {
            string loopPath = string.Empty;
            foreach (var pipe in "|-LJ7F")
            {
                (bool isLoop, string path) = CheckLoop(pipe);
                if (isLoop)
                {
                    loopPath = path;
                    CorrectS = pipe;
                    break;
                }
            }

            return loopPath.Length / 2;
        }

        protected override object SolvePartTwo()
        {
            map[start] = CorrectS;
            var keyList = map.Keys.ToList();
            foreach (var p in keyList.Where(x => !visited.Contains(x)))
            {
                map[p] = '.';
            }
            List<string> cleanedMap = new();
            for (int y = 0; y <= maxY; y++)
            {
                StringBuilder sb = new();
                for (int x = 0; x <= maxX; x++)
                {
                    sb.Append(map[(x, y)]);
                }

                cleanedMap.Add(Regex.Replace(Regex.Replace(sb.ToString(), "F-*7|L-*J", string.Empty), "F-*J|L-*7", "|"));
            }

            int ans = 0;

            foreach (var l in cleanedMap)
            {
                int parity = 0;
                foreach (var c in l)
                {
                    if (c == '|') parity++;
                    if (c == '.' && parity % 2 == 1) ans++;
                }
            }

            return ans;
        }


        private (bool isLoop, string path) CheckLoop(char replaceS)
        {   //To save computation S prefers to go North, West, and South in that order
            StringBuilder sb = new("S");
            bool isLoop = false;

            var cur = start;
            CompassDirection curDir = (replaceS) switch
            {
                '|' => N,
                '-' => W,
                'L' => N,
                'J' => N,
                '7' => W,
                'F' => S,
                _ => throw new ArgumentOutOfRangeException($"'{replaceS}' is not a valid pipe designator")
            };

            HashSet<Coordinate2D> tempVisited = new();
            tempVisited.Add(start);
            while (map.TryGetValue(cur.Move(curDir, true), out var nextPipe) && (nextPipe == 'S' || pipeDirChange.ContainsKey((nextPipe, curDir.Flip()))))
            {
                if (nextPipe == 'S')
                {
                    isLoop = true;
                    visited = tempVisited;
                    break;
                }

                sb.Append(nextPipe);
                cur = cur.Move(curDir, true);
                curDir = pipeDirChange[(nextPipe, curDir.Flip())];
                tempVisited.Add(cur);
            }

            return (isLoop, sb.ToString());
        }
    }
}
