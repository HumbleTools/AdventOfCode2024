using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day5;

public class Day5
{
    private static Dictionary<int, int[]> _ruleMap = [];

    public static void Run(string input)
    {
        _ruleMap = Regex.Matches(input, @"(\d+)\|(\d+)")
            .GroupBy(match => int.Parse(match.Groups[1].Value))
            .ToDictionary(group => group.Key, group =>
                group.Select(m => int.Parse(m.Groups[2].Value)).ToArray());
        var updates = Regex.Matches(input, @"((?:\d+,)+(?:\d+))")
            .Select(match => match.Groups[1].Value.Split(",").Select(int.Parse).ToArray())
            .ToArray();
        var sortedUpdates = updates
            .GroupBy(IsValidUpdate)
            .ToDictionary(
                group => group.Key ? "valid" : "invalid",
                group => group.ToArray());
        var centerValidUpdatesSum = sortedUpdates.ContainsKey("valid") ? SumUpdateCenters(sortedUpdates["valid"]) : 0;
        var orderedInvalidUpdatesSum = sortedUpdates.ContainsKey("invalid")
            ? SumUpdateCenters(sortedUpdates["invalid"].Select(update => OrderUpdate(update, 0)).ToArray())
            : 0;

        Console.WriteLine(
            $"Day5 : centerValidUpdatesSum={centerValidUpdatesSum} orderedInvalidUpdatesSum={orderedInvalidUpdatesSum}");
    }

    private static bool IsValidUpdate(int[] update) => update
        .Select((_, index) => IsPageValid(update, index))
        .Count(valid => valid) == update.Length;

    private static bool IsPageValid(int[] update, int pageIndex)
    {
        if (!_ruleMap.ContainsKey(update[pageIndex]))
        {
            return true;
        }

        var pagesToCheck = update.Take(pageIndex).ToArray();
        var mustComeAfterKey = _ruleMap[update[pageIndex]];
        return !pagesToCheck.Intersect(mustComeAfterKey).Any();
    }

    private static int SumUpdateCenters(int[][] updates) => updates
        .Select(validUpdate => validUpdate[((validUpdate.Length + 1) / 2) - 1])
        .Sum();

    private static int[] OrderUpdate(int[] update, int pageToCheckOrShiftIndex)
    {
        if (IsValidUpdate(update))
        {
            return update;
        }
        
        while (IsPageValid(update, pageToCheckOrShiftIndex) && pageToCheckOrShiftIndex < update.Length - 1)
        {
            pageToCheckOrShiftIndex++;
        }

        (update[pageToCheckOrShiftIndex], update[pageToCheckOrShiftIndex - 1]) =
            (update[pageToCheckOrShiftIndex - 1], update[pageToCheckOrShiftIndex]);
        pageToCheckOrShiftIndex--;
        return OrderUpdate(update, pageToCheckOrShiftIndex);
    }
}