using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2018
{

    class Day18 : ASolution
    {
        readonly Dictionary<(int x, int y), char> baseMap = new Dictionary<(int x, int y), char>();
        public Day18() : base(18, 2018, "")
        {
            int y = 0;
            foreach(var line in Input.SplitByNewline())
            {
                for(int x = 0; x < line.Length; x++)
                {
                    baseMap[(x, y)] = line[x];
                }
                y++;
            }
        }

        protected override string SolvePartOne()
        {
            var p1 = new Dictionary<(int x, int y), char>(baseMap);

            string res = LoggingSteps(p1, 10);

            return (res.Count(x => x == '|') * res.Count(x => x == '#')).ToString();
        }

        protected override string SolvePartTwo()
        {
            var p1 = new Dictionary<(int x, int y), char>(baseMap);

            string res = LoggingSteps(p1, 1000000000);

            return (res.Count(x => x == '|') * res.Count(x => x == '#')).ToString();
        }

        private string LoggingSteps(Dictionary<(int x, int y), char> p1, int numIterations)
        {
            List<string> SeenLayouts = new List<string>();
            Dictionary<(int x, int y), char> next;
            foreach (int i in Enumerable.Range(1, numIterations))
            {
                next = new Dictionary<(int x, int y), char>(p1);
                foreach (var cell in p1.Keys)
                {
                    int trees = 0;
                    int clear = 0;
                    int yards = 0;
                    foreach(var dir in (CompassDirection[])Enum.GetValues(typeof(CompassDirection)))
                    {
                        char c = p1.GetValueOrDefault(cell.MoveDirection(dir), '~');
                        switch (c)
                        {
                            case '~': continue;
                            case '.': clear++; break;
                            case '#': yards++; break;
                            case '|': trees++; break;
                        }
                    }
                    switch (p1[cell])
                    {
                        case '.':
                            if (trees >= 3) next[cell] = '|';
                            break;
                        case '|':
                            if (yards >= 3) next[cell] = '#';
                            break;
                        case '#':
                            if (yards < 1 || trees < 1) next[cell] = '.';
                            break;
                    }
                }
                p1 = new Dictionary<(int x, int y), char>(next);

                StringBuilder sb = new StringBuilder();
                for(int y = 0; y < 50; y++)
                {
                    for(int x = 0; x < 50; x++)
                    {
                        sb.Append(p1[(x, y)]);
                    }
                }
                var tmp = sb.ToString();

                if(SeenLayouts.Contains(tmp))
                {
                    SeenLayouts.RemoveRange(0, SeenLayouts.IndexOf(tmp));
                    return SeenLayouts[(numIterations - i) % SeenLayouts.Count];
                } else
                {
                    SeenLayouts.Add(tmp);
                }
            }

            return SeenLayouts[^1];
        }
    }
}