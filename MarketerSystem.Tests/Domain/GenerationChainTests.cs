using MarketerSystem.Domain.Policies;
using Shouldly;

namespace MarketerSystem.Tests.Domain;

public class GenerationChainTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Parse_NullOrWhitespace_ReturnsEmpty(string? chain)
    {
        GenerationChain.Parse(chain).ShouldBeEmpty();
    }

    [Fact]
    public void Parse_ValidChain_ReturnsIdsRootFirst()
    {
        GenerationChain.Parse("1,2,3").ShouldBe([1, 2, 3]);
    }

    [Fact]
    public void Parse_SkipsInvalidAndEmptyEntries()
    {
        GenerationChain.Parse("1,abc,, 3 ").ShouldBe([1, 3]);
    }

    [Fact]
    public void Append_NoParentChain_ReturnsParentId()
    {
        GenerationChain.Append(null, 5).ShouldBe("5");
    }

    [Fact]
    public void Append_ExistingChain_AppendsParentLast()
    {
        GenerationChain.Append("1,2", 7).ShouldBe("1,2,7");
    }

    [Theory]
    [InlineData(null, 0)]
    [InlineData("4", 1)]
    [InlineData("1,2,3", 3)]
    public void Depth_CountsChainEntries(string? chain, int expected)
    {
        GenerationChain.Depth(chain).ShouldBe(expected);
    }

    [Theory]
    [InlineData("1,2,3,4", false)]
    [InlineData("1,2,3,4,5", true)]
    [InlineData("1,2,3,4,5,6", true)] // corrupt over-length chain still blocks
    public void IsAtMaxDepth_BlocksAtFiveOrMore(string chain, bool expected)
    {
        GenerationChain.IsAtMaxDepth(chain).ShouldBe(expected);
    }
}
