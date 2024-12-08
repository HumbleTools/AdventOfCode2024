namespace AdventOfCode2024.Day4;

public class Day4
{
    public static void Run(string input)
    {
        var grid = input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.ToArray())
            .ToArray();
        var width = grid[0].Length;
        var height = grid.Length;

        var totalXmas = 0;
        var totalCorrectXmas = 0;
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if (grid[y][x] == 'X')
                {
                    totalXmas += CountFromOrigin(grid, x, y, "XMAS".ToArray());
                }
                if (grid[y][x] == 'A')
                {
                    totalCorrectXmas += IsXmas(grid, x, y, "MAS".ToArray()) ? 1 : 0;
                }
            }
        }

        Console.WriteLine($"Day4 : totalXMAS={totalXmas} totalCorrectXmas={totalCorrectXmas}");
    }

    private static int CountFromOrigin(char[][] grid, int originX, int originY, char[] word)
    {
        return new[]
        {
            MatchRightWord(grid, originX, originY, word),
            MatchReverseWord(grid, originX, originY, word),
            MatchUpwardWord(grid, originX, originY, word),
            MatchDownwardWord(grid, originX, originY, word),
            MatchUpRightWord(grid, originX, originY, word),
            MatchDownRightWord(grid, originX, originY, word),
            MatchDownLeftWord(grid, originX, originY, word),
            MatchUpLeftWord(grid, originX, originY, word)
        }.Count(it => it is true);
    }

    private static bool IsXmas(char[][] grid, int originX, int originY, char[] word)
    {
        var originX1 = originX - 1;
        var originY1 = originY + 1;
        var originX2 = originX - 1;
        var originY2 = originY - 1;
        var reversedWord = word.Reverse().ToArray();
        return (MatchDownRightWord(grid, originX1, originY1, word) ||
                MatchDownRightWord(grid, originX1, originY1, reversedWord))
               && (MatchUpRightWord(grid, originX2, originY2, word) ||
                   MatchUpRightWord(grid, originX2, originY2, reversedWord));
    }

    private static bool MatchRightWord(char[][] grid, int originX, int originY, char[] word) =>
        MatchWord(word, grid,
            GenerateCoordArray(originX, word.Length, false),
            GenerateConstantArray(originY, word.Length));

    private static bool MatchReverseWord(char[][] grid, int originX, int originY, char[] word) =>
        MatchWord(word, grid,
            GenerateCoordArray(originX, word.Length, true),
            GenerateConstantArray(originY, word.Length));

    private static bool MatchUpwardWord(char[][] grid, int originX, int originY, char[] word) =>
        MatchWord(word, grid,
            GenerateConstantArray(originX, word.Length),
            GenerateCoordArray(originY, word.Length, false));

    private static bool MatchDownwardWord(char[][] grid, int originX, int originY, char[] word) =>
        MatchWord(word, grid,
            GenerateConstantArray(originX, word.Length),
            GenerateCoordArray(originY, word.Length, true));

    private static bool MatchUpRightWord(char[][] grid, int originX, int originY, char[] word) =>
        MatchWord(word, grid,
            GenerateCoordArray(originX, word.Length, false),
            GenerateCoordArray(originY, word.Length, false));

    private static bool MatchDownRightWord(char[][] grid, int originX, int originY, char[] word) =>
        MatchWord(word, grid,
            GenerateCoordArray(originX, word.Length, false),
            GenerateCoordArray(originY, word.Length, true));

    private static bool MatchDownLeftWord(char[][] grid, int originX, int originY, char[] word) =>
        MatchWord(word, grid,
            GenerateCoordArray(originX, word.Length, true),
            GenerateCoordArray(originY, word.Length, true));

    private static bool MatchUpLeftWord(char[][] grid, int originX, int originY, char[] word) =>
        MatchWord(word, grid,
            GenerateCoordArray(originX, word.Length, true),
            GenerateCoordArray(originY, word.Length, false));

    private static bool MatchWord(char[] word, char[][] grid, int[] coordX, int[] coordY)
    {
        for (var index = 0; index < coordX.Length; index++)
        {
            var x = coordX[index];
            var y = coordY[index];
            if (x < 0 || x >= grid[0].Length || y < 0 || y >= grid.Length || word[index] != grid[y][x])
            {
                return false;
            }
        }

        return true;
    }

    private static int[] GenerateCoordArray(int start, int size, bool reverse)
    {
        var values = new List<int>();
        if (reverse)
        {
            for (var i = start; i > start - size; i--)
            {
                values.Add(i);
            }

            return values.ToArray();
        }

        return Enumerable.Range(start, size).ToArray();
    }

    public static int[] GenerateConstantArray(int value, int length)
    {
        return Enumerable.Repeat(value, length).ToArray();
    }
}