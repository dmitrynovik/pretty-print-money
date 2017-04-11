using System;
using System.Collections.Generic;
using System.Linq;

namespace PrettyPrintMoney
{
    public static class MoneyFormatter
    {
        public static string FormatMoney(this double d)
        {
            if (d <= 0) throw new ArgumentException("please input positive number", nameof(d));

            var intPart = (int)Math.Truncate(d);
            var floatPart = Math.Round(d - intPart, 2);

            var intStr = DoFormatMoney(intPart);
            intStr = intStr.IsEmpty() ? intStr : $"{intStr} dollars";

            var floatStr = DoFormatMoney((int)(floatPart * 100));
            floatStr = floatStr.IsEmpty() ? floatStr : $"{floatStr} cents";

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
            var digitMap = new Dictionary<int, string>()
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

            var x10Map = new Dictionary<int, string>()
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

            var orderMap = new Dictionary<int, string>()
            {
                {2, "hundred" },
                {3, "thousand" },
                {6, "million" },
                {9, "billion" },
            };

            var output = new List<string>();
            do
            {
                var order = (int)Math.Log10(num);

                if (orderMap.ContainsKey(order))
                {
                    var orderValue = (int)Math.Pow(10, order); // 7000 => 1000
                    var currentDigit = num/orderValue; // 7000 => 7
                    output.Add($"{digitMap[currentDigit]} {orderMap[order]}"); // seven thousand
                    num -= currentDigit * orderValue;
                }
                else
                {
                    if (num < 10 && num > 0)
                    {
                        output.Add(digitMap[num]);
                        num = 0;
                    }
                    else if (num > 0)
                    {
                        var x10 = num / 10;
                        var tens = x10Map[x10];
                        num -= 10 * x10;
                        var ones = digitMap[num];
                        output.Add($"{tens} {ones}");
                        num = 0;
                    }
                }
            } while (num > 0);

            if (output.Count < 2)
                return output.FirstOrDefault() ?? string.Empty;

            return $"{string.Join(", ", output.Take(output.Count - 1))} and {output[output.Count - 1]}";
        }
    }
}
