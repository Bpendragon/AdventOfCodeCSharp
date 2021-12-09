using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2020
{

    class Day20 : ASolution
    {
        readonly Tile[][] tiles;
        readonly Tile BigTile;
        public Day20() : base(20, 2020, "Jurassic Jigsaw")
        {
            tiles = AssemblePuzzle(Input);
            BigTile = MergeTiles(tiles);
        }



        protected override string SolvePartOne()
        {

            return
                (tiles[0][0].id *
                tiles[0][^1].id *
                tiles[^1][0].id *
                tiles[^1][^1].id).ToString();
        }



        protected override string SolvePartTwo()
        {
            List<(int x, int y)> Nessie = new() //these are *just* the offsets from the tip off the tail
            {
                (0,0), //include the tail in case it gets ripped out of our dictionary early
                (1, -1),
                (4, -1),
                (5,0),
                (6,0),
                (7, -1),
                (10, -1),
                (11, 0),
                (12, 0),
                (13, -1),
                (16, -1),
                (17, 0),
                (18, 0),
                (18,1),
                (19, 0)
            };

            Dictionary<(int x, int y), char> map = new();

            for (int y = 0; y < BigTile.data.Length; y++)
            {
                for(int x = 0; x < BigTile.data[0].Length; x++)
                {
                    map[(x, y)] = BigTile.data[y][x];
                }
            }

            for (int i = 0; i < 4; i++)
            {
                var hashLocations = map.Where(x => x.Value == '#');
                
                foreach (var kvp in hashLocations)
                {
                    bool success = true;
                    //Check normal
                    foreach (var offset in Nessie)
                    {
                        if (map.GetValueOrDefault(kvp.Key.Add(offset), '.') != '#')
                        { success = false; break; }
                    }

                    if(success)
                    {
                        foreach (var offset in Nessie) map.Remove(kvp.Key.Add(offset));
                    }
                   
                }

                hashLocations = map.Where(x => x.Value == '#');

                foreach (var kvp in hashLocations)
                {
                    //Check horizontal flip
                    bool success = true;
                    //Check normal
                    foreach (var (x, y) in Nessie)
                    {
                        if (map.GetValueOrDefault(kvp.Key.Add((-x, y)), '.') != '#')
                        { success = false; break; }
                    }

                    if (success)
                    {
                        foreach (var (x, y) in Nessie) map.Remove(kvp.Key.Add((-x, y)));
                    }
                }

                //rotate 90
                var newNessie = new List<(int x, int y)>();
                foreach(var (x, y) in Nessie)
                {
                    newNessie.Add((-y, x));
                }
                Nessie = new List<(int x, int y)>(newNessie);
            }

            return map.Count(x => x.Value == '#').ToString();
        }

        private static Tile[] ParseTiles(string input)
        {
            return (
                from block in input.Split("\n\n")
                let lines = block.Split("\n")
                let id = Regex.Match(lines[0], "\\d+").Value
                let image = lines.Skip(1).Where(x => x != "").ToArray()
                select new Tile(int.Parse(id), image)
            ).ToArray();
        }

        static bool IsEdge(string pattern, Dictionary<string, List<Tile>> pairs) => pairs[pattern].Count == 1;
        private static Tile[][] AssemblePuzzle(string input)
        {
            var tiles = ParseTiles(input);

            var pairs = new Dictionary<string, List<Tile>>();
            foreach (var tile in tiles)
            {
                for (var i = 0; i < 8; i++) //This works by going through all 8 possible orientations, equivalent to Matrix Transpose, check, then flip about the horizontal, check, repeat 4 times
                {
                    var pattern = tile.Top();
                    if (!pairs.ContainsKey(pattern))
                    {
                        pairs[pattern] = new List<Tile>();
                    }
                    pairs[pattern].Add(tile);
                    tile.ChangeOrientation();
                }
            }

            
            Tile getNeighbour(Tile tile, string pattern) => pairs[pattern].SingleOrDefault(other => other != tile);

            Tile putTileInPlace(Tile above, Tile left)
            {
                if (above == null && left == null)
                {
                    // find top-left corner
                    foreach (var tile in tiles)
                    {
                        for (var i = 0; i < 8; i++)
                        {
                            if (IsEdge(tile.Top(), pairs) && IsEdge(tile.Left(), pairs))
                            {
                                return tile;
                            }
                            tile.ChangeOrientation();
                        }
                    }
                }
                else
                {
                    // we know the tile from the inversion structure, just need to find its orientation
                    var tile = above != null ? getNeighbour(above, above.Bottom()) : getNeighbour(left, left.Right());
                    while (true)
                    {
                        var topMatch = above == null ? IsEdge(tile.Top(), pairs) : tile.Top() == above.Bottom();
                        var leftMatch = left == null ? IsEdge(tile.Left(), pairs) : tile.Left() == left.Right();

                        if (topMatch && leftMatch)
                        {
                            return tile;
                        }
                        tile.ChangeOrientation();
                    }
                }

                throw new Exception();
            }

            // once the corner is fixed we can always find a unique tile that matches the one to the left & above
            // just fill up the tileset one by one
            var size = (int)Math.Sqrt(tiles.Length);
            var puzzle = new Tile[size][];
            for (var x = 0; x < size; x++)
            {
                puzzle[x] = new Tile[size];
                for (var y = 0; y < size; y++)
                {
                    var above = x == 0 ? null : puzzle[x - 1][y];
                    var left = y == 0 ? null : puzzle[x][y - 1];
                    puzzle[x][y] = putTileInPlace(above, left);
                }
            }
            return puzzle;
        }

        private static Tile MergeTiles(Tile[][] tiles)
        {
            // create a big tile leaving out the borders
            var image = new List<string>();
            var tileSize = tiles[0][0].size;
            var tileCount = tiles.Length;
            for (var x = 0; x < tileCount; x++)
            {
                for (var i = 1; i < tileSize - 1; i++) //leaves out top and bottom
                {
                    var st = "";
                    for (var y = 0; y < tileCount; y++)
                    {
                        st += tiles[x][y].Row(i).Substring(1, tileSize - 2); //leaves out left and right
                    }
                    image.Add(st);
                }
            }
            return new Tile(00, image.ToArray());
        }
    }

    class Tile
    {
        public long id;
        public int size;
        public string[] data;


        int orientation = 0;

        public Tile(long id, string[] data)
        {
            this.id = id;
            this.data = data;
            this.size = data.Length;
        }

        public void ChangeOrientation()
        {
            this.orientation++;
        }

        public char this[int x, int y]
        {
            get
            {
                for (var i = 0; i < orientation % 4; i++)
                {
                    (x, y) = (y, size - 1 - x); // rotate
                }

                if (orientation % 8 >= 4)
                {
                    y = size - 1 - y; // flip vertical axis
                }

                return this.data[x][y];
            }
        }

        public string Row(int row) => GetSlice(row, 0, 0, 1);
        public string Col(int col) => GetSlice(0, col, 1, 0);
        public string Top() => Row(0);
        public string Bottom() => Row(size - 1);
        public string Left() => Col(0);
        public string Right() => Col(size - 1);

        public override string ToString()
        {
            return $"Tile {id}:\n" + string.Join("\n", Enumerable.Range(0, size).Select(i => Row(i)));
        }

        string GetSlice(int x, int y, int drow, int dcol)
        {
            var st = "";
            for (var i = 0; i < size; i++)
            {
                st += this[x, y];
                x += drow;
                y += dcol;
            }
            return st;
        }
    }
}


//Original implementation below. Note how "BuildImage" got nowhere because the way I was doing things was silly
/*

private void AttemptFit(Tile t1, Tile t2)
{
    for (int i = 0; i < t1.slices.Length; i++)
    {
        for (int j = 0; j < t2.slices.Length; j++)
        {
            if (t1.slices[i] == t2.slices[j])
            {
                t1.neighbors[t2] = (i % 4, i < 4);
                t2.neighbors[t1] = (j % 4, j < 4);
            }
        }
    }
}


private List<string> BuildImage()
{
    List<Tile> visited = new List<Tile>();
    var tmpImage = new char[96][];
    for (int i = 0; i < 96; i++)
    {
        tmpImage[i] = new char[96];
    }
    //Have to deal with one corner first
    var cornerPiece = tiles.Where(t => t.neighbors.Count == 2).First();
    var sideInfo = cornerPiece.neighbors.Values.ToArray();




    Queue<Tile> q = new Queue<Tile>();



    throw new NotImplementedException();
}

public static string[] FlipVertical(string[] grid)
{
    string[] rows = grid;
    string[] newRows = new string[rows.Length];

    for (int i = 0; i < rows.Length; i++)
    {
        newRows[rows.Length - i - 1] = rows[i];
    }

    return newRows;
}

public static string[] FlipHorizontal(string[] grid)
{
    string[] rows = grid;
    string[] newRows = new string[rows.Length];

    for (int i = 0; i < rows.Length; i++)
    {
        newRows[i] = rows[i].Reverse();
    }

    return newRows;
}

public static string[] Rotate(string[] grid)
{
    string[] rows = grid;
    char[,] newRows = new char[rows.Length, rows.Length];

    for (int i = 0; i < rows.Length; i++)
    {
        for (int j = 0; j < rows.Length; j++)
        {
            newRows[rows.Length - j - 1, i] = rows[i][j];
        }
    }

    string[] sNewRows = new string[rows.Length];

    for (int i = 0; i < rows.Length; i++)
    {
        for (int j = 0; j < rows.Length; j++)
        {
            sNewRows[i] += newRows[i, j];
        }
    }


    return sNewRows;
}

public static string[] CopyFrom(string[] grid, int startRow, int startColumn, int num)
{
    string[] section = new string[num];
    for (int i = 0; i < num; i++)
    {
        for (int j = 0; j < num; j++)
        {
            section[i] += grid[i + startRow][j + startColumn];
        }
    }

    return section;
}

public static void CopyTo(string[] grid, string section, int size, int startRow, int startColumn)
{
    string[] rows = section.Split('/', StringSplitOptions.RemoveEmptyEntries);
    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            grid[startRow + i] += rows[i][j];
        }
    }
}


class Tile
{
    public long id;

    public string[] data;

    public Dictionary<Tile, (int edge, bool norm)> neighbors = new Dictionary<Tile, (int edge, bool norm)>();

    public string[] slices;

    public string[] imageData;

    public Tile(long id, string[] data)
    {
        this.id = id;
        this.data = data;
        slices = GetSlices(data);
        imageData = GetImageData(data);
    }

    private string[] GetImageData(string[] data)
    {
        var res = new string[data.Length - 2];
        for (int i = 1; i < data.Length - 1; i++)
        {
            res[i - 1] = data[i][1..^1];
        }
        return res;
    }

    public static string[] GetSlices(string[] data)
    {
        string FirstCol = "", LastCol = "";

        for (int i = 0; i < 10; i++)
        {
            FirstCol += data[i][0];
            LastCol += data[i][^1];
        }
        string[] Slices = new string[]
        {
        data[0], //top normal
        FirstCol, //left normal
        data[^1], //bottom normal
        LastCol, // right normal
        data[0].Reverse(), //top horizontalFlip or 180 rotation
        data[^1].Reverse(), //left horizontalFlip or 180 rotation
        FirstCol.Reverse(), //bottom vertical flip or 180 rotation
        LastCol.Reverse(), //right verticalFlip or 180 rotation
        };
        return Slices;

    }

    public static IEnumerable<string[]> GetRotations(string[] data)
    {
        var tmp = data;
        for (int i = 0; i < 4; i++)
        {
            yield return tmp = Rotate(tmp);
        }



    }

}
}
} */
