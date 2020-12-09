using System;
using System.Diagnostics;
using AdventOfCode.Solutions;

namespace AdventOfCode
{

    class Program
    {

        public static Config Config = Config.Get("config.json");
        static SolutionCollector Solutions = new SolutionCollector(Config.Year, Config.Days);

        static void Main(string[] args) {
            long total = 0;
            foreach( ASolution solution in Solutions ) {
                solution.Solve();
                total += solution.ContructionTime + solution.Part1Ticks + solution.Part2Ticks;
            }
            string output = $"Total time taken: {TimeSpan.FromTicks(total).TotalMilliseconds}ms | {total} ticks | {TimeSpan.FromTicks(total)}";
            Trace.WriteLine(output);
            Console.WriteLine(output);
        }
    }
}
