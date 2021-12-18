using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace AdventOfCode.Solutions
{

    abstract class ASolution
    {
        long _part1Time, _part2Time;
        readonly Lazy<string> _input, _part1, _part2;

        public int Day { get; }
        public int Year { get; }
        public string Title { get; }
        public string DebugInput { get; set; }
        public string Input => string.IsNullOrEmpty(_input.Value) ? null : _input.Value;
        public string Part1 => string.IsNullOrEmpty(_part1.Value) ? "" : _part1.Value;
        public string Part2 => string.IsNullOrEmpty(_part2.Value) ? "" : _part2.Value;
        public long Part1Ticks => _part1Time;
        public long Part2Ticks => _part2Time;
        public long ContructionTime { get; set; }
        protected bool UseDebugInput { get; set; }

        private protected ASolution(int day, int year, string title) {
            Day = day;
            Year = year;
            Title = title;
            _input = new Lazy<string>(LoadInput);
            _part1 = new Lazy<string>(() => SafelySolve(SolvePartOne, out _part1Time));
            _part2 = new Lazy<string>(() => SafelySolve(SolvePartTwo, out _part2Time));
        }

        public void Solve(int part = 0) {
            if( Input == null ) return;

            bool doOutput = false;
            StringBuilder output = new($"--- Day {Day}: {Title} --- \n");
            if( DebugInput != null ) {
                output.Append($"!!! DebugInput used !!!\n");
            }

            output.Append($"Ctor in {TimeSpan.FromTicks(ContructionTime).TotalMilliseconds}ms\n");
            if( part != 2 ) {
                if( Part1 != "" ) {
                    output.Append($"Part 1:\tin {TimeSpan.FromTicks(_part1Time).TotalMilliseconds}ms\n{Part1}\n\n");
                    doOutput = true;
                }
                else {
                    output.Append("Part 1: Unsolved\n");
                    if( part == 1 ) doOutput = true;
                }
            }
            if( part != 1 ) {
                if( Part2 != "" ) {
                    output.Append($"Part 2:\tin {TimeSpan.FromTicks(_part2Time).TotalMilliseconds}ms\n{Part2}\n\n");
                    doOutput = true;
                }
                else {
                    output.Append("Part 2: Unsolved\n");
                    if( part == 2 ) doOutput = true;
                }
            }

            if( doOutput ) {
                Trace.Write(output.ToString());
                Console.Write(output.ToString());
            }
        }

        string LoadInput() {
            string DEBUGINPUT_FILEPATH = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"../../../Solutions/Year{Year}/Day{Day:D2}-debugInput"));
            string DEBUGINPUT_FILEPATH_ALT = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"../../../Solutions/Year{Year}/Day{Day:D2}-debugInput.txt"));
            string INPUT_FILEPATH = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"../../../Solutions/Year{Year}/Day{Day:D2}-input"));
            string INPUT_FILEPATH_ALT = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, $"../../../Solutions/Year{Year}/Day{Day:D2}-input.txt"));
            string INPUT_URL = $"https://adventofcode.com/{Year}/day/{Day}/input";
            string input = "";

            if( UseDebugInput && File.Exists(DEBUGINPUT_FILEPATH) && new FileInfo(DEBUGINPUT_FILEPATH).Length > 0 ) {
                input = DebugInput = File.ReadAllText(DEBUGINPUT_FILEPATH);
            }
            else if (UseDebugInput && File.Exists(DEBUGINPUT_FILEPATH_ALT) && new FileInfo(DEBUGINPUT_FILEPATH_ALT).Length > 0)
            {
                input = DebugInput = File.ReadAllText(DEBUGINPUT_FILEPATH_ALT);
            }
            else if( File.Exists(INPUT_FILEPATH) && new FileInfo(INPUT_FILEPATH).Length > 0 ) {
                input = File.ReadAllText(INPUT_FILEPATH);
            }
            else if (File.Exists(INPUT_FILEPATH_ALT) && new FileInfo(INPUT_FILEPATH_ALT).Length > 0)
            {
                input = File.ReadAllText(INPUT_FILEPATH_ALT);
            }
            else {
                try {
                    DateTime CURRENT_EST = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Utc).AddHours(-5);
                    if( CURRENT_EST < new DateTime(Year, 12, Day) ) throw new InvalidOperationException();

                    using WebClient client = new();
                    client.Headers.Add(HttpRequestHeader.Cookie, Program.Config.Cookie);
                    input = client.DownloadString(INPUT_URL).TrimEnd();
                    File.WriteAllText(INPUT_FILEPATH, input);
                }
                catch( WebException e ) {
                    HttpStatusCode statusCode = ((HttpWebResponse)e.Response).StatusCode;
                    if( statusCode == HttpStatusCode.BadRequest ) {
                        Console.WriteLine($"Day {Day}: Error code 400 when attempting to retrieve puzzle input through the web client. Your session cookie is probably not recognized.");
                    }
                    else if( statusCode == HttpStatusCode.NotFound ) {
                        Console.WriteLine($"Day {Day}: Error code 404 when attempting to retrieve puzzle input through the web client. The puzzle is probably not available yet.");
                    }
                    else {
                        Console.WriteLine(e.ToString());
                    }
                }
                catch( InvalidOperationException ) {
                    Console.WriteLine($"Day {Day}: Cannot fetch puzzle input before given date (Eastern Standard Time).");
                }
            }
            return input;
        }

        protected abstract string SolvePartOne();
        protected abstract string SolvePartTwo();

        private static string SafelySolve(Func<string> partSolver, out long timeTaken) {
            Stopwatch clock = new(); clock.Start();
            string solution = string.Empty;
            try {
                solution = partSolver();
            }
            catch( Exception ex ) {
                Trace.TraceError($"Caught Exception:\r\n{ex}");
                Debugger.Break();
            }
            clock.Stop();
            timeTaken = clock.ElapsedTicks;
            return solution;
        }
    }
}
