using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2015
{

    [DayInfo(07, 2015, "Some Assembly Required")]
    class Day07 : ASolution
    {
        private readonly List<string> Lines;
        private readonly Dictionary<string, Gate> circuit = new();
        public Day07() : base()
        {
            Lines = new List<string>(Input.SplitByNewline());
            foreach (string line in Lines)
            {
                string[] tokens = line.Split();
                string gateName = tokens[^1];
                Gate _gate;
                if (!circuit.ContainsKey(gateName))
                {
                    _gate = new Gate(gateName);
                    circuit[gateName] = _gate;
                }
                else
                {
                    _gate = circuit[gateName];
                }
                if (tokens[0].Equals("NOT"))
                {
                    if (circuit.TryGetValue(tokens[1], out Gate val))
                    {
                        _gate.Inputs.Add(val);
                    }
                    else
                    {
                        Gate _gate2 = new(tokens[1]);
                        circuit[_gate2.Name] = _gate2;
                        _gate.Inputs.Add(_gate2);
                    }
                    _gate.Type = GateTypes.Not;
                }
                else if (tokens[1].Equals("->")) //special case, so kept out of switch statement
                {
                    _gate.Type = GateTypes.Direct;
                    if (ushort.TryParse(tokens[0], out ushort val))
                    {
                        _gate.Inputs.Add(new Gate()
                        {
                            Output = val,
                            Type = GateTypes.Direct
                        });
                        _gate.Output = val;
                    }
                    else
                    {
                        if (circuit.TryGetValue(tokens[0], out Gate val2))
                        {
                            _gate.Inputs.Add(val2);
                        }
                        else
                        {
                            Gate _gate2 = new(tokens[0]);
                            circuit[_gate2.Name] = _gate2;
                            _gate.Inputs.Add(_gate2);
                        }
                    }
                }
                else
                {
                    switch (tokens[1])
                    {
                        case "OR":
                            _gate.Type = GateTypes.Or;
                            break;
                        case "AND":
                            _gate.Type = GateTypes.And;
                            break;
                        case "LSHIFT":
                            _gate.Type = GateTypes.LShift;
                            break;
                        case "RSHIFT":
                            _gate.Type = GateTypes.RShift;
                            break;
                    }

                    if (circuit.TryGetValue(tokens[0], out Gate value))
                    {
                        _gate.Inputs.Add(value);
                    }
                    else
                    {
                        if (ushort.TryParse(tokens[0], out ushort val))
                        {
                            Gate _gateD = new()
                            {
                                Output = val,
                                Type = GateTypes.Direct
                            };

                            _gate.Inputs.Add(_gateD);
                        }
                        else
                        {
                            Gate _gate2 = new(tokens[0]);
                            circuit[_gate2.Name] = _gate2;
                            _gate.Inputs.Add(_gate2);
                        }
                    }

                    if (_gate.Type == GateTypes.LShift || _gate.Type == GateTypes.RShift)
                    {
                        _gate.Inputs.Add(new Gate()
                        {
                            Type = GateTypes.Direct,
                            Output = ushort.Parse(tokens[2])
                        });
                    }
                    else
                    {
                        if (circuit.TryGetValue(tokens[2], out Gate value2))
                        {
                            _gate.Inputs.Add(value2);
                        }
                        else
                        {
                            Gate _gate2 = new(tokens[2]);
                            circuit[_gate2.Name] = _gate2;
                            _gate.Inputs.Add(_gate2);
                        }
                    }
                }
                UpdateCircuit();
            }
            UpdateCircuit();
        }

        private void UpdateCircuit()
        {
            int changes;
            do
            {
                changes = 0;
                foreach (string key in circuit.Keys)
                {
                    Gate gateToCheck = circuit[key];
                    if (!gateToCheck.Output.HasValue && gateToCheck.Type != GateTypes.Unknown)
                    {
                        bool ready = true;
                        foreach (Gate i in gateToCheck.Inputs)
                        {
                            if (!i.Output.HasValue)
                            {
                                ready = false;
                                break;
                            }
                        }
                        if (ready)
                        {
                            switch (gateToCheck.Type)
                            {
                                case GateTypes.And:
                                    gateToCheck.Output = (ushort)(gateToCheck.Inputs[0].Output & gateToCheck.Inputs[1].Output);
                                    break;
                                case GateTypes.Or:
                                    gateToCheck.Output = (ushort)(gateToCheck.Inputs[0].Output | gateToCheck.Inputs[1].Output);
                                    break;
                                case GateTypes.LShift:
                                    gateToCheck.Output = (ushort)(gateToCheck.Inputs[0].Output << gateToCheck.Inputs[1].Output);
                                    break;
                                case GateTypes.RShift:
                                    gateToCheck.Output = (ushort)(gateToCheck.Inputs[0].Output >> gateToCheck.Inputs[1].Output);
                                    break;
                                case GateTypes.Direct:
                                    gateToCheck.Output = gateToCheck.Inputs[0].Output;
                                    break;
                                case GateTypes.Not:
                                    int tmp = (ushort)gateToCheck.Inputs[0].Output;
                                    tmp = ~tmp;
                                    tmp &= 0b0000_0000_0000_0000_1111_1111_1111_1111;
                                    gateToCheck.Output = (ushort)tmp;
                                    break;
                            }
                            changes++;
                        }
                    }
                }
            } while (changes > 0);
        }

        private void ResetCircuit()
        {
            foreach (string key in circuit.Keys)
            {
                circuit[key].Output = null;
            }
        }

        protected override object SolvePartOne()
        {
            return circuit["a"].Output;
        }

        protected override object SolvePartTwo()
        {
            ushort? tmp = circuit["a"].Output;
            ResetCircuit();
            circuit["b"].Output = tmp;
            UpdateCircuit();
            return circuit["a"].Output;
        }
    }

    class Gate
    {
        public Gate() { }

        public Gate(string gateName)
        {
            Name = gateName;
        }

        public string Name { get; set; } //the name of the output wire from the gate
        public List<Gate> Inputs { get; set; } = new List<Gate>();
        public ushort? Output { get; set; } = null;
        public GateTypes Type { get; set; } = GateTypes.Unknown;
    }

    enum GateTypes
    {
        Direct,
        Not,
        Or,
        And,
        LShift,
        RShift,
        Unknown
    }
}
