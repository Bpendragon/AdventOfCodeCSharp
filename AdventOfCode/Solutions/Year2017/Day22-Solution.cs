using System.Collections.Generic;
using System.Linq;
using System.Text;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2017
{

    [DayInfo(22, 2017, "")]
    class Day22 : ASolution
    {
        readonly Dictionary<(int x, int y), char> startMap = new();
        const string L = "L";
        const string R = "R";
        readonly (int x, int y) center;
        public Day22() : base()
        {
            var lines = Input.SplitByNewline();
            int xOffset = (lines[0].Length / 2);
            int yOffset = (lines.Count / 2);

            center = (xOffset, yOffset);

            StringBuilder sb = new();
            for (int j = 0; j < lines.Count; j++)
            {
                for (int i = 0; i < lines[0].Length; i++)
                {
                    startMap[(i, j)] = lines[j][i];
                    if ((i, j) == (lines[0].Length / 2, lines.Count / 2)) sb.Append($"[{lines[j][i]}]");
                    else sb.Append($" {lines[j][i]} ");
                }
                sb.Append('\n');
            }

            WriteLine("Starting Map: ");
            WriteLine(sb);
        }

        protected override object SolvePartOne()
        {
            (int x, int y) curPosition = center;
            CompassDirection curDirection = N;
            var newMap = new Dictionary<(int x, int y), char>(startMap);


            int numInfected = 0;

            foreach (int _ in Enumerable.Range(0, 10000))
            {
                switch (newMap[curPosition])
                {
                    case '#':
                        curDirection = curDirection.Turn(R);
                        newMap[curPosition] = '.';
                        break;
                    case '.':
                        curDirection = curDirection.Turn(L);
                        newMap[curPosition] = '#';
                        numInfected++;
                        break;
                }
                curPosition = curPosition.Move(curDirection, true);
                if (!newMap.ContainsKey(curPosition)) newMap[curPosition] = '.';
            }

            return numInfected;
        }

        protected override object SolvePartTwo()
        {
            (int x, int y) curPosition = center;
            CompassDirection curDirection = N;
            var newMap = new Dictionary<(int x, int y), char>(startMap);


            int numInfected = 0;

            foreach (int _ in Enumerable.Range(0, 10000000))
            {
                switch (newMap[curPosition])
                {
                    case '#':
                        curDirection = curDirection.Turn(R);
                        newMap[curPosition] = 'f';
                        break;
                    case '.':
                        curDirection = curDirection.Turn(L);
                        newMap[curPosition] = 'w';
                        break;
                    case 'w':
                        newMap[curPosition] = '#';
                        numInfected++;
                        break;
                    case 'f':
                        curDirection = curDirection.Turn(L, 180);
                        newMap[curPosition] = '.';
                        break;
                }
                curPosition = curPosition.Move(curDirection, true);
                if (!newMap.ContainsKey(curPosition)) newMap[curPosition] = '.';
            }

            return numInfected;
        }
    }
}
