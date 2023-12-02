using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(02, 2023, "Cube Conundrum")]
    class Day02 : ASolution
    {
        List<CubeGame> cubeGames = [];
        internal static readonly char[] separator = [':', ';'];
        readonly Regex PILES = new("(?<amount>\\d+) (?<color>red|green|blue)");

        public Day02() : base()
        {
            foreach (var g in Input.SplitByNewline())
            {
                var t = new CubeGame(g.ExtractInts().First());
                var matches = PILES.Matches(g);
                foreach (Match m in matches)
                {
                    (string color, int amount) = (m.Groups["color"].Value, int.Parse(m.Groups["amount"].Value)); 
                    switch (color)
                    {
                        case "red": t.red = Math.Max(t.red, amount); break;
                        case "green": t.green = Math.Max(t.green, amount); break;
                        case "blue": t.blue = Math.Max(t.blue, amount); break;
                    }
                }

                cubeGames.Add(t);
            }
        }

        protected override object SolvePartOne()
        {
            return cubeGames.Sum(t => t.p1Score);
        }

        protected override object SolvePartTwo()
        {
            return cubeGames.Sum(t => t.power);
        }

        private class CubeGame
        {
            public int Id { get; set; }
            public int red { get; set; } = 0;
            public int blue { get; set; } = 0;
            public int green { get; set; } = 0;

            public int power => red * blue * green;
            public int p1Score => red <= 12 && green <= 13 && blue <= 14 ? Id : 0;

            public CubeGame() { }
            public CubeGame(int Id)
            {
                this.Id = Id;
            }
        }
    }
}
