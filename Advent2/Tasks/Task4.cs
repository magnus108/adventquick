using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Advent2.Tasks3;

namespace Advent2.Tasks4;

public record Input(ImmutableList<string> input);

public record Position(int x, int y);

public record Map(ImmutableDictionary<Position, char> dict)
{
    public static Map FromStrings(ImmutableList<string> s)
    {
        return new Map(s
            .SelectMany((line, i) => line.Select((ch, j) => new KeyValuePair<Position, char>(new Position(i, j), ch)))
            .ToImmutableDictionary(pair => pair.Key, pair => pair.Value));
    }

    public static readonly Map Example = FromStrings(ImmutableList.Create(
        "MMMSXXMASM",
        "MSAMXMSMSA",
        "AMXSXMAAMM",
        "MSAMASMSMX",
        "XMASAMXAMM",
        "XXAMMXXAMA",
        "SMSMSASXSS",
        "SAXAMASAAA",
        "MAMMMXMMMM",
        "MXMXAXMASX"
    ));
};

public class Task4
{
    private static Input Parse(string s)
    {
        var lines = s.Split('\n').ToImmutableList();
        return new Input(lines);
    }

    public static int Solve1(Input input)
    {
        var myMap = Map.FromStrings(input.input);

        var candidates = myMap.dict.Keys.SelectMany(GenerateCandidates).ToImmutableList();

        var result = candidates
            .Select(candidate =>
                LookupCandidate(myMap, candidate, c1 => c2 => c1 == c2, () => new Func<char, bool>(c2 => false)))
            .Count(f => f.Zip("XMAS").All((tuple => tuple.First(tuple.Second))));

        return result;
    }

    public static ImmutableList<TX> LookupCandidate<TX>(Map map, ImmutableList<Position> candidates,
        Func<char, TX> succ, Func<TX> err)
    {
        // must fix. This unsafe
        return candidates.Select(p =>
        {
            if (map.dict.TryGetValue(p, out var value))
            {
                return succ(value);
            }
            else
            {
                return err();
            }

            ;
        }).ToImmutableList();
    }

    public static ImmutableList<ImmutableList<Position>> GenerateCandidates(Position position)
    {
        return GetDirections()
            .Select(dir => GoFrom(4, dir, position))
            .ToImmutableList();

        /*
        return ImmutableList.Create<ImmutableList<Position>>(
            [[position, position with {x = position.x+1},position with {x = position.x+2}, position with {x = position.x+3}]
            ,[position, position with {x = position.x-1},position with {x = position.x-2}, position with {x = position.x-3}]
            ,[position, position with {y = position.y+1},position with {y = position.y+2}, position with {y = position.y+3}]
            ,[position, position with {y = position.y-1},position with {y = position.y-2}, position with {y = position.y-3}]
            ,[position, position with {x = position.x-1, y = position.y-1}, position with {x = position.x-2, y = position.y-2}, position with {x = position.x-3, y = position.y-3}]
            ,[position, position with {x = position.x+1, y = position.y+1}, position with {x = position.x+2, y = position.y+2}, position with {x = position.x+3, y = position.y+3}]
            ,[position, position with {x = position.x+1, y = position.y-1}, position with {x = position.x+2, y = position.y-2}, position with {x = position.x+3, y = position.y-3}]
            ,[position, position with {x = position.x-1, y = position.y+1}, position with {x = position.x-2, y = position.y+2}, position with {x = position.x-3, y = position.y+3}]
            ]
            );
            */
    }

    public static ImmutableList<Position> GoFrom(int n, (int dx, int dy) direction, Position start)
    {
        return Enumerable.Range(0, n)
            .Select(step => start with
            {
                x = start.x + step * direction.dx,
                y = start.y + step * direction.dy
            })
            .ToImmutableList();
    }

    public static ImmutableList<(int, int)> GetDirections()
    {
        return Enumerable.Range(-1, 3)
            .SelectMany(i => Enumerable.Range(-1, 3)
                .Select(j => (i, j)))
            .Where(pair => pair != (0, 0))
            .ToImmutableList();
    }

    public int Solve2(Input input)
    {
        var myMap = Map.FromStrings(input.input);

        var candidates = myMap.dict.Keys.SelectMany(GenerateCandidates2).ToImmutableList();

        var result = candidates
            .Select(candidate =>
                LookupCandidate(myMap, candidate, c1 => c2 => c1 == c2, () => new Func<char, bool>(c2 => false)))
            .Count(f => f.Zip("MASMAS").All((tuple => tuple.First(tuple.Second))));

        return result;
    }

    private static readonly ImmutableList<(int dx, int dy)> D1 =
        ImmutableList.Create((-1, -1), (0, 0), ( 1,  1));

    private static readonly ImmutableList<(int dx, int dy)> D2 =
        ImmutableList.Create((-1,  1), (0, 0), ( 1, -1));

    private static readonly ImmutableList<ImmutableList<(int dx, int dy)>> XWalks =
        (
            from flipFirst  in new[] { false, true }  
            from flipSecond in new[] { false, true } 
            let diag1 = flipFirst  ? D1.Reverse() : D1
            let diag2 = flipSecond ? D2.Reverse() : D2
            select diag1.Concat(diag2).ToImmutableList()
        ).ToImmutableList();

    public static ImmutableList<ImmutableList<Position>> GenerateCandidates2(
        Position position)
    {
        return XWalks.Select(walk =>
                walk.Select(o => position with
                {
                    x = position.x + o.dx,
                    y = position.y + o.dy
                }).ToImmutableList())
            .ToImmutableList();
    }

    public int Part1(string s)
    {
        var input = Parse(s);
        var solution = Solve1(input);
        return solution;
    }

    public int Part2(string s)
    {
        var input = Parse(s);
        var solution = Solve2(input);
        return solution;
    }
}