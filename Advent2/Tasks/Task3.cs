using System.Text.RegularExpressions;

namespace Advent2.Tasks3;

public record Input(string corrupted);

public record State(bool isEnabled, int total)
{
    public static State Empty = new State(true, 0);
}

public partial class Task3
{
    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex MulRegex();

    [GeneratedRegex(@"do\(\)|don't\(\)|mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex CommandRegex();

    private static Input Parse(string s)
    {
        return new Input(s);
    }

    public static int Solve1(Input input)
    {
        var matches = MulRegex().Matches(input.corrupted);
        var sum = matches.Select(m =>
        {
            var x = int.Parse(m.Groups[1].Value);
            var y = int.Parse(m.Groups[2].Value);
            return x * y;
        }).Sum();
        return sum;
    }

    
    // one could probably benifit from not using regexes but using parser combinators
    public int Solve2(Input input)
    {
        var matches = CommandRegex().Matches(input.corrupted);
        var sum = matches.Aggregate(State.Empty, (state, m) =>
        {
            return m.Value switch
            {
                "do()" => state with { isEnabled = true },
                "don't()" => state with { isEnabled = false },
                _ when state.isEnabled => state with
                {
                    total = state.total + int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value)
                },
                _ => state
            };
        }).total;
        return sum;
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