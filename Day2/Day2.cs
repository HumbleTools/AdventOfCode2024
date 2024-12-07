using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day2;

public class Day2
{
    public static void Run(string input)
    {
        var reports = input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(report => Regex.Split(report, @"\s+"))
            .Select(report => report.Select(levelString => Int32.Parse(levelString)).ToArray());
        var valid = reports
            .Count(IsValid);
        var reallyValid = reports.Count(IsReallyValid);
        Console.WriteLine($"Day2: valid={valid} reallyValid={reallyValid}");
    }

    private static bool IsReallyValid(int[] report)
    {
        if (IsValid(report))
        {
            return true;
        }

        IList<int> reportMinusOne = [];
        for (var index = 0; index < report.Length; index++)
        {
            for (var index2 = 0; index2 < report.Length; index2++)
            {
                if (index2 == index)
                {
                    continue;
                }

                reportMinusOne.Add(report[index2]);
            }

            if (IsValid(reportMinusOne.ToArray()))
            {
                return true;
            }

            reportMinusOne.Clear();
        }

        return false;
    }

    private static bool IsValid(int[] report) => (IsIncreasing(report) || IsDecreasing(report)) && AreAllSpaced(report);

    private static bool AreAllSpaced(int[] report)
    {
        var areSpaced = true;
        report.Aggregate((prev, next) =>
        {
            var diff = Math.Abs(prev - next);
            if (diff < 1 || diff > 3)
            {
                areSpaced = false;
            }

            return next;
        });
        return areSpaced;
    }

    private static bool IsDecreasing(int[] report)
    {
        var isDecreasing = true;
        report.Aggregate((previous, next) =>
        {
            if (next - previous < 0)
            {
                isDecreasing = false;
            }

            return next;
        });
        return isDecreasing;
    }

    private static bool IsIncreasing(int[] report)
    {
        var isIncreasing = true;
        report.Aggregate((previous, next) =>
        {
            if (next - previous > 0)
            {
                isIncreasing = false;
            }

            return next;
        });
        return isIncreasing;
    }
}