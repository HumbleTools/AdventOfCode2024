using System.Text.RegularExpressions;

namespace AdventOfCode2024.day1;

public class Day1
{
    public static void Run(string input)
    {
        var listOne = new List<int> { };
        var listTwo = new List<int> { };
        input
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(it => Regex.Split(it, @"\s+"))
            .ToList().ForEach(it =>
            {
                listOne.Add(Int32.Parse(it[0]));
                listTwo.Add(Int32.Parse(it[1]));
            });
        listOne.Sort();
        listTwo.Sort();
        var distance = listOne.Zip(listTwo, (x, y) => Math.Abs(x - y)).Sum();
        var similarity = listOne.Select(it => listTwo.Count(that => that == it) * (long)it).Sum();
        Console.WriteLine($"Day1: distance={distance} similarity={similarity}");
    }
}