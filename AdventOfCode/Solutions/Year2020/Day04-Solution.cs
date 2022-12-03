using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2020
{

    class Day04 : ASolution
    {
        readonly List<string> passports;
        readonly List<string> semiValid;
        public Day04() : base(04, 2020, "Passport Processing")
        {
            passports = new List<String>(Input.Split("\n\n"));
            semiValid = new List<string>();
        }

        protected override object SolvePartOne()
        {
            int valid = 0;
            string[] requiredItems = new string[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};
            foreach(string passport in passports)
            {
                bool validB = true;
                foreach(string item in requiredItems)
                {
                    if (!passport.Contains(item)) validB = false;
                }

                if (validB)
                {
                    valid++;
                    semiValid.Add(passport);
                }

            }
            return valid;
        }

        protected override object SolvePartTwo()
        {
            int valid = 0;
            foreach(string passport in semiValid)
            {
                string[] tokens = passport.Split();
                bool isValid = true;

                foreach(string token in tokens)
                {
                    if (!isValid) break;
                    string[] t = token.Split(':');
                    int value;
                    switch(t[0])
                    {
                        case "byr":
                            value = int.Parse(t[1]);
                            if (value < 1920 || value > 2002) isValid = false;
                            break;
                        case "iyr":
                            value = int.Parse(t[1]);
                            if (value < 2010 || value > 2020) isValid = false;
                            break;
                        case "eyr":
                            value = int.Parse(t[1]);
                            if (value < 2020 || value > 2030) isValid = false;
                            break;
                        case "hgt":
                            string sub = t[1][^2..];
                            int hgt;

                            if(sub == "in")
                            {
                                if(!int.TryParse(t[1].TrimEnd(new char[] {'i','n'}), out hgt)) isValid = false;
                                if (hgt < 59 || hgt > 76) isValid = false;
                            } else if(sub == "cm")
                            {
                                if (!int.TryParse(t[1].TrimEnd(new char[] { 'c', 'm' }), out hgt)) isValid = false;
                                if (hgt < 150 || hgt > 193) isValid = false;
                            } else
                            {
                                isValid = false;
                            }
                            
                            break;
                        case "hcl":
                            if (!Regex.IsMatch(t[1], "^#[0-9a-f]{6}$")) isValid = false;
                            break;
                        case "ecl":
                            if (t[1].Length != 3) isValid = false;
                            else if (!"amb blu brn gry grn hzl oth".Contains(t[1])) isValid = false;
                            break;
                        case "pid":
                            if (t[1].Length != 9) isValid = false;
                            if (!int.TryParse(t[1], out int l)) isValid = false;
                            break;
                    }
                    
                }
                if (isValid) valid++;
            }

            return valid;
        }
    }
}
