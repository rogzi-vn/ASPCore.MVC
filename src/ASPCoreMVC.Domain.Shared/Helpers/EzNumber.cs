using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ASPCoreMVC.Helpers
{
    public static class EzNumber
    {
        private static List<string> romanNumerals = new List<string>() { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
        private static List<int> numerals = new List<int>() { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
        private static string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string ToRomanNumeral(int number)
        {
            var romanNumeral = string.Empty;
            while (number > 0)
            {
                // find biggest numeral that is less than equal to number
                var index = numerals.FindIndex(x => x <= number);
                // subtract it's value from your number
                number -= numerals[index];
                // tack it onto the end of your roman numeral
                romanNumeral += romanNumerals[index];
            }
            return romanNumeral;
        }

        public static string ToAlphabetNumeral(int number)
        {
            return alphabet[number % alphabet.Length].ToString();
        }

        public static string WithCommas(this string inp)
        {
            string pattern = @"\B(?=(\d{3})+(?!\d))";
            return Regex.Replace(inp, pattern, ",");
        }
        public static string WithoutCommas(this string inp)
        {
            return inp.Replace(",", "");
        }
    }
}
