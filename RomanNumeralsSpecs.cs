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
    
    [Fact]
    public void Integer_5_maps_onto_V()
    {
        RomenNumeral.For(5).Should().Be("V");
    }
}

public static class RomenNumeral
{
    public static string For(int i)
    {
        if (i == 5) return "V";
        return "I";
    }
}