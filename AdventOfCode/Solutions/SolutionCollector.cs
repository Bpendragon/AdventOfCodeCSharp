using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
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

        static IEnumerable<ASolution> LoadSolutions(int year, int[] days) {
            if( days.Sum() == 0 ) {
                days = Enumerable.Range(1, 25).ToArray();
            }
            //Type solution = Type.GetType($"AdventOfCode.Solutions.Year{year}.Day{day:D2}");
            foreach (int day in days)
            {
                Type solution = Type.GetType($"AdventOfCode.Solutions.Year{year}.Day{day:D2}");
                if (solution != null)
                {
                    Stopwatch clock = new();
                    clock.Reset();
                    var ctor = Expression.Lambda<Func<ASolution>>(Expression.New(solution.GetConstructor(Type.EmptyTypes)))
                        .Compile();
                    ASolution result;
                    try
                    {
                        clock.Start();
                        result = ctor();
                        clock.Stop();
                        result.ContructionTime = clock.ElapsedTicks;
                        
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError($"Caught Exception:\r\n{ex}");
                        throw;
                    }
                    yield return result;
                }
            }



            //
            //return Assembly.GetExecutingAssembly()
            //    .GetTypes()
            //    .Where(type => type.BaseType == typeof(ASolution))
            //    //.Select(type => (type, info: type.GetCustomAttribute<DayInfoAttribute>()))
            //    //.Where(solution => {
            //    //    if (solution.info is null) return false;
            //    //    return solution.info.Year == year && days.Contains(solution.info.Day);
            //    //})
            //    //.OrderBy(solution => solution.info.Year)
            //    //.ThenBy(solution => solution.info.Day)
            //    //.ThenBy(solution => solution.type.Name)
            //    .Select(solution => {
            //        clock.Reset();
            //        var ctor = Expression.Lambda<Func<ASolution>>(Expression.New(solution.type.GetConstructor(Type.EmptyTypes)))
            //            .Compile();
            //        try
            //        {
            //            clock.Start();
            //            var result = ctor();
            //            clock.Stop();
            //            result.ContructionTime = clock.ElapsedTicks;
            //            return result;
            //        }
            //        catch (Exception ex)
            //        {
            //            Trace.TraceError($"Caught Exception:\r\n{ex}");
            //            throw;
            //        }
            //    })
            //    .ToArray();
        }
    }
}
