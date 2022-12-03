using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    class Day21 : ASolution
    {
        readonly int playerHP = 100;
        readonly List<int> CostsOfVictory = new();
        readonly List<int> CostsOfFailure = new();

        //FROM INPUT
        const int bossHP = 103;
        const int bossDmg = 9;
        const int bossDef = 2;
        public Day21() : base(21, 2015, "")
        {
                    var weapons = new[]
            {
                new { Cost = 8, Attack = 4 },
                new { Cost = 10, Attack = 5 },
                new { Cost = 25, Attack = 6 },
                new { Cost = 40, Attack = 7 },
                new { Cost = 74, Attack = 8 },
            };


                    var Armor = new[]
                {
                new { Cost = 0, Armor = 0 }, //The No Armor Option
                new { Cost = 13, Armor = 1 },
                new { Cost = 31, Armor = 2 },
                new { Cost = 53, Armor = 3 },
                new { Cost = 75, Armor = 4 },
                new { Cost = 102, Armor = 5 },
            };

                    var rings = new[]
                    {
                new { Cost = 0, Attack = 0, Armor = 0, ID = 0 }, //Need 2 empties with different IDs for the "No Rings" option
                new { Cost = 0, Attack = 0, Armor = 0, ID = 1  },
                new { Cost = 25, Attack = 1, Armor = 0, ID = 2  },
                new { Cost = 50, Attack = 2, Armor = 0, ID = 3  },
                new { Cost = 100, Attack = 3, Armor = 0, ID = 4  },
                new { Cost = 20, Attack = 0, Armor = 1, ID = 5  },
                new { Cost = 40, Attack = 0, Armor = 2 , ID = 6 },
                new { Cost = 80, Attack = 0, Armor = 3, ID = 7  },

            };


                    var combinations =
                from w in weapons
                from a in Armor
                from ring1 in rings
                from ring2 in rings.Where(x => ring1.ID != x.ID)
                select new
                {
                    Attack = w.Attack + ring1.Attack + ring2.Attack,
                    Defence = a.Armor + ring1.Armor + ring2.Armor,
                    Cost = w.Cost + a.Cost + ring1.Cost + ring2.Cost
                };

            foreach(var combo in combinations)
            {
                if (PlayerWins(combo.Attack, combo.Defence)) CostsOfVictory.Add(combo.Cost);
                else CostsOfFailure.Add(combo.Cost);
            }
        }


        protected override object SolvePartOne()
        {
            return CostsOfVictory.Min();
        }

        protected override object SolvePartTwo()
        {
            return CostsOfFailure.Max();
        }


        private bool PlayerWins(int Attack, int Defence)
        {
            double dmgBossTakesPerTurn = Attack - bossDef >= 1 ? Attack - bossDef : 1.0;
            double dmgPlayerTakesPerTurn = bossDmg - Defence >= 1 ? bossDmg - Defence : 1.0;

            int turnsToKillBoss = (int)Math.Ceiling(bossHP / dmgBossTakesPerTurn);
            int turnsToDie = (int)Math.Ceiling(playerHP / dmgPlayerTakesPerTurn);

            return turnsToDie >= turnsToKillBoss; //Player always moves first. Thus if they would dies same turn, player wins (barely)
        }

    }
}