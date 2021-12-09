namespace AdventOfCode.Solutions.Year2015
{

    class Day11 : ASolution
    {
        string part1;
        public Day11() : base(11, 2015, "")
        {

        }

        protected override string SolvePartOne()
        {
            char[] pass = Input.ToCharArray();
            do
            {
                pass = Increment(pass, 1);
            } while (!IsValid(pass));
            part1 = pass.JoinAsStrings();
            return part1;
        }
        protected override string SolvePartTwo()
        {
            char[] pass = part1.ToCharArray();
            do
            {
                pass = Increment(pass, 1);
            } while (!IsValid(pass));
            return pass.JoinAsStrings();
        }
        private static bool IsValid(char[] pass)
        {
            //check 1
            bool contCheck = false;
            for(int i = 0; i < pass.Length - 2; i++)
            {
                if (pass[i] + 1 == pass[i + 1] && pass[i + 1] + 1 == pass[i + 2]) { contCheck = true; break; }
            }
            if (!contCheck) return false;

            int pairs = 0;
            for (int i = 0; i < pass.Length - 1; i++)
            {
                if (pass[i] == pass[i+1])
                {
                    if(i > 0)
                    {
                        if (pass[i] != pass[i - 1]) pairs++;
                    } else
                    {
                        pairs++;
                    }
                }
            }

            return pairs >= 2;
        }

        private char[] Increment(char[] pass, int index)
        {
            if (index > pass.Length) return pass;
            switch(pass[^index])
            {
                case 'z': 
                    pass[^index] = 'a';
                    return Increment(pass, index + 1);
                case 'h':
                    pass[^index] = 'k';
                    break;
                case 'k':
                    pass[^index] = 'm';
                    break;
                case 'n':
                    pass[^index] = 'p';
                    break;
                default: 
                    pass[^index]++;
                    break;
            }

            return pass;
        }


    }
}
