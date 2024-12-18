using System;

namespace AdventOfCode.Solutions.Year2015
{

    [DayInfo(22, 2015, "")]
    class Day22 : ASolution
    {
        //From Input
        const int bossH = 55;
        const int bossAt = 8;

        //From Description
        readonly int costMissile = 53;
        readonly int costDrain = 73;
        readonly int costPoison = 173;
        readonly int costShield = 113;
        readonly int costRecharge = 229;

        const int heroH = 50;
        const int heroMana = 500;
        int heroAr = 0;


        int boss;
        int hero;
        int mana;

        //available actions "NotEnough" indicates the player ran out of mana and is therefore dead.
        enum ActionType { NotEnough = -1, Missile, Drain, Poison, Shield, Recharge }

        readonly Random r = new(); //I've got a fast computer, let's just simulate a few hundred thousand runs and be done with it.


        public Day22() : base()
        {

        }

        protected override object SolvePartOne()
        {
            int bestRun = int.MaxValue;

            for (int i = 0; i < 3000000; i++)
            {
                if (SimulateFight(out int cost))
                {
                    bestRun = Math.Min(bestRun, cost);
                }
            }

            return bestRun;
        }

        protected override object SolvePartTwo()
        {
            int bestRun = int.MaxValue;

            for (int i = 0; i < 3000000; i++)
            {
                if (SimulateFight(out int cost, true))
                {
                    bestRun = Math.Min(bestRun, cost);
                }
            }

            return bestRun;
        }

        private ActionType ChooseAction(bool poisonValid, bool rechargeValid, bool shieldValid)
        {
            if (mana < costMissile) return ActionType.NotEnough;

            for (; ; ) //Need an infinite loop? Why not Zoidberg?
            {
                int next = r.Next(5);
                if (next == 0 && mana >= costMissile)
                {
                    return ActionType.Missile;
                }
                else if (next == 1 && mana >= costDrain)
                {
                    return ActionType.Drain;
                }
                else if (next == 2 && mana >= costPoison && poisonValid)
                {
                    return ActionType.Poison;
                }
                else if (next == 3 && mana >= costRecharge && rechargeValid)
                {
                    return ActionType.Recharge;
                }
                else if (next == 4 && mana >= costShield && shieldValid)
                {
                    return ActionType.Shield;
                }
            }
        }

        private bool SimulateFight(out int cost, bool HardMode = false) //Returns True if success, false if fail
        {
            bool turn = true;
            hero = heroH;
            boss = bossH;
            mana = heroMana;
            cost = 0;
            //effect timers
            int poison = 0;
            int recharge = 0;
            int shield = 0;

            while (true)
            {
                if (turn)
                {
                    if (HardMode) hero--;
                    if (hero <= 0)
                    {
                        return false;
                    }
                }

                if (poison > 0)
                {
                    poison--;
                    boss -= 3;
                }

                if (recharge > 0)
                {
                    recharge--;
                    mana += 101;
                }

                if (shield > 0)
                {
                    shield--;
                }
                if (shield == 0)
                {
                    heroAr = 0;
                }

                if (boss <= 0)
                {
                    return true;
                }

                if (hero <= 0)
                {
                    return false;
                }

                if (turn)
                {
                    // hard mode


                    ActionType type = ChooseAction((poison == 0), (recharge == 0), (shield == 0));
                    if (type == ActionType.NotEnough)
                    {
                        return false;
                    }

                    if (type == ActionType.Drain)
                    {
                        boss -= 2;
                        hero += 2;
                        cost += costDrain;
                        mana -= costDrain;
                    }
                    else if (type == ActionType.Missile)
                    {
                        boss -= 4;
                        cost += costMissile;
                        mana -= costMissile;
                    }
                    else if (type == ActionType.Poison)
                    {
                        poison = 6;
                        cost += costPoison;
                        mana -= costPoison;
                    }
                    else if (type == ActionType.Recharge)
                    {
                        recharge = 5;
                        cost += costRecharge;
                        mana -= costRecharge;
                    }
                    else if (type == ActionType.Shield)
                    {
                        shield = 6;
                        heroAr = 7;
                        cost += costShield;
                        mana -= costShield;
                    }
                }
                else
                {
                    hero -= Math.Max(1, bossAt - heroAr);
                }

                turn = !turn;
            }

        }
    }
}
