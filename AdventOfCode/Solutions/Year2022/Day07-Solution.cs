using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(07, 2022, "No Space Left On Device")]
    class Day07 : ASolution
    {
        readonly List<string> cout;
        readonly Dictionary<string, Directory> Directories = new();
        
        public Day07() : base()
        {
            cout = Input.SplitByNewline();
            Directory root = new()
            {
                Name = "/"
            };
            Directories["/"] = root;

            Directory curDir = Directories["/"];
            for (int i = 0; i < cout.Count; i++)
            {
                var sections = cout[i].Split();
                if (sections[1] == "cd")
                {
                    if (sections[2] == "..")
                    {
                        curDir = curDir.Parent;
                        continue;
                    }
                    else if (Directories.ContainsKey($"{curDir.Name},{sections[2]}")) curDir = Directories[$"{curDir.Name},{sections[2]}"];
                    else
                    {
                        Directory newDir = new()
                        {
                            Name = sections[2],
                            Parent = curDir
                        };
                        curDir.Children.Add(newDir);
                        Directories[sections[2]] = newDir;
                        curDir = newDir;
                    }
                }
                else if (sections[1] == "ls")
                {
                    while (i + 1 < cout.Count && cout[i + 1][0] != '$')
                    {
                        i++;
                        sections = cout[i].Split();
                        if (long.TryParse(sections[0], out long size))
                        {
                            AocFile newFile = new()
                            {
                                Name = sections[1],
                                Size = size
                            };
                            curDir.Files.Add(newFile);
                        }
                        else
                        {
                            Directory newDir = new()
                            {
                                Name = sections[1],
                                Parent = curDir
                            };
                            curDir.Children.Add(newDir);
                            Directories[$"{curDir.Name},{sections[1]}"] = newDir;
                        }
                    }
                }
            }
        }

        protected override object SolvePartOne()
        {
            return Directories.Values.Where(a => a.Size <= 100_000).Sum(a=> a.Size);
        }

        protected override object SolvePartTwo()
        {
            long requiredSpace = 30_000_000 - (70_000_000 - Directories["/"].Size);
            return Directories.Values.Where(a => a.Size >= requiredSpace).Min(b=>b.Size);
        }

        private class Directory
        {
            public string Name { get; set; }
            public HashSet<Directory> Children { get; set; } = new();
            public Directory Parent { get; set; }
            public HashSet<AocFile> Files { get; set; } = new();

            public long Size => Files.Sum(a => a.Size) + Children.Sum(a => a.Size);

            public override string ToString()
            {
                return $"{Name}, {Size}";
            }

            public override int GetHashCode()
            {
                return string.GetHashCode($"{Parent.Name},{Name}");
            }
        }

        private class AocFile
        {
            public string Name { get; set; }
            public long Size { get; set; }

            public override string ToString()
            {
                return $"{Name}, {Size}";
            }

            public override int GetHashCode()
            {
                return string.GetHashCode($"{Name}, {Size}");
            }
        }
    }

    
}
