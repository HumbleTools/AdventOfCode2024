using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day6;

/// <summary>
/// OK sorry I had a little too much fun with this one :o)
/// </summary>
public static class Day6
{
    private const char Outside = 'o';
    private const char Object = '#';
    private const char Marked = 'X';
    private const string GuardChars = "^>v<";
    private static readonly (int x, int y)[] MoveForwardTuples = [(0, -1), (1, 0), (0, 1), (-1, 0)];
    private static char[][] _grid = null!;
    private static (int x, int y) _guard;
    private static bool _printGrid = false;

    public static void Run(string input, bool printGrid = false)
    {
        _printGrid = printGrid;
        _grid = input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(it => Regex.Matches(it, @".").Select(match => match.Value[0]).ToArray())
            .ToArray();
        _guard = FindGuardPosition();
        
        PrintGrid();
        while (IsOnGrid(_guard))
        {
            while (IsFacingObstruction())
            {
                TurnRight();
            }

            MoveForward();
        }

        Console.WriteLine($"Day6 : distinctPositions={CountDistinctPositions()}");
    }

    private static bool IsOnGrid((int x, int y) position) =>
        position.y >= 0 && position.y < _grid.Length && position.x >= 0 && position.x < _grid[0].Length;

    private static void MoveForward()
    {
        (int x, int y) previousPosition = (_guard.x, _guard.y);
        var guardChar = GetGridChar(_guard);
        _guard = _guard.Sum(GetMoveForwardTuple());
        MarkPosition(previousPosition, Marked);
        MarkPosition(_guard, guardChar);
        PrintGrid();
    }

    private static void PrintGrid()
    {
        if (!_printGrid)
        {
            return;
        }
        foreach (var line in _grid)
        {
            Console.WriteLine(line);
        }
        Console.WriteLine();
    }

    private static void MarkPosition((int x, int y) position, char mark)
    {
        if (IsOnGrid(position))
        {
            _grid[position.y][position.x] = mark;
        }
    }

    private static int CountDistinctPositions() => _grid
        .SelectMany(line => line)
        .Count(gridChar => gridChar==Marked);

    private static void TurnRight()
    {
        var guardFacingIndex = GetGuardFacingIndex();
        _grid[_guard.y][_guard.x] = guardFacingIndex == GuardChars.Length - 1
            ? GuardChars[0]
            : GuardChars[++guardFacingIndex];
        
        PrintGrid();
    }

    private static bool IsFacingObstruction() =>
        GetGridChar(_guard.Sum(GetMoveForwardTuple())) == Object;

    private static (int x, int y) GetMoveForwardTuple() => MoveForwardTuples[GetGuardFacingIndex()];

    private static int GetGuardFacingIndex() => GuardChars.IndexOf(GetGridChar(_guard));

    private static char GetGridChar((int x, int y) position) =>
        IsOnGrid(position) ? _grid[position.y][position.x] : Outside;

    private static (int posX, int posY) FindGuardPosition() => _grid
        .SelectMany((line, lineIndex) => 
            line.Select((gridChar, charIndex) => (gridChar, x: charIndex, y: lineIndex)))
        .Where(tuple => GuardChars.IndexOf(tuple.gridChar) >= 0)
        .Select(tuple => (tuple.x, tuple.y))
        .First();
}