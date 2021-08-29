using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2019
{

    class Day10 : ASolution
    {
        readonly List<Asteroid> Asteroids;
        readonly Asteroid baseLoc;
        public Day10() : base(10, 2019, "")
        {
            Asteroids = new List<Asteroid>();
            string[] lines = Input.SplitByNewline();
            for (int i = 0; i < lines[0].Length; i++)
            {
                for (int j = 0; j < lines.Length; j++)
                {
                    if (lines[i][j] == '#') Asteroids.Add(new Asteroid((i, j)));
                }
            }

            foreach (IEnumerable<Asteroid> pair in Asteroids.Combinations(2))
            {

                Asteroid ast1 = pair.First();
                Asteroid ast2 = pair.Last();
                double dX = Math.Abs(ast1.Coords.x - ast2.Coords.x);
                double dY = Math.Abs(ast1.Coords.y - ast2.Coords.y);
                int gcd = (int)Utilities.FindGCD(dX, dY);
                if (gcd == 0) gcd = (int)Math.Max(dX, dY); //avoid divide by 0

                (int dX, int dY) onetoTwo = ((ast2.Coords.x - ast1.Coords.x) / gcd, (ast2.Coords.y - ast1.Coords.y) / gcd);
                (int dX, int dY) twoToOne = (-onetoTwo.dX, -onetoTwo.dY);

                if (ast1.InView.ContainsKey(onetoTwo))
                {
                    if (Utilities.ManhattanDistance(ast1.Coords, ast1.InView[onetoTwo].Coords) > Utilities.ManhattanDistance(ast1.Coords, ast2.Coords))
                    {
                        ast1.InView[onetoTwo] = ast2;
                    }
                }
                else
                {
                    ast1.InView[onetoTwo] = ast2;
                }

                if (ast2.InView.ContainsKey(twoToOne))
                {
                    if (Utilities.ManhattanDistance(ast2.Coords, ast2.InView[twoToOne].Coords) > Utilities.ManhattanDistance(ast1.Coords, ast2.Coords))
                    {
                        ast2.InView[twoToOne] = ast1;
                    }
                }
                else
                {
                    ast2.InView[twoToOne] = ast1;
                }
            }
            Asteroids.Sort((a, b) => a.InView.Count.CompareTo(b.InView.Count));
            baseLoc = Asteroids.Last();
        }

        protected override string SolvePartOne()
        {

            return baseLoc.InView.Count.ToString();
        }

        protected override string SolvePartTwo()
        {
            //I tried to do something fancy, then I decided I'd just do it using excel and sorting. 
            using (System.IO.StreamWriter file =
            new(@"D:\Users\campb\Documents\AdventOfCode\AdventOfCodeCSharp\AdventOfCode\Solutions\Year2019\Day10-visible.txt"))
            {
                foreach (KeyValuePair<(int, int), Asteroid> vis in baseLoc.InView)
                {
                    file.WriteLine(vis.Value.Coords);
                }
            }
            /*
            List<(int, int)> targets = new List<(int, int)>(baseLoc.InView.Keys);
            targets.Sort((a, b) => Math.Atan2(a.Item1, a.Item2).CompareTo(Math.Atan2(b.Item1, b.Item2)));
            var vec = (0, 1);
            var index = targets.IndexOf(vec);



            var finaltgt = baseLoc.InView[targets[(index + 200) % targets.Count]];

            return ((finaltgt.Coords.x * 100) + finaltgt.Coords.y).ToString();
            */
            return null;
        }

        public class Asteroid
        {
            public (int x, int y) Coords { get; } = new ValueTuple<int, int>(-1, -1);

            public Dictionary<(int, int), Asteroid> InView { get; set; } = new Dictionary<(int, int), Asteroid>();

            public Asteroid((int x, int y) Coords)
            {
                this.Coords = Coords;
            }
        }
    }
}