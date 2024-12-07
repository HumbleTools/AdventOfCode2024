using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day3;

public static class Day3
{
    public static void Run(string input)
    {
        var mulMatches = Regex.Matches(input, @"mul\((\d+),(\d+)\)");
        var dontMatches = Regex.Matches(input, @"don't\(\)");
        var doMatches = Regex.Matches(input, @"do\(\)");
        var allMulsResult = mulMatches
            .Aggregate(0, (agg, match) => agg + MulGroupResult(match.Groups));
        var allMatches = mulMatches
            .Concat(dontMatches)
            .Concat(doMatches)
            .OrderBy(match => match.Index);
        var isOn = true;
        var doDontResult = allMatches.Aggregate(0, (agg, match) =>
        {
            if (match.IsDo())
            {
                isOn = true;
            }
            if (match.IsDont())
            {
                isOn = false;
            }
            return (match.IsMul() && isOn) ? agg + MulGroupResult(match.Groups) : agg;
        });
        Console.WriteLine($"Day3: allMulsResult={allMulsResult} doDontResult={doDontResult}");
    }

    private static int MulGroupResult(GroupCollection group) =>
        Int32.Parse(group[1].Value) * Int32.Parse(group[2].Value);
    
    private static bool IsMul(this Match match) => match.Value.StartsWith("mul");
    private static bool IsDo(this Match match) => match.Value.StartsWith("do");
    private static bool IsDont(this Match match) => match.Value.StartsWith("don't");
}