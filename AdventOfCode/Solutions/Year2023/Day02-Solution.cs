using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(02, 2023, "Cube Conundrum")]
    class Day02 : ASolution
    {
        List<CubeGame> cubeGames = [];
        internal static readonly char[] separator = [':', ';'];

        public Day02() : base()
        {
            foreach (var g in Input.SplitByNewline())
            {
                var rounds = g.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var t = new CubeGame(rounds[0].ExtractInts().First());

                foreach (var r in rounds.Skip(1))
                {
                    var pulls = r.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    foreach (var p in pulls)
                    {
                        var count = p.ExtractInts().First();
                        if (p.Contains("red") && t.minRed < count) t.minRed = count;
                        else if (p.Contains("green") && t.minGreen < count) t.minGreen = count;
                        else if (p.Contains("blue") && t.minBlue < count) t.minBlue = count;
                    }
                }

                cubeGames.Add(t);
            }
        }

        protected override object SolvePartOne()
        {
            return cubeGames.Sum(g => (g.minRed <= 12 && g.minGreen <= 13 && g.minBlue <= 14) ? g.Id : 0);
        }

        protected override object SolvePartTwo()
        {
            return cubeGames.Sum(t => t.power);
        }

        private class CubeGame
        {
            public int Id { get; set; }
            public int minRed { get; set; } = 0;
            public int minBlue { get; set; } = 0;
            public int minGreen { get; set; } = 0;

            public int power => minRed * minBlue * minGreen;

            public CubeGame() { }
            public CubeGame(int Id)
            {
                this.Id = Id;
            }
        }
    }
}
