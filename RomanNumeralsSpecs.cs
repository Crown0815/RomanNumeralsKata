using System.Linq;
using FluentAssertions;
using Humanizer;
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
        RomanNumeral.For(integer).Should().Be(roman.ToString());
    }
    
    [Theory]
    [InlineData(1, 'I')]
    [InlineData(10, 'X')]
    [InlineData(100, 'C')]
    [InlineData(1000, 'M')]
    public void I_X_C_and_M_letters_may_be_repeated_up_to_three_times(int integer, char roman)
    {
        RomanNumeral.For(2*integer).Should().Be($"{roman}{roman}");
        RomanNumeral.For(3*integer).Should().Be($"{roman}{roman}{roman}");
    }
    
    [Theory]
    [InlineData(11, "XI")]
    [InlineData(111, "CXI")]
    [InlineData(1111, "MCXI")]
    [InlineData(3333, "MMMCCCXXXIII")]
    [InlineData(3555, "MMMDLV")]
    public void Roman_letters_are_added_when_a_lower_value_follows_a_higher_value(int integer, string roman)
    {
        RomanNumeral.For(integer).Should().Be(roman);
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
        RomanNumeral.For(integer).Should().Be(roman);
    }
    

    [Fact]
    public void Validation_against_library_method()
    {
        for (int i = 1; i < 4000; i++) RomanNumeral.For(i).Should().Be(i.ToRoman());
    }
}

public static class RomanNumeral
{
    private record Map(int Integer, string Letter, Map? Subtrahend = null)
    {
        public Map Lowered() => Subtrahend is null ? this : this - Subtrahend!;
        public static Map operator -(Map one, Map two) => new(one.Integer - two.Integer, $"{two.Letter}{one.Letter}");
        public static Map operator *(int count, Map map) => new(count * map.Integer, count.Times(map.Letter));
    }

    private static readonly Map I = new(1, "I");
    private static readonly Map V = new(5, "V", I);
    private static readonly Map X = new(10, "X", I);
    private static readonly Map L = new(50, "L", X);
    private static readonly Map C = new(100, "C", X);
    private static readonly Map D = new(500, "D", C);
    private static readonly Map M = new(1000, "M", C);
    
    
    private record Buffer(int Rest, string Roman);
    
    public static string For(int integer) => new Buffer(integer, "")
            .ApplyAll(M, D, C, L, X, V, I)
            .Roman;

    private static Buffer ApplyAll(this Buffer buffer, params Map[] maps) => maps.Aggregate(buffer, ApplyWithLowering);

    private static Buffer ApplyWithLowering(this Buffer buffer, Map map) => buffer
        .ApplyMultiplesOf(map)
        .ApplyMultiplesOf(map.Lowered());

    private static Buffer ApplyMultiplesOf(this Buffer buffer, Map map) => buffer.Apply(map.CountIn(buffer) * map);
    
    private static Buffer Apply(this Buffer buffer, Map map) => new(buffer.Rest - map.Integer, buffer.Roman + map.Letter);

    
    private static int CountIn(this Map map, Buffer buffer) => map.Integer.CountIn(buffer.Rest);
    private static int CountIn(this int @base, int value) => value >= @base 
        ? value / @base
        : 0;

    private static string Times(this int repeats, string letter) => string.Join("", Enumerable.Repeat(letter, repeats));
}