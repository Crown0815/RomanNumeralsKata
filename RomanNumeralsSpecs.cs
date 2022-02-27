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
    public void Integer_maps_onto_roman_letter(int integer, char roman)
    {
        RomenNumeral.For(integer).Should().Be(roman.ToString());
    }
    
    [Theory]
    [InlineData(1, 'I')]
    [InlineData(10, 'X')]
    [InlineData(100, 'C')]
    [InlineData(1000, 'M')]
    public void I_X_C_and_M_letters_may_be_repeated_up_to_three_times(int integer, char roman)
    {
        RomenNumeral.For(2*integer).Should().Be($"{roman}{roman}");
        RomenNumeral.For(3*integer).Should().Be($"{roman}{roman}{roman}");
    }
    
    [Theory]
    [InlineData(11, "XI")]
    [InlineData(111, "CXI")]
    [InlineData(1111, "MCXI")]
    [InlineData(3333, "MMMCCCXXXIII")]
    [InlineData(3555, "MMMDLV")]
    public void Roman_letters_are_added_when_a_lower_value_follows_a_higher_value(int integer, string roman)
    {
        RomenNumeral.For(integer).Should().Be(roman);
    }
    
    
    [Theory]
    [InlineData(4, "IV")]
    [InlineData(9, "IX")]
    [InlineData(40, "XL")]
    [InlineData(90, "XC")]
    [InlineData(400, "CD")]
    [InlineData(900, "CM")]
    [InlineData(3949, "MMMCMXLIX")]
    [InlineData(3494, "MMMCDXCIV")]
    public void Roman_letters_are_subtracted_when_a_higher_value_follows_a_lower_value(int integer, string roman)
    {
        RomenNumeral.For(integer).Should().Be(roman);
    }
}

public static class RomenNumeral
{
    private record Temp(int Integer, string Roman);
    
    public static string For(int integer)
    {
        return new Temp(integer, "")
            .Minus(1000, 'M', 100, 'C')
            .Minus(500, 'D', 100, 'C')
            .Minus(100, 'C', 10, 'X')
            .Minus(50, 'L', 10, 'X')
            .Minus(10, 'X', 1, 'I')
            .Minus(5, 'V', 1, 'I')
            .Minus(1, "I")
            .Roman;
    }
    

    private static Temp Minus(this Temp value, int integer, char letter, int subtrahend, char subtrahendLetter)
    {
        return value
            .Minus(integer, $"{letter}")
            .Minus(integer-subtrahend, $"{subtrahendLetter}{letter}");
    }

    private static Temp Minus(this Temp value, int integer, string letter)
    {
        var count = Contained(value.Integer, integer);
        var roman = value.Roman + count.Times(letter);
        return new Temp(value.Integer - count*integer, roman);
    }
    
    private static int Contained(this int integer, int @base) => integer >= @base 
        ? integer / @base
        : 0;

    private static string Times(this int repeats, string letter) => string.Join("", Enumerable.Repeat(letter, repeats));
}