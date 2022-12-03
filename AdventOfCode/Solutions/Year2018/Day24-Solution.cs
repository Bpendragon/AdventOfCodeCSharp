using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2018
{

    class Day24 : ASolution
    {
        public Day24() : base(24, 2018, "")
        {
        }

        protected override object SolvePartOne()
        {
            List<Unit> Units = new(GetUnits());
            var res = SimulateCombat(Units, out bool _);
            return res;
        }



        protected override object SolvePartTwo()
        {
            int curBoost = 0;
            long res;

            bool lastWasWin;
            do
            {
                curBoost++;
                res = SimulateCombat(new List<Unit>(GetUnits(boost: curBoost)), out lastWasWin);
            } while (!lastWasWin);
            return res;
        }


        private class Unit : IComparable<Unit>, IEquatable<Unit>
        {
            public bool isImmuneSystem;
            public long numAlive;
            public long hitPoints;
            public long attackDamage;
            public AttackTypes attackType;
            public long initiative;
            public List<AttackTypes> Immunities = new();
            public List<AttackTypes> Weaknesses = new();

            public long EffectivePower => numAlive * attackDamage;

            public int CompareTo(Unit other)
            {
                return this.initiative.CompareTo(other.initiative);
            }

            public bool Equals(Unit other)
            {
                if (ReferenceEquals(this, other)) return true;
                if (other is null) return false;
                return this.initiative == other.initiative;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Unit);
            }

            public override int GetHashCode()
            {
                return initiative.GetHashCode();
            }
        }

        private enum AttackTypes
        {
            fire,
            slashing,
            bludgeoning,
            radiation,
            cold
        }

        private static long SimulateCombat(List<Unit> units, out bool isImmuneWin)
        {

            while (units.Any(a => a.isImmuneSystem) && units.Any(a => !a.isImmuneSystem))
            {
                //Target Selection Phase
                long numTargetsKilled = 0;
                var targetSelectionOrder = units.OrderByDescending(a => a.EffectivePower).ThenByDescending(a => a.initiative); //must re-sort every time
                List<Unit> alreadySelected = new();
                SortedDictionary<Unit, Unit> attackplan = new();
                foreach (var unit in targetSelectionOrder)
                {
                    var possibleTargets = units.Where(x => x.isImmuneSystem != unit.isImmuneSystem).Except(alreadySelected);
                    Unit bestTarget = null;
                    long bestDamage = -1;
                    foreach (var target in possibleTargets)
                    {
                        long expectedAttack = unit.EffectivePower;
                        if (target.Immunities.Contains(unit.attackType)) expectedAttack = 0;
                        else if (target.Weaknesses.Contains(unit.attackType)) expectedAttack = 2 * unit.EffectivePower;

                        if (expectedAttack > bestDamage)
                        {
                            bestTarget = target;
                            bestDamage = expectedAttack;
                        }
                        else if (expectedAttack == bestDamage && target.EffectivePower > bestTarget.EffectivePower)
                        {
                            bestTarget = target;
                            bestDamage = expectedAttack;
                        }
                        else if ((expectedAttack == bestDamage && target.EffectivePower == bestTarget.EffectivePower) && target.initiative > bestTarget.initiative)
                        {
                            bestTarget = target;
                            bestDamage = expectedAttack;
                        }

                    }

                    if (bestTarget == null) continue;
                    attackplan[unit] = bestTarget;
                    alreadySelected.Add(bestTarget);
                }

                //Attack Phase
                var attackOrder = attackplan.Keys.Reverse();

                foreach (var attacker in attackOrder)
                {
                    if (!units.Contains(attacker)) continue; //attacker was wiped out while defending earlier in the match
                    var target = attackplan[attacker];

                    long expectedAttack = attacker.EffectivePower;
                    if (target.Immunities.Contains(attacker.attackType)) expectedAttack = 0;
                    else if (target.Weaknesses.Contains(attacker.attackType)) expectedAttack = 2 * attacker.EffectivePower;
                    long unitsLost = expectedAttack / target.hitPoints; //integer division, can only take out whole number of units.
                    target.numAlive -= unitsLost;
                    numTargetsKilled += unitsLost;

                    if (target.numAlive <= 0) units.Remove(target);
                }

                if (numTargetsKilled == 0)
                {
                    isImmuneWin = false;
                    return -1;
                }
            }
            if (units.Any(a => a.isImmuneSystem))
            { 
                isImmuneWin = true; 
            }
            else isImmuneWin = false;
            return units.Sum(a => a.numAlive);
        }





        //eegads this is ugly
        private List<Unit> GetUnits(int boost = 0)
        {
            var lines = Input.SplitByNewline();
            var immuneSystem = false;
            var res = new List<Unit>();
            foreach (var line in lines)
                if (line == "Immune System:")
                {
                    immuneSystem = true;
                }
                else if (line == "Infection:")
                {
                    immuneSystem = false;
                }
                else if (line != "")
                {
                    var rx = @"(\d+) units each with (\d+) hit points(.*)with an attack that does (\d+)(.*)damage at initiative (\d+)";
                    var m = Regex.Match(line, rx);
                    if (m.Success)
                    {
                        Unit g = new()
                        {
                            isImmuneSystem = immuneSystem,
                            numAlive = int.Parse(m.Groups[1].Value),
                            hitPoints = int.Parse(m.Groups[2].Value),
                            attackDamage = int.Parse(m.Groups[4].Value),
                            attackType = (AttackTypes)Enum.Parse(typeof(AttackTypes), m.Groups[5].Value.Trim(), true),
                            initiative = int.Parse(m.Groups[6].Value)
                        };

                        if (immuneSystem) g.attackDamage += boost;
                        var st = m.Groups[3].Value.Trim();
                        if (st != "")
                        {
                            st = st[1..^1];
                            foreach (var part in st.Split(";"))
                            {
                                var k = part.Split(" to ");
                                var set = new HashSet<string>(k[1].Split(", "));
                                var list = new List<AttackTypes>();
                                foreach(var item in set)
                                {
                                    list.Add((AttackTypes)Enum.Parse(typeof(AttackTypes), item, true));
                                }
                                var w = k[0].Trim();
                                if (w == "immune")
                                {
                                    g.Immunities = list;
                                }
                                else if (w == "weak")
                                {
                                    g.Weaknesses = list;
                                }
                                else
                                {
                                    throw new Exception();
                                }
                            }
                        }
                        res.Add(g);
                    }
                    else
                    {
                        throw new Exception();
                    }

                }
            return res;
        }

    }
}
