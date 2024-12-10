using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(09, 2024, "Disk Fragmenter")]
    class Day09 : ASolution
    {
        Dictionary<long, AmphiFile> filesp1 = new();
        Dictionary<long, AmphiFile> filesp2 = new();
        static Queue<long> emptyBlocks = new();
        static Stack<long> fileBlocks = new();
        List<(long start, long length)> emptyRanges = new();
        Dictionary<long, PriorityQueue<long, long>> emptyRangeQueues = new();
        long maxFileID;

        public Day09() : base()
        {
            var asLongs = Input.ToLongList();
            long pointer = 0;
            long curFileId = 0;
            foreach (var i in Enumerable.Range(0, 10)) emptyRangeQueues[i] = new();
            for(int i = 0; i < asLongs.Count; i++)
            {
                if (i % 2 == 0)
                {
                    AmphiFile newFile = new(curFileId);
                    AmphiFile newFile2 = new(curFileId);

                    for (int j = 0; j < asLongs[i]; j++)
                    {
                        newFile.blocks.Add(pointer);
                        newFile2.blocks.Add(pointer);
                        pointer++;
                        fileBlocks.Push(curFileId);
                    }
                    filesp1[curFileId] = newFile;
                    filesp2[curFileId] = newFile2;
                    curFileId++;
                } 
                else
                {
                    if (asLongs[i] > 0)
                    {
                        emptyRanges.Add((pointer, asLongs[i]));
                        emptyRangeQueues[asLongs[i]].Enqueue(pointer, pointer);
                        for (int j = 0; j < asLongs[i]; j++)
                        {
                            emptyBlocks.Enqueue(pointer);
                            pointer++;
                        }
                    }
                }
            }

            maxFileID = curFileId - 1;
        }

        protected override object SolvePartOne()
        {
            bool isMoved;
            do
            {
                var fb = fileBlocks.Pop();
                isMoved = filesp1[fb].MoveBlock(emptyBlocks.Dequeue());
            } while (isMoved);

            return filesp1.Values.Sum(a => a.checksum);
        }

        protected override object SolvePartTwo()
        {
            for(long i = maxFileID; i > 0; i--)
            {
                if (!emptyRangeQueues.Any(a => a.Value.Count > 0)) break;
                var reqSpace = filesp2[i].size;

                List<(int index, long start)> testPoint = new();
                for (int j = reqSpace; j <= 9; j++)
                {
                    if (emptyRangeQueues[j].TryPeek(out long start, out _))
                    {

                        if (start > filesp2[i].start)
                        {
                            emptyRangeQueues[j].Clear();
                            continue;
                        }
                        testPoint.Add((j, start));
                    }
                }

                if (testPoint.Count > 0)
                {
                    (int j, long start) = testPoint.MinBy(a => a.start);
                    emptyRangeQueues[j].Dequeue();
                    filesp2[i].moveFile(start);
                    if (j - reqSpace > 0)
                    {
                        emptyRangeQueues[j - reqSpace].Enqueue(start + reqSpace, start + reqSpace);
                    }
                }
            }
            return filesp2.Values.Sum(a => a.checksum);
        }

        private class AmphiFile
        {
            public long id { get; set; }
            public List<long> blocks = new();

            public long checksum => blocks.Sum(a => a * id);
            public long start => blocks[0];
            public int size => blocks.Count;

            public AmphiFile() { }
            public AmphiFile(long id) { this.id = id; }

            public bool MoveBlock(long targetBlock)
            {
                var tmp = blocks.TakeLast(1).First();
                if (tmp < targetBlock) return false;
                emptyBlocks.Enqueue(tmp);
                blocks = blocks.SkipLast(1).Prepend(targetBlock).ToList();
                return true;
            }

            public void moveFile(long startBlock)
            {
                blocks = LongRange(startBlock, blocks.Count).ToList();
            } 
        }
    }
}
