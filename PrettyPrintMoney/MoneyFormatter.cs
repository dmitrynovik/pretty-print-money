using System;
using System.Collections.Generic;
using System.Linq;

namespace PrettyPrintMoney
{
    class Power10
    {
        public int Magnitude { get; set; }
        public string Name { get; set; }
    }

    public static class MoneyFormatter
    {
        static readonly IDictionary<int, string> DigitMap = new Dictionary<int, string>()
            {
                {1, "one" },
                {2, "two" },
                {3, "three" },
                {4, "four" },
                {5, "five" },
                {6, "six" },
                {7, "seven" },
                {8, "eight" },
                {9, "nine" },
            };

        static readonly IDictionary<int, string> X10Map = new Dictionary<int, string>()
            {
                {1, "ten" },
                {2, "twenty" },
                {3, "thirty" },
                {4, "fourty" },
                {5, "fifty" },
                {6, "sixty" },
                {7, "seventy" },
                {8, "eightty" },
                {9, "ninety" },
            };

        static readonly IDictionary<int, Power10> Power10Map = new Dictionary<int, Power10>()
            {
                {2, new Power10 { Magnitude = 100, Name = "hundred" } },
                {3, new Power10 { Magnitude = 1000, Name = "thousand" } },
                {4, new Power10 { Magnitude = 1000, Name = "thousand" } },
                {5, new Power10 { Magnitude = 1000, Name = "thousand" } },
                {6, new Power10 { Magnitude = 1000000, Name = "million" } },
                {7, new Power10 { Magnitude = 1000000, Name = "million" } },
                {8, new Power10 { Magnitude = 1000000, Name = "million" } },
                {9, new Power10 { Magnitude = 1000000000, Name = "billion" } },
            };

        public static string FormatMoney(this double d)
        {
            const int maxValue = 2000000000;

            if (d < 0) throw new ArgumentException("please input non-negative number", nameof(d));
            if (d > maxValue) throw new ArgumentException("input too large", nameof(d));

            if (Math.Abs(d) < 0.001) // aka == 0 (FP)
                return "zero dollars";

            var intPart = (int)Math.Truncate(d);
            var floatPart = (int) (Math.Round(d - intPart, 2) * 100);

            var intStr = DoFormatMoney(intPart);
            var suffix = intPart == 1 ? "dollar" : "dollars";
            intStr = intStr.IsEmpty() ? intStr : $"{intStr} {suffix}";

            var floatStr = DoFormatMoney(floatPart);
            suffix = floatPart == 1 ? "cent" : "cents";
            floatStr = floatStr.IsEmpty() ? floatStr : $"{floatStr} {suffix}";

            if (intStr.IsEmpty() && floatStr.IsEmpty())
                return string.Empty;
            if (intStr.IsEmpty())
                return floatStr;
            if (floatStr.IsEmpty())
                return intStr;
            
            return $"{intStr} and {floatStr}";
        }

        private static string DoFormatMoney(int num)
        {
            var output = new List<string>();
            do
            {
                var power10 = (int)Math.Log10(num);

                if (Power10Map.ContainsKey(power10))
                {
                    var value = Power10Map[power10];
                    var currentNumber = num / value.Magnitude; // 7000 => 7, 35000 => 35
                    output.Add($"{DoFormatMoney(currentNumber)} {value.Name}"); // seven thousand etc...
                    num -= currentNumber * value.Magnitude;
                }
                else if (num > 0) 
                {
                    // ... we are small now - base case of recursion
                    if (num < 10 && num > 0)
                    {
                        output.Add(DigitMap[num]);
                    }
                    else
                    {
                        var x10 = num / 10;
                        var tens = X10Map[x10];
                        num -= 10 * x10;
                        var ones = DigitMap[num];
                        output.Add($"{tens} {ones}");
                    }
                    num = 0;
                }
            } while (num > 0);

            if (output.Count < 2)
                return output.FirstOrDefault() ?? string.Empty;

            return $"{string.Join(", ", output.Take(output.Count - 1))} and {output[output.Count - 1]}";
        }
    }
}
