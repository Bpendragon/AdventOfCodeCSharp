using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    class Day21 : ASolution
    {
        readonly Dictionary<string, List<string>> AllergenPossibilities = new();
        readonly Dictionary<string, int> ingredientCounts = new();
        readonly StringSplitOptions splitOpts = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
        public Day21() : base(21, 2020, "Allergen Assessment")
        {
            foreach(var line in Input.SplitByNewline(true))
            {
                var halves = line.Split("()".ToCharArray(), splitOpts) ;
                var ings = halves[0].Split();
                
                foreach(var ing in ings)
                {
                    if (ingredientCounts.TryGetValue(ing, out _))
                    {
                        ingredientCounts[ing]++;
                    } else
                    {
                        ingredientCounts[ing] = 1;
                    }
                }

                string[] allergens = halves[1].Split(' ', splitOpts);
                for(int i = 1; i < allergens.Length; i++) //skip "contains
                {
                    var actAllergen = allergens[i].Trim(',');
                    if(AllergenPossibilities.TryGetValue(actAllergen, out var currentList))
                    {
                        AllergenPossibilities[actAllergen] = currentList.Intersect(ings).ToList();
                    } else
                    {
                        AllergenPossibilities[actAllergen] = new List<string>(ings);
                    }
                }
            }
        }

        protected override string SolvePartOne()
        {
            List<string> knownAllergens = AllergenPossibilities.Values.Aggregate((a, b) => a.Union(b).ToList());

            return ingredientCounts.Where(x => !knownAllergens.Contains(x.Key)).Sum(x => x.Value).ToString();
        }

        protected override string SolvePartTwo()
        {
            Dictionary<string, string> knownAllergens = new();


            while(AllergenPossibilities.Any(x => x.Value.Count > 1))
            {
                var onlyOnes = AllergenPossibilities.Where(x => x.Value.Count == 1).ToList();
                foreach(var s in onlyOnes)
                {
                    foreach(var item in AllergenPossibilities.Where(x => x.Key != s.Key))
                    {
                        AllergenPossibilities[item.Key] = item.Value.Except(s.Value).ToList();
                    }
                }
            }

            StringBuilder sb = new();

            List<string> sortedAllergens = AllergenPossibilities.Keys.ToList();
            sortedAllergens.Sort();

            foreach(var s in sortedAllergens)
            {
                sb.Append($"{AllergenPossibilities[s][0]},");
            }

            return sb.ToString().TrimEnd(',');
        }
    }
}
