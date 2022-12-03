using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    class Day16 : ASolution
    {
        readonly List<TicketField> ValidFields = new();
        readonly List<string> tickets;
        readonly List<string> validTickets;
        readonly string myTicket;
        public Day16() : base(16, 2020, "Ticket Translation")
        {
            var firstSplit = Input.Split("\n\n");

            foreach (var l in firstSplit[0].SplitByNewline())
            {
                var ticketField = new TicketField();
                var secondsplit = l.Split(':');
                ticketField.name = secondsplit[0];

                var thirdSplit = secondsplit[1].Split("- or".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                ticketField.lower = int.Parse(thirdSplit[0]);
                ticketField.upper = int.Parse(thirdSplit[1]);
                ticketField.lower2 = int.Parse(thirdSplit[2]);
                ticketField.upper2 = int.Parse(thirdSplit[3]);


                ValidFields.Add(ticketField);

            }

            myTicket = firstSplit[1].Split('\n')[1];

            tickets = new List<string>(firstSplit[2].SplitByNewline());
            tickets.RemoveAt(0);
            validTickets = new List<string>(tickets);
        }

        protected override object SolvePartOne()
        {
            int ticketScanningErrorRate = 0;

            foreach (var ticket in tickets)
            {

                foreach (int field in ticket.ToIntList(","))
                {
                    bool isValid = false;
                    foreach (var v in ValidFields)
                    {
                        if ((v.lower <= field && field <= v.upper) || (v.lower2 <= field && field <= v.upper2))
                        {
                            isValid = true;
                            break;
                        }
                    }
                    if (!isValid)
                    {
                        ticketScanningErrorRate += field;
                        validTickets.Remove(ticket);
                    }
                }
            }

            return ticketScanningErrorRate;
        }



        protected override object SolvePartTwo()
        {
            Dictionary<int, string> KnownFields = new();
            Dictionary<(int ticketPosition, string name), int> TicketsThatMatch = new();

            int t = 0;
            while (t < validTickets.Count) //only go until we we have to
            {
                int[] tFields = validTickets[t].ToIntList(",").ToArray();

                for (int i = 0; i < tFields.Length; i++)
                {
                    var field = tFields[i];
                    for (int j = 0; j < ValidFields.Count; j++)
                    {
                        var v = ValidFields[j];
                        if (((v.lower <= field && field <= v.upper) || (v.lower2 <= field && field <= v.upper2)))
                        {
                            if (!TicketsThatMatch.ContainsKey((i, v.name))) TicketsThatMatch[(i, v.name)] = 1;
                            else TicketsThatMatch[(i, v.name)]++;
                        }
                    }
                }
                t++;
            }

            while (KnownFields.Count < ValidFields.Count)
            {
                for (int i = 0; i < ValidFields.Count; i++)
                {
                    var ValidAtPosition = TicketsThatMatch.Where(x => x.Key.ticketPosition == i).ToList();

                    if (ValidAtPosition.Count(x => x.Value == validTickets.Count) == 1)
                    {
                        var tmp = ValidAtPosition.First(x => x.Value == validTickets.Count);
                        KnownFields[tmp.Key.ticketPosition] = tmp.Key.name;

                        foreach (var k in TicketsThatMatch.Keys)
                        {
                            if (k.name == tmp.Key.name) TicketsThatMatch.Remove(k);
                        }

                    }
                }
            }

            int[] myTicketFields = myTicket.ToIntList(",").ToArray();

            long departureFields = 1;

            foreach (var f in KnownFields)
            {
                if (f.Value.Contains("departure")) departureFields *= myTicketFields[f.Key];
            }

            return departureFields;
        }


        internal class TicketField
        {
            public string name;
            public int lower;
            public int upper;
            public int lower2;
            public int upper2;
        }
    }
}
