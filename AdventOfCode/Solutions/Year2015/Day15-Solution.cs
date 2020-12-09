using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    class Day15 : ASolution
    {
        readonly List<string> Lines;
        readonly List<Ingredient> ings;
        public Day15() : base(15, 2015, "")
        {
            Lines = new List<string>(Input.SplitByNewline());
            ings = new List<Ingredient>();

            foreach (string line in Lines)
            {
                var tmp = line.Replace(":", "").Replace(",", " ").Split(" ", StringSplitOptions.RemoveEmptyEntries);
                ings.Add(new Ingredient
                {
                    name = tmp[0],
                    capacity = int.Parse(tmp[2]),
                    durability = int.Parse(tmp[4]),
                    flavor = int.Parse(tmp[6]),
                    texture = int.Parse(tmp[8]),
                    calories = int.Parse(tmp[^1])
                });
            }

        }

        protected override string SolvePartOne()
        {
            List<int> recipeScores = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < (100 - i); j++)
                {
                    for(int k = 0; k < (100 - i - j); k++)
                    {
                        int l = 100 - i - j - k;
                        recipeScores.Add(GetScore(i, j, k, l));
                        
                    }
                }
            }
            recipeScores = recipeScores.Distinct().ToList();
            return recipeScores.Max().ToString();
        }

       

        protected override string SolvePartTwo()
        {
            List<int> recipeScores = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < (100 - i); j++)
                {
                    for (int k = 0; k < (100 - i - j); k++)
                    {
                        int l = 100 - i - j - k;
                        recipeScores.Add(GetScore(i, j, k, l, true));

                    }
                }
            }
            recipeScores = recipeScores.Distinct().ToList();
            return recipeScores.Max().ToString();
        }

        private int GetScore(int i, int j, int k, int l, bool part2 = false)
        {
            if (i + j + k + l != 100) throw new Exception();
            int cap = (ings[0].capacity * i) + (ings[1].capacity * j) + (ings[2].capacity * k) + (ings[3].capacity * l);
            int dur = (ings[0].durability * i) + (ings[1].durability * j) + (ings[2].durability * k) + (ings[3].durability * l);
            int flav = (ings[0].flavor * i) + (ings[1].flavor * j) + (ings[2].flavor * k) + (ings[3].flavor * l);
            int tex = (ings[0].texture * i) + (ings[1].texture * j) + (ings[2].texture * k) + (ings[3].texture * l);
            int cal = (ings[0].calories * i) + (ings[1].calories * j) + (ings[2].calories * k) + (ings[3].calories * l);

            if (cap <= 0 || dur <= 0 || flav <= 0 || tex <= 0)
            {
                return 0;
            }

            if (part2)
            {
                if (cal == 500) return cap * dur * flav * tex;
                return 0;
            }

            return cap * dur * flav * tex;
        }
    }

    public class Ingredient
    {
        public string name;
        public int capacity;
        public int durability;
        public int flavor;
        public int texture;
        public int calories;
    }
}