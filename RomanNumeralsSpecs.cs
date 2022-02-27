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
    public void Roman_letters_are_added_when_a_lower_value_follows_a_higher_value(int integer, string roman)
    {
        RomenNumeral.For(integer).Should().Be(roman);
    }

}

public static class RomenNumeral
{
    private record Temp(int Integer, string Roman);
    
    public static string For(int integer)
    {
        var temp = new Temp(integer, "");
        temp = temp.Contains(1000, 'M');
        temp = temp.Contains(500, 'D');
        temp = temp.Contains(100, 'C');
        temp = temp.Contains(50, 'L');
        temp = temp.Contains(10, 'X');
        temp = temp.Contains(5, 'V');
        temp = temp.Contains(1, 'I');
        return temp.Roman;
    }
    

    private static Temp Contains(this Temp integer, int @base, char letter)
    {
        var count = Contained(integer.Integer, @base);
        var roman = integer.Roman + count.Times(letter);
        return new Temp(integer.Integer - count*@base, roman);
    }
    
    private static int Contained(this int integer, int @base) => integer >= @base 
        ? integer / @base
        : 0;

    private static string Times(this int repeats, char letter) => new(Enumerable.Repeat(letter, repeats).ToArray());
}