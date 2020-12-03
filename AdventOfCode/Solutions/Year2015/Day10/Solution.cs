using System;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2015
{

    class Day10 : ASolution
    {
        public Day10() : base(10, 2015, "")
        {

        }

        protected override string SolvePartOne()
        {
            var s = Input.Clone().ToString();

            foreach (int i in Enumerable.Range(0, 40)) s = SpeakAndSay(s);
            return s.Length.ToString() ;
        }

        protected override string SolvePartTwo()
        {
            var s = Input.Clone().ToString();

            foreach (int i in Enumerable.Range(0, 50)) s = SpeakAndSay(s);
            return s.Length.ToString();
        }

        private string SpeakAndSay(string value)
        {
            StringBuilder sb = new StringBuilder();
            Queue<char> q = new Queue<char>(value);
            while(q.Count > 0)
            {
                char cur = q.Dequeue();
                int count = 1;
                while(q.Count > 0 && q.Peek() == cur)
                {
                    count++;
                    q.Dequeue();
                }
                sb.Append(count + cur.ToString());
            }

            return sb.ToString(); ;    
        }
    }
}