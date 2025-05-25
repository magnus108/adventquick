using System.Collections.Immutable;
using Advent2.Tasks2;
using FluentAssertions;

namespace TestProject1;

public class UnitTest2
{
    //would benifit from parameterized tests
    [Fact]
    public void Test1()
    {
        var input = ImmutableList.Create<int>(7,6, 4, 2,1);
        var unvalidatedLevel = new UnvalidatedLevel(input);
        var result = Task2.ParseSafeLevel(unvalidatedLevel, x => x.level, () => []);
        result.Should().BeEquivalentTo(input);
    }
    
    
	public static TheoryData<String, ImmutableList<int>, bool> TestData() =>
		ImmutableList.Create(
				(Description: "Case 1", Input: ImmutableList.Create(7,6, 4, 2,1), Expected: true),
				(Description: "Case 2", Input: ImmutableList.Create(1,2,7,8,9), Expected: false),
				(Description: "Case 3", Input: ImmutableList.Create(9,7,6,2,1), Expected: false),
				(Description: "Case 4", Input: ImmutableList.Create(1,3,2,4,5), Expected: false),
				(Description: "Case 5", Input: ImmutableList.Create(8,6,4,4,1), Expected: false),
				(Description: "Case 6", Input: ImmutableList.Create(1,3,6,7,9), Expected: true)
			)
			.Aggregate(
				new TheoryData<string, ImmutableList<int>, bool>(),
				(data, tuple) =>
				{
					data.Add(tuple.Description, tuple.Input, tuple.Expected);
					return data;
				}
			);

	[Theory]
	[MemberData(nameof(TestData), MemberType = typeof(UnitTest2))]
	public void part1(
		string description,
		ImmutableList<int> input,
		bool expected)
	{
        var unvalidatedLevel = new UnvalidatedLevel(input);
        var result = Task2.ParseSafeLevel(unvalidatedLevel, x => true, () => false);
        result.Should().Be(expected, description);
	}
	
	public static TheoryData<String, ImmutableList<int>, bool> TestData2() =>
		ImmutableList.Create(
				(Description: "Case 1", Input: ImmutableList.Create(7,6, 4, 2,1), Expected: true),
				(Description: "Case 2", Input: ImmutableList.Create(1,2,7,8,9), Expected: false),
				(Description: "Case 3", Input: ImmutableList.Create(9,7,6,2,1), Expected: false),
				(Description: "Case 4", Input: ImmutableList.Create(1,3,2,4,5), Expected: true),
				(Description: "Case 5", Input: ImmutableList.Create(8,6,4,4,1), Expected: true),
				(Description: "Case 6", Input: ImmutableList.Create(1,3,6,7,9), Expected: true)
			)
			.Aggregate(
				new TheoryData<string, ImmutableList<int>, bool>(),
				(data, tuple) =>
				{
					data.Add(tuple.Description, tuple.Input, tuple.Expected);
					return data;
				}
			);

	[Theory]
	[MemberData(nameof(TestData2), MemberType = typeof(UnitTest2))]
	public void part2(
		string description,
		ImmutableList<int> input,
		bool expected)
	{
        var unvalidatedLevel = new UnvalidatedLevel(input);
        var result = Task2.ParseSafeLevel(unvalidatedLevel, x => true, () => false);
        result.Should().Be(expected, description);
	}
    
}