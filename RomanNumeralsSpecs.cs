using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace RomanNumerals;

public class RomanNumeralsSpecs
{
    [Theory]
    [InlineData(1, 'I')]
    [InlineData(5, 'V')]
    [InlineData(10, 'X')]
    [InlineData(50, 'L')]
    [InlineData(100, 'C')]
    [InlineData(500, 'D')]
    [InlineData(1000, 'M')]
    public void Integer_maps_onto_character(int integer, char roman)
    {
        RomenNumeral.For(integer).Should().Be(roman.ToString());
    }
    
    [Theory]
    [InlineData(1, 'I')]
    public void I_may_be_repeated_up_to_three_times(int integer, char roman)
    {
        RomenNumeral.For(2*integer).Should().Be($"{roman}{roman}");
        RomenNumeral.For(3*integer).Should().Be($"{roman}{roman}{roman}");
    }
}

public static class RomenNumeral
{
    public static string For(int integer)
    {
        if (integer == 5) return "V";
        if (integer == 10) return "X";
        if (integer == 50) return "L";
        if (integer == 100) return "C";
        if (integer == 500) return "D";
        if (integer == 1000) return "M";
        return integer.Times('I');
    }

    private static string Times(this int repeats, char letter)
    {
        return new string(Enumerable.Repeat(letter, repeats).ToArray());
    }
}