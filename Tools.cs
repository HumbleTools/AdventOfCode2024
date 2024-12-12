namespace AdventOfCode2024;

public static class Tools
{
    public static (int x, int y) Sum(this (int x, int y) tuple1, (int x, int y) tuple2) =>
        (tuple1.x + tuple2.x, tuple1.y + tuple2.y);
}