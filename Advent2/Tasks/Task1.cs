using System.Collections.Immutable;
using Sprache;

namespace Advent2.Tasks1;


public record Input(ImmutableList<int> lefts, ImmutableList<int> rights);
public class Task1
{
       
       private static Input Parse(string s)
       {
              
         //     var number = Sprache.Parse.Number.Select(int.Parse);
          //    var commaSeparated = number.DelimitedBy(Sprache.Parse.Char(','));
           //   var result = commaSeparated.Parse(s);
            //  return new Input(ImmutableList<int>.Empty, ImmutableList<int>.Empty);
            var lefts = ImmutableList.CreateBuilder<int>();
            var rights = ImmutableList.CreateBuilder<int>();
            
            var lines = s.Split('\n');
            foreach (var t in lines)
            {
                   var input = t.Split(' ');
                   var left = int.Parse(input[0]);
                   var right = int.Parse(input[3]);
                   lefts.Add(left);
                   rights.Add(right);
            }

            return new Input(lefts.ToImmutableList(), rights.ToImmutableList());
       }
       public static int Solve1(Input input)
       {
              var lefts = input.lefts.Order();
              var rights = input.rights.Order();
              return lefts.Zip(rights).Select(x => Math.Abs(x.First-x.Second)).Sum();
       }
       
       
       public static int Solve2(Input input)
       {
              var lefts = input.lefts;
              var rights = input.rights.GroupBy(x => x).ToImmutableDictionary(x => x.Key, x => x.Count());
              return lefts.Select(l => l*rights.GetValueOrDefault(l, 0)).Sum();
       }
       
       //refactor later since solutions to task seem very similar
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