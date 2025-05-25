using Advent2.Tasks1;
using FluentAssertions;

namespace TestProject1;

public class UnitTest1
{
    //would benifit from parameterized tests
    [Fact]
    public void Test1()
    {
        var input = new Input([3, 4, 2, 1, 3, 3], [4, 3, 5, 3, 9, 3]);
        var result = Task1.Solve1(input);
        result.Should().Be(11);
    }
    
    [Fact]
    public void Test2()
    {
        var input = new Input([3, 4, 2, 1, 3, 3], [4, 3, 5, 3, 9, 3]);
        var result = Task1.Solve2(input);
        result.Should().Be(31);
    }
    
}