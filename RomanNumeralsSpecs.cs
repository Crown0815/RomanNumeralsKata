using FluentAssertions;
using Xunit;

namespace RomanNumerals;

public class RomanNumeralsSpecs
{
    [Fact]
    public void Integer_1_maps_onto_I()
    {
        RomenNumeral.For(1).Should().Be("I");
    }
}

public static class RomenNumeral
{
    public static string For(int i)
    {
        return "I";
    }
}