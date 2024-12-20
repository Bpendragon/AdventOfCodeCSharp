using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2017
{

    [DayInfo(17, 2017, "")]
    class Day17 : ASolution
    {
        //from input
        const int Steps = 345;
        public Day17() : base()
        {

        }

        protected override object SolvePartOne()
        {
            List<int> buffer = new() { 0 };
            int curPos = 0;
            for (int i = 1; i <= 2017; i++)
            {
                curPos = (curPos + Steps) % buffer.Count;
                buffer.Insert(curPos + 1, i);
                curPos = (curPos + 1) % buffer.Count;
            }

            int index = buffer.IndexOf(2017);

            return buffer[(index + 1) % buffer.Count];
        }

        protected override object SolvePartTwo()
        {
            long total = 50_000_000;

            long pos = 0;
            long target = 0;
            for (int i = 1; i < total; i++)
            {
                pos = ((pos + Steps) % i) + 1;
                if (pos == 1)
                {
                    target = i;
                }
            }
            return target;

            //Don't Actually run this please, it took 22 minutes and almost 5GB of RAM

            /*
            LinkedList<int> buffer = new LinkedList<int>();
            buffer.AddFirst(0);
            LinkedListNode<int> current = buffer.First;
            for (int i = 1; i <= 50000000; i++)
            {
                foreach(int _ in Enumerable.Range(0, Steps)) current = current.Next ?? current.List.First;
                buffer.AddAfter(current, i);
                current = current.Next ?? current.List.First;
                if (i % 1000 == 0) Utilities.WriteLine(i);
            }

            current = buffer.Find(0);
            current = current.Next ?? current.List.First;

            return current.Value;
            */
        }
    }
}
