using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Year2015
{

    class Day04 : ASolution
    {
        long firstRes = 0;
        long secondRes = 0;

        public Day04() : base(04, 2015, "")
        {

        }

        protected override string SolvePartOne()
        {
            return firstRes.ToString();
        }

        protected override string SolvePartTwo()
        {
            return secondRes.ToString();
        }

        public async Task<string> CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
