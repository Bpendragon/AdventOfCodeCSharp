using System.Collections.Generic;
using System.Data;
using System.Linq;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(20, 2023, "Pulse Propagation")]
    class Day20 : ASolution
    {
        static Dictionary<string, PulseModule> modules = new();
        static Queue<string> processOrder = new();
        static Dictionary<string, long> rxConjunctions = new();
        static string rxFeeder;

        public Day20() : base()
        {
            foreach(var n in Input.ExtractWords().Distinct())
            {
                PulseModule tmp = new PulseModule()
                {
                    name = n
                };

                modules[n] = tmp;
            }
            foreach(var l in Input.SplitByNewline())
            {
                var relModules = l.ExtractWords().ToList();
                string curMod = relModules[0];
                modules[curMod].Type = (l[0]) switch
                {
                    '&' => PulseModuleType.Conjunction,
                    '%' => PulseModuleType.FlipFlop,
                    _ => PulseModuleType.Broadcast
                };

                foreach(var m in relModules[1..])
                {
                    modules[curMod].outputs.Add(m);
                    modules[m].lastReceivedPulses[curMod] = false;
                    if (m == "rx") rxFeeder = curMod;
                }
            }

            PulseModule button = new()
            {
                name = "button",
                Type = PulseModuleType.Broadcast,
                outputs = new() { "broadcaster" }
            };

            modules["button"] = button;

            foreach(var m in modules.Values.Where(a => a.outputs.Contains(rxFeeder)))
            {
                rxConjunctions[m.name] = 0;
            }
        }

        protected override object SolvePartOne()
        {
            long lowPulses = 0;
            long highPulses = 0;

            for(int i = 0; i < 1000; i++)
            {
                modules["button"].incomingPulses.Enqueue(("finger", false));
                processOrder.Enqueue("button");

                while(processOrder.TryDequeue(out string nextPulseTarget))
                {
                    (long pulsesSent, bool pulseVal) = modules[nextPulseTarget].ProcessPulse();

                    if (pulseVal) highPulses += pulsesSent;
                    else lowPulses += pulsesSent;
                }
            }

            return (highPulses * lowPulses);
        }

        protected override object SolvePartTwo()
        {
            foreach (var m in modules.Values) m.Reset();

            for (int i = 1; ; i++)
            {
                modules["button"].incomingPulses.Enqueue(("finger", false));
                processOrder.Enqueue("button");

                while (processOrder.TryDequeue(out string name))
                {
                    (_, bool pulseVal)  = modules[name].ProcessPulse();
                    if (rxConjunctions.ContainsKey(name) && rxConjunctions[name] == 0 && pulseVal) rxConjunctions[name] = i;
                }

                if (rxConjunctions.Values.All(a => a != 0)) break;
            }

            return rxConjunctions.Values.Aggregate(1L, (a, b) => a = FindLCM(a, b));
        }

        private class PulseModule
        {
            public string name;
            public PulseModuleType Type = PulseModuleType.Unknown;
            public bool flipFlopState = false;
            public Dictionary<string, bool> lastReceivedPulses = new();
            public Queue<(string source, bool pulse)> incomingPulses = new();
            public List<string> outputs = new();

            public void Reset()
            {
                flipFlopState = false;
                foreach(var k in lastReceivedPulses.Keys)
                {
                    lastReceivedPulses[k] = false;
                }
            }

            public (long pulsesSent, bool pulseVal) ProcessPulse()
            {
                long pulsesSent = 0;
                bool pulseVal = false;

                if (incomingPulses.TryDequeue(out var p))
                {
                    (var src, var pulse) = p;
                    if (Type == PulseModuleType.FlipFlop)
                    {
                        if (!pulse)
                        { //FF only acts on low (false) pulse

                            flipFlopState = !flipFlopState;
                            foreach (var n in outputs)
                            {
                                modules[n].incomingPulses.Enqueue((name, flipFlopState));
                                processOrder.Enqueue(n);
                                pulsesSent++;
                            }

                            pulseVal = flipFlopState;
                        }
                    }
                    else if (Type == PulseModuleType.Conjunction)
                    {
                        lastReceivedPulses[src] = pulse;
                        pulseVal = lastReceivedPulses.Values.Any(a => !a);

                        foreach (var n in outputs)
                        {
                            modules[n].incomingPulses.Enqueue((name, pulseVal));
                            processOrder.Enqueue(n);
                            pulsesSent++;
                        }
                    }
                    else if (Type == PulseModuleType.Broadcast)
                    {
                        pulseVal = pulse;
                        foreach (var n in outputs)
                        {
                            modules[n].incomingPulses.Enqueue((name, pulseVal));
                            processOrder.Enqueue(n);
                            pulsesSent++;
                        }
                    }
                }
                return (pulsesSent, pulseVal);
            }
        }

        private enum PulseModuleType
        {
            FlipFlop,
            Conjunction,
            Broadcast,
            Unknown
        }
    }
}
