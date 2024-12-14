using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day6;

/// <summary>
/// OK sorry I had a little too much fun with this one :o)
/// </summary>
public static class Day6
{
    private const string GuardChars = "^>v<";
    private const char Object = '#';
    private const char Marked = 'X';
    private const char Outside = '/';
    private const char Vertical = '|';
    private const char Horizontal = '-';
    private const char BothDirections = '+';
    private const char NewObstacle = 'O';
    private static readonly (int x, int y)[] MoveForwardTuples = [(0, -1), (1, 0), (0, 1), (-1, 0)];
    private static char[][] _grid = null!;
    private static (int x, int y) _guard;
    private static bool _shouldPrintGrid;

    public static void Run(string input, bool shouldPrintGrid = false)
    {
        _shouldPrintGrid = shouldPrintGrid;
        _grid = ResetGrid(input);
        _guard = FindGuardPosition();
        
        WalkGrid();
        var distinctPositions = CountDistinctPositions();

        var numberOfLoops = 0;
        using var gridPositionsEnumerator = GetAllGridPositions().GetEnumerator();
        while (gridPositionsEnumerator.MoveNext())
        {
            _grid = ResetGrid(input);    
            _guard = FindGuardPosition();
            var currentPosition = gridPositionsEnumerator.Current;
            if (IsGuardAt(currentPosition) || IsObjectAtPosition(Object, currentPosition))
            {
                continue;
            }
            MarkPosition(currentPosition, NewObstacle);
            if (IsGuardInALoop())
            {
                numberOfLoops++;
            }
        }
        
        Console.WriteLine($"Day6 : distinctPositions={distinctPositions} numberOfLoops={numberOfLoops}");
    }

    private static void WalkGrid()
    {
        PrintGrid();
        while (IsOnGrid(_guard))
        {
            while (IsFacingObstruction())
            {
                TurnRight();
            }

            MoveForward();
        }
    }

    private static bool IsGuardInALoop()
    {
        throw new NotImplementedException();
    }

    private static char[][] ResetGrid(string input) => input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(it => Regex.Matches(it, @".").Select(match => match.Value[0]).ToArray())
            .ToArray();

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
        if (!_shouldPrintGrid)
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
            line.Select((_, charIndex) => (x: charIndex, y: lineIndex)))
        .Where(IsGuardAt)
        .First();

    private static bool IsGuardAt((int x, int y) position) => GuardChars.IndexOf(GetGridChar(position)) >= 0;
    private static bool IsObjectAtPosition(char objectChar, (int x, int y) position) => GetGridChar(position) == objectChar;

    private static IEnumerable<(int posX, int posY)> GetAllGridPositions() => _grid
        .SelectMany((line, lineIndex) =>
            line.Select((gridChar, charIndex) => (gridChar, x: charIndex, y: lineIndex)))
        .Select(tuple => (tuple.x, tuple.y));
}