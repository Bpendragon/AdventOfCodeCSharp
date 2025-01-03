using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2015
{

    [DayInfo(10, 2015, "")]
    class Day10 : ASolution
    {
        public Day10() : base()
        {

        }

        protected override object SolvePartOne()
        {
            string s = Input.Clone().ToString();

            foreach (int i in Enumerable.Range(0, 40)) s = SpeakAndSay(s);
            return s.Length.ToString();
        }

        protected override object SolvePartTwo()
        {
            string s = Input.Clone().ToString();

            foreach (int i in Enumerable.Range(0, 50)) s = SpeakAndSay(s);
            return s.Length;
        }

        private static string SpeakAndSay(string value)
        {
            StringBuilder sb = new();
            Queue<char> q = new(value);
            while (q.Count > 0)
            {
                char cur = q.Dequeue();
                int count = 1;
                while (q.Count > 0 && q.Peek() == cur)
                {
                    count++;
                    q.Dequeue();
                }
                sb.Append(count + cur.ToString());
            }

            return sb.ToString();
        }
    }
}
