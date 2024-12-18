using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(11, 2022, "Monkey in the Middle")]
    class Day11 : ASolution
    {
        readonly List<Monkey> monkeys = new();
        private static List<Monkey> workingMonkeys;
        private static long p2Relief;
        public Day11() : base()
        {

            workingMonkeys ??= new();
            foreach (var m in Input.SplitByDoubleNewline())
            {
                Monkey monk = new();
                var monkDef = m.SplitByNewline();
                var items = monkDef[1].Split(':')[1].ToLongList(",");
                foreach (var item in items) monk.Items.AddLast(item);
                monk.IsMultiply = monkDef[2].Contains('*');

                if (long.TryParse(monkDef[2].Split(" ")[^1], out long val)) monk.OperationConst = val;
                else monk.OperationConst = -1;

                monk.TestDivisor = long.Parse(monkDef[3].Split(" ")[^1]);
                monk.TrueTarget = int.Parse(monkDef[4].Split(" ")[^1]);
                monk.FalseTarget = int.Parse(monkDef[5].Split(" ")[^1]);

                monkeys.Add(monk);
            }

            p2Relief = monkeys.Aggregate(1L, (a, b) => a * b.TestDivisor);
        }

        protected override object SolvePartOne()
        {
            workingMonkeys.Clear();
            foreach (var m in monkeys) workingMonkeys.Add(m.Clone());
            for (long i = 0; i < 20; i++)
            {
                foreach (var monkey in workingMonkeys)
                {
                    while (monkey.Items.Count > 0)
                    {
                        monkey.Inspect();
                        monkey.Relief();
                        monkey.Throw();
                    }
                }
            }

            return workingMonkeys.OrderByDescending(a => a.ItemsInspected).Take(2).Aggregate(1L, (a, b) => a * b.ItemsInspected);
        }

        protected override object SolvePartTwo()
        {
            workingMonkeys.Clear();
            foreach (var m in monkeys) workingMonkeys.Add(m.Clone());
            for (long i = 0; i < 10_000; i++)
            {
                foreach (var monkey in workingMonkeys)
                {
                    while (monkey.Items.Count > 0)
                    {
                        monkey.Inspect();
                        monkey.ReliefP2();
                        monkey.Throw();
                    }
                }
            }

            return workingMonkeys.OrderByDescending(a => a.ItemsInspected).Take(2).Aggregate(1L, (a, b) => a * b.ItemsInspected);
        }

        internal class Monkey
        {
            public long ItemsInspected { get; set; } = 0;
            public bool IsMultiply { get; set; } //False is an Addition
            public long OperationConst { get; set; } //-1 is self
            public long TestDivisor { get; set; }
            public LinkedList<long> Items { get; set; } = new();
            public int TrueTarget { get; set; }
            public int FalseTarget { get; set; }

            public void Inspect()
            {
                ItemsInspected++;
                long tmpVal = Items.First.Value;
                if (IsMultiply)
                {
                    if (OperationConst == -1) tmpVal *= tmpVal;
                    else tmpVal *= OperationConst;
                }
                else
                {
                    if (OperationConst == -1) tmpVal += tmpVal;
                    else tmpVal += OperationConst;
                }

                Items.First.Value = tmpVal;
            }

            public void Relief()
            {
                Items.First.Value /= 3;
            }

            public void ReliefP2()
            {
                Items.First.Value %= p2Relief;
            }

            public void Throw()
            {
                var val = Items.First.Value;
                Items.RemoveFirst();

                if (val % TestDivisor == 0) workingMonkeys[TrueTarget].Items.AddLast(val);
                else workingMonkeys[FalseTarget].Items.AddLast(val);
            }

            public Monkey Clone()
            {
                var tmpM = new Monkey()
                {
                    ItemsInspected = ItemsInspected,
                    IsMultiply = IsMultiply,
                    OperationConst = OperationConst,
                    TestDivisor = TestDivisor,
                    Items = new(),
                    TrueTarget = TrueTarget,
                    FalseTarget = FalseTarget
                };

                var cur = Items.First;
                while (cur != null)
                {
                    tmpM.Items.AddLast(cur.Value);
                    cur = cur.Next;
                }

                return tmpM;
            }
        }
    }
}
