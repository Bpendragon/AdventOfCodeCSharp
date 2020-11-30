using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2015
{

    class Day08 : ASolution
    {
        int totalCount = 0;
        int asciiChars = 0;
        int encodedChars = 0;
        List<string> lines;
        

        public Day08() : base(08, 2015, "")
        {
            lines = new List<string>(Input.SplitByNewline());
            foreach(var line in lines)
            {
                totalCount += line.Length;
                encodedChars += 6;

                for (int i = 1; i < line.Length - 1; i++)
                {
                    if(line[i] == '\\')
                    {
                       switch (line[i+1])
                        {
                            case '\\':
                            case '\"':
                                i++;
                                encodedChars += 4;
                                break;
                            case 'x':
                                encodedChars+=5;
                                i += 3;
                                break;
                        }     
                    } else
                    {
                        encodedChars++;
                    }

                    asciiChars++;
                }

            }
        }

        protected override string SolvePartOne()
        {
            return (totalCount - asciiChars).ToString() ;
        }

        protected override string SolvePartTwo()
        {
            return (encodedChars - totalCount).ToString();
        }
    }
}