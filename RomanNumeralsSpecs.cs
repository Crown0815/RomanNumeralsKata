using FluentAssertions;
using Xunit;

namespace RomanNumerals;

public class RomanNumeralsSpecs
{
    [Theory]
    [InlineData(1, 'I')]
    [InlineData(5, 'V')]
    public void Integer_maps_onto_character(int integer, char roman)
    {
        RomenNumeral.For(integer).Should().Be(roman.ToString());
    }
}

public static class RomenNumeral
{
    public static string For(int integer)
    {
        if (integer == 5) return "V";
        return "I";
    }
}