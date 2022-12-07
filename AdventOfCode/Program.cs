using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;

using AdventOfCode.Solutions;

namespace AdventOfCode
{

    class Program
    {
        internal static Config Config = Config.Get("config.json");
        internal static readonly HttpClient Http = new();

        static void Main(string[] args) {
            var productVal = new ProductInfoHeaderValue(".NET", Environment.Version.ToString());
            var commentVal = new ProductInfoHeaderValue("(+Bpen's Advent of Code Template; https://github.com/Bpendragon/AdventOfCodeBase; github@bpendragon.horse)");
            Http.DefaultRequestHeaders.UserAgent.Add(productVal);
            Http.DefaultRequestHeaders.UserAgent.Add(commentVal);
            Http.DefaultRequestHeaders.Add("Cookie", Config.Cookie);
            Http.DefaultRequestHeaders.Add("From", "github@bpendragon.horse");

            long total = 0;
            foreach (ASolution solution in new SolutionCollector(Config.Year, Config.Days))
            {
                solution.Solve();
                total += solution.ContructionTime + solution.Part1Ticks + solution.Part2Ticks;
            }
            string output = $"Total time taken: {TimeSpan.FromTicks(total).TotalMilliseconds}ms | {total} ticks | {TimeSpan.FromTicks(total)}";
            Trace.WriteLine(output);
            Console.WriteLine(output);
            Console.Write("Press Enter to close...");
            Console.ReadLine();
        }
    }
}
