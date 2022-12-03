using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2015
{

    class Day07 : ASolution
    {
        private readonly List<string> Lines;
        private readonly Dictionary<string, Gate> circuit = new();
        public Day07() : base(07, 2015, "")
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
                    if (circuit.ContainsKey(tokens[1]))
                    {
                        _gate.Inputs.Add(circuit[tokens[1]]);
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
                        if (circuit.ContainsKey(tokens[0]))
                        {
                            _gate.Inputs.Add(circuit[tokens[0]]);
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

                    if (circuit.ContainsKey(tokens[0]))
                    {
                        _gate.Inputs.Add(circuit[tokens[0]]);
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
                        if (circuit.ContainsKey(tokens[2]))
                        {
                            _gate.Inputs.Add(circuit[tokens[2]]);
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
                                    gateToCheck.Output = (ushort)~gateToCheck.Inputs[0].Output;
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
            foreach(string key in circuit.Keys)
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