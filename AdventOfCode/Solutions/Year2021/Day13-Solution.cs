using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2021
{

    class Day13 : ASolution
    {
        readonly Dictionary<Coordinate2D, string> paper = new();
        readonly List<string> folds = new();
        public Day13() : base(13, 2021, "Transparent Origami")
        {
            var halves = Input.Split("\n\n");
            foreach (var p in halves[0].SplitByNewline())
            {
                var asInts = p.ToIntList(",");
                paper[(asInts[0], asInts[1])] = "#";
            }

            folds = halves[1].SplitByNewline();
        }

        protected override string SolvePartOne()
        {
            var instruction = folds[0].Split()[2].Split("=");
            int lineLoc = int.Parse(instruction[1]);
            List<Coordinate2D> dotsToFlip = new();
            if (instruction[0] == "x")
            {
                dotsToFlip = paper.Keys.Where(a => a.x > lineLoc).ToList();
                foreach(var dot in dotsToFlip)
                {
                    paper.Remove(dot);
                    paper[(dot.x - ((dot.x - lineLoc) * 2), dot.y)] = "#";
                }

            }
            else if (instruction[0] == "y")
            {
                dotsToFlip = paper.Keys.Where(a => a.y > lineLoc).ToList();
                foreach (var dot in dotsToFlip)
                {
                    paper.Remove(dot);
                    paper[(dot.x, dot.y - ((dot.y - lineLoc) * 2))] = "#";
                }
            }
            else throw new ArgumentException();
            return paper.Count.ToString();
        }

        protected override string SolvePartTwo()
        {
            foreach(var tmp in folds.Skip(1))
            {
                var instruction = tmp.Split()[2].Split("=");
                int lineLoc = int.Parse(instruction[1]);
                List<Coordinate2D> dotsToFlip = new();
                if (instruction[0] == "x")
                {
                    dotsToFlip = paper.Keys.Where(a => a.x > lineLoc).ToList();
                    foreach (var dot in dotsToFlip)
                    {
                        paper.Remove(dot);
                        int newXCoord = dot.x - ((dot.x - lineLoc) * 2);
                        paper[(newXCoord, dot.y)] = "#";
                    }

                }
                else if (instruction[0] == "y")
                {
                    dotsToFlip = paper.Keys.Where(a => a.y > lineLoc).ToList();
                    foreach (var dot in dotsToFlip)
                    {
                        paper.Remove(dot);
                        int newYCoord = dot.y - ((dot.y - lineLoc) * 2);
                        paper[(dot.x, newYCoord)] = "#";
                    }
                }
                else throw new ArgumentException();
            }
            int maxX = paper.Max(a => a.Key.x);
            int maxY = paper.Max(a => a.Key.y);
            StringBuilder sb = new("\n");
            for (int i = 0; i <= maxY; i++)
            {
                for(int j = 0; j <= maxX; j++)
                {
                    sb.Append(paper.GetValueOrDefault((j, i), " "));
                }
                sb.Append('\n');
            }
            return sb.ToString();
        }
    }
}
