using System;
using System.Collections.Generic;
using System.Linq;

namespace AliceHook
{
    public static class Utils
    {
        public static bool ContainsStartWith(this IEnumerable<string> list, string start)
        {
            return list.Any(element => element.ToLower().Trim().StartsWith(start));
        }

        public static string CapitalizeFirst(this string s)
        {
            return s.IsNullOrEmpty() ? "" : s.Substring(0, 1).ToUpper() + s.Substring(1);
        }

        public static string SafeSubstring(this string s, int length)
        {
            if (s.Length <= length)
            {
                return s;
            }

            return s.Substring(0, length);
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static string Join(this IEnumerable<string> list, string separator)
        {
            return string.Join(separator, list);
        }
        
        public static int LevenshteinDistance(string a, string b)
        {
            if (string.IsNullOrEmpty(a) && string.IsNullOrEmpty(b))
            {
                return 0;
            }
            if (string.IsNullOrEmpty(a))
            {
                return b.Length;
            }
            if (string.IsNullOrEmpty(b))
            {
                return a.Length;
            }
            var lengthA = a.Length;
            var lengthB = b.Length;
            var distances = new int[lengthA + 1, lengthB + 1];
            for (var i = 0; i <= lengthA; distances[i, 0] = i++) ;
            for (var j = 0; j <= lengthB; distances[0, j] = j++) ;

            for (var i = 1; i <= lengthA; i++)
            for (var j = 1; j <= lengthB; j++)
            {
                var cost = b[j - 1] == a[i - 1] ? 0 : 1;
                distances[i, j] = Math.Min
                (
                    Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                    distances[i - 1, j - 1] + cost
                );
            }
            return distances[lengthA, lengthB];
        }

        public static double LevenshteinRatio(string a, string b)
        {
            var maxLen = Math.Max(a.Length, b.Length);
            if (maxLen == 0)
            {
                return 1.0;
            }

            var levDist = LevenshteinDistance(a, b);
            return (double) levDist / maxLen;
        }
        
        public static string GetNumericPhrase(int num, string one, string few, string many)
        {
            num = num < 0 ? 0 : num;
            string postfix;

            if (num < 10)
            {
                if (num == 1) postfix = one;
                else if (num > 1 && num < 5) postfix = few;
                else postfix = many;
            }
            else if (num <= 20)
            {
                postfix = many;
            }
            else if (num <= 99)
            {
                var lastOne = num - ((int)Math.Floor((double)num / 10)) * 10;
                postfix = GetNumericPhrase(lastOne, one, few, many);
            }
            else
            {
                var lastTwo = num - ((int)Math.Floor((double)num / 100)) * 100;
                postfix = GetNumericPhrase(lastTwo, one, few, many);
            }
            return postfix;
        }

        public static string ToPhrase(this int num, string one, string few, string many)
        {
            return num + " " + GetNumericPhrase(num, one, few, many);
        }

        public static int OptimalSkipLength(string matchPhrase, string[] tokens)
        {
            var minDist = int.MaxValue;
            var matchTokens = matchPhrase.Split(" ");
            var matchShort = matchTokens.Join("");
            var bestSkip = matchTokens.Length;
                
            for (var i = 1; i <= matchTokens.Length + 1; i++)
            {
                var secondString = tokens.Take(i).Join("");
                var dist = LevenshteinDistance(matchShort, secondString);
                if (dist < minDist)
                {
                    minDist = dist;
                    bestSkip = i;
                }
            }

            return bestSkip;
        }

        public static double PossibleRatio(int len)
        {
            if (len <= 6)
            {
                return 0.15;
            } 
            if (len <= 10)
            {
                return 0.25;
            }

            if (len <= 15)
            {
                return 0.35;
            }

            return 0.4;
        }
    }
}