using System.Collections.Immutable;

namespace Advent2.Tasks5;

public record Input(Rules rules, ImmutableList<UnvalidatedUpdate> updates);

public record Rules(ImmutableHashSet<Rule> rules);

public record Rule(int x, int y);

public record Update(ImmutableList<int> commands);

public record UnvalidatedUpdate(ImmutableList<int> commands);

public class Task5
{
    private static Input Example
    {
        get
        {
            var rules = ImmutableList.Create<(int, int)>([
                (47, 53),
                (97, 13),
                (97, 61),
                (97, 47),
                (75, 29),
                (61, 13),
                (75, 53),
                (29, 13),
                (97, 29),
                (53, 29),
                (61, 53),
                (97, 53),
                (61, 29),
                (47, 13),
                (75, 47),
                (97, 75),
                (47, 61),
                (75, 61),
                (47, 29),
                (75, 13),
                (53, 13)
            ]).Select(x => new Rule(x.Item1, x.Item2)).ToImmutableHashSet();

            var updates = ImmutableList.Create<UnvalidatedUpdate>([
                new UnvalidatedUpdate([75, 47, 61, 53, 29]),
                new UnvalidatedUpdate([97, 61, 53, 29, 13]),
                new UnvalidatedUpdate([75, 29, 13]),
                new UnvalidatedUpdate([75, 97, 47, 61, 53]),
                new UnvalidatedUpdate([61, 13, 29]),
                new UnvalidatedUpdate([97, 13, 75, 29, 47])
            ]);

            return new Input(new Rules(rules), updates);
        }
    }

    private static Input Parse(string s)
    {
            var clean = s.Replace("\r", "");

            var blocks = clean.Split("\n\n");   

            var rules = blocks[0]
                .Split('\n')                     
                .Where(l => l.Length > 0)       
                .Select(l =>
                {
                    var p = l.Split('|');
                    return new Rule(int.Parse(p[0]), int.Parse(p[1]));
                })
                .ToImmutableHashSet();

            var updates = blocks[1]
                .Split('\n')
                .Where(l => l.Length > 0)  
                .Select(l => new UnvalidatedUpdate(l.Split(',').Select(int.Parse).ToImmutableList()))
                .ToImmutableList();

            return new Input(new Rules(rules), updates);
    }

    public static ImmutableList<Rule> GenerateRules(UnvalidatedUpdate update)
    {
        var numbers = update.commands;
        var result = ImmutableList.CreateBuilder<Rule>();
        for (var i = 0; i < numbers.Count - 1; i++)
        {
            for (var j = i + 1; j < numbers.Count; j++)
            {
                result.Add(new Rule(numbers[i], numbers[j]));
            }
        }

        return result.ToImmutableList();
    }

    public static int Solve1(Input input)
    {
        var validRules = input.updates.SelectMany(unvalidatedUpate =>
            ValidateRules<ImmutableList<Update>>(input.rules, unvalidatedUpate, update => [update], () => []));
        return validRules.Select(TakeCenter).Sum();
    }

    private static C ValidateRules<C>(Rules inputRules, UnvalidatedUpdate unvalidatedUpdate, Func<Update, C> succ,
        Func<C> err)
    {
        var relevantRules = GenerateRules(unvalidatedUpdate);

        var conflictingRules = relevantRules.Any(x => inputRules.rules.Contains(new Rule(x.y, x.x)));
        return conflictingRules ? err() : succ(new Update(unvalidatedUpdate.commands));
    }

    private static int TakeCenter(Update update)
    {
        // could be wrong but we would have checked that earlier
        return update.commands[update.commands.Count / 2];
    }


    public int Solve2(Input input)
    {
        throw new NotImplementedException();
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