using System.Collections.Generic;
using System.Linq;
using System.Text;

using AdventOfCode.Solutions;

namespace AdventOfCode.UserClasses
{
    class KnotHash
    {
        private int CurPosition = 0;
        private int SkipSize = 0;
        private List<int> Sequence;
        private List<int> knot = new List<int>(256);
        public string Hash { get; private set; }


        public KnotHash(List<int> Sequence) //only kept public for 2015 Day 10, part 1
        {
            this.Sequence = Sequence;
            ResetKnot();

        }

        public KnotHash(string ToHash)
        {
            ResetKnot();
            GenerateSequence(ToHash);
            CalculateHash();
        }

        public KnotHash()
        {
            ResetKnot();
        }

        private void ResetKnot()
        {
            knot = new List<int>();
            foreach (int i in Enumerable.Range(0, 256)) knot.Add(i);
        }

        private void GenerateSequence(string ToHash)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(ToHash);
            Sequence = new List<int>();
            foreach (byte b in bytes) Sequence.Add(b);
            Sequence.AddRange(new int[] { 17, 31, 73, 47, 23 });
        }

        public List<int> Round(List<int> input)
        {
            List<int> res = new List<int>(input);
            foreach(int length in Sequence)
            {
                //Get Substring and reverse
                List<int> sub = new List<int>(res);
                if(CurPosition + length >= res.Count)
                {
                    sub.AddRange(res);
                    sub.Reverse(CurPosition, length);
                    for(int i = res.Count; i < CurPosition + length; i++)
                    {
                        sub[i - res.Count] = sub[i];
                    }
                    sub = sub.GetRange(0, 256);
                } else
                {
                    sub.Reverse(CurPosition, length);     
                }

                res = new List<int>(sub);

                //move curPosition
                CurPosition = (CurPosition + length + SkipSize) % knot.Count;
                //Increment SkipSize
                SkipSize++;
            }

            return res;
        }

        public string CalculateHash(string ToHash)
        {
            GenerateSequence(ToHash);
            ResetKnot();
            return CalculateHash();
        }

        private string CalculateHash()
        {
            CurPosition = 0;
            SkipSize = 0;
            //get sparse
            for(int i = 0; i < 64; i++)
            {
                knot = new List<int>(Round(knot));
            }

            //condense
            var groups = knot.Split(16).ToList();

            int[] hashVals = new int[16];

            for(int i = 0; i < 16; i++)
            {
                hashVals[i] = groups[i].Aggregate((a, b) => a ^ b);
            }

            StringBuilder sb = new StringBuilder();

            foreach(int i in hashVals)
            {
                sb.Append(i.ToString("X2"));
            }
            Hash = sb.ToString().ToLower();
            return Hash;
        }

        public override string ToString()
        {
            return Hash;
        }
    }
}
