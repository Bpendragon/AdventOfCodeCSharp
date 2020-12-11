using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace AdventOfCode.Solutions
{

    class SolutionCollector : IEnumerable<ASolution>
    {
        readonly IEnumerable<ASolution> Solutions;

        public SolutionCollector(int year, int[] days) => Solutions = LoadSolutions(year, days).ToArray();

        public ASolution GetSolution(int day) {
            try {
                return Solutions.Single(s => s.Day == day);
            }
            catch( InvalidOperationException ) {
                return null;
            }
        }

        public IEnumerator<ASolution> GetEnumerator() {
            return Solutions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        IEnumerable<ASolution> LoadSolutions(int year, int[] days) {
            if( days.Sum() == 0 ) {
                days = Enumerable.Range(1, 25).ToArray();
            }

            foreach( int day in days ) {
                Type solution = Type.GetType($"AdventOfCode.Solutions.Year{year}.Day{day.ToString("D2")}");
                if( solution != null ) {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    ASolution val = (ASolution)Activator.CreateInstance(solution);
                    sw.Stop();
                    val.ContructionTime = sw.ElapsedTicks;
                    yield return val;
                }
            }
        }
    }
}
