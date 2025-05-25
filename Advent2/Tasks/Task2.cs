using System.Collections.Immutable;

namespace Advent2.Tasks2;


public record Input(ImmutableList<ImmutableList<int>> levels);

public record UnvalidatedLevel(ImmutableList<int> level);
public record SafeLevel(ImmutableList<int> level);


public class Task2
{
       
       private static Input Parse(string s)
       { 
              // Could concern ourselfs with big input files / read lines idividually, keep track of location in line using span?
            var input = ImmutableList.CreateBuilder<ImmutableList<int>>();
            
            var lines = s.Split('\n');
            foreach (var t1 in lines)
            {
                   var level = ImmutableList.CreateBuilder<int>();
                   
                   var line = t1.Split(' ');
                   foreach (var t in line)
                   {
                          var item = int.Parse(t);
                          level.Add(item);
                   }
                   input.Add(level.ToImmutableList());
            }

            return new Input(input.ToImmutableList());
       }

       public static C ParseSafeLevel<C>(UnvalidatedLevel unvalidatedLevel, Func<SafeLevel, C> succ, Func<C> err)
       {
              var level = unvalidatedLevel.level;
              var diffs = level.Zip(level.Skip(1)).Select(x => x.First - x.Second).ToImmutableList();
              var upDown = ImmutableList.Create(diffs, diffs.Select(x => x * -1));
              var result = upDown.Any(path => path.All(p => p is >= 1 and <= 3));
              return result ? succ(new SafeLevel(level)) : err();

       }
       public static int Solve1(Input input)
       {
              var levels = input.levels;
              var safeLevels = levels.Select(level => ParseSafeLevel(new UnvalidatedLevel(level), safeLevel => 1, () => 0)).Sum();
              return safeLevels;
       }
       
       
       // harder because we need to search
       public static C ParseSafeLevel2<C>(UnvalidatedLevel unvalidatedLevel, Func<SafeLevel, C> succ, Func<C> err)
       {
              var level = unvalidatedLevel.level;
              var diffs = level.Zip(level.Skip(1)).Select(x => x.First - x.Second).ToImmutableList();
              var upDown = ImmutableList.Create(diffs, diffs.Select(x => x * -1));
              var result = upDown.Any(path => path.All(p => p is >= 1 and <= 3));
              return result ? succ(new SafeLevel(level)) : err();

       }
       
       public static int Solve2(Input input)
       {
              var possible = input.levels.Select(GenerateValidLevels);
              var safeLevels = possible.Select(levels => levels.Any(level => ParseSafeLevel2(level, safeLevel => true, () => false)) ?  1 : 0).Sum();
              return safeLevels;
       }

       //yield?
       private static IEnumerable<UnvalidatedLevel> GenerateValidLevels(ImmutableList<int> inputLevels)
       {
               if (inputLevels.Count == 0)
               {
                   yield return new UnvalidatedLevel(ImmutableList<int>.Empty);
                   yield break;
               }

               var xs = inputLevels.Skip(1).ToImmutableList();
               yield return new UnvalidatedLevel(xs);

               foreach (var sub in GenerateValidLevels(xs))
               { 
                     var withHead = inputLevels.Take(1);
                     var result  = withHead.Concat(sub.level);
                     yield return new UnvalidatedLevel(result.ToImmutableList());
               }
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