using System;
using System.Collections.Generic;
using System.Linq;

namespace PrettyPrintMoney
{
    class Power10
    {
        public int Order { get; set; }
        public string Name { get; set; }
    }

    public static class MoneyFormatter
    {
        static IDictionary<int, string> digitMap = new Dictionary<int, string>()
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

        static IDictionary<int, string> x10Map = new Dictionary<int, string>()
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

        static IDictionary<int, Power10> orderMap = new Dictionary<int, Power10>()
            {
                {2, new Power10 { Order = 100, Name = "hundred" } },
                {3, new Power10 { Order = 1000, Name = "thousand" } },
                {4, new Power10 { Order = 1000, Name = "thousand" } },
                {5, new Power10 { Order = 1000, Name = "thousand" } },
                {6, new Power10 { Order = 1000000, Name = "million" } },
                {7, new Power10 { Order = 1000000, Name = "million" } },
                {8, new Power10 { Order = 1000000, Name = "million" } },
                {9, new Power10 { Order = 1000000000, Name = "billion" } },
            };

        public static string FormatMoney(this double d)
        {
            const int maxValue = 2000000000;

            if (d < 0) throw new ArgumentException("please input non-negative number", nameof(d));
            if (d > maxValue) throw new ArgumentException("input too large", nameof(d));

            if (d == 0)
                return "zero dollars";

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
            var output = new List<string>();
            do
            {
                var order = (int)Math.Log10(num);

                if (orderMap.ContainsKey(order))
                {
                    var value = orderMap[order];
                    var currentNumber = num/value.Order; // 7000 => 7, 35000 => 35
                    output.Add($"{DoFormatMoney(currentNumber)} {value.Name}"); // seven thousand
                    num -= currentNumber * value.Order;
                }
                else if (num > 0)
                {
                    output.Add(DoPrettyPrintCurrent(num));
                    num = 0;
                }
            } while (num > 0);

            if (output.Count < 2)
                return output.FirstOrDefault() ?? string.Empty;

            return $"{string.Join(", ", output.Take(output.Count - 1))} and {output[output.Count - 1]}";
        }

        private static string DoPrettyPrintCurrent(int num)
        {
            if (num < 10 && num > 0)
            {
                return digitMap[num];
            }
            var x10 = num / 10;
            var tens = x10Map[x10];
            num -= 10 * x10;
            var ones = digitMap[num];
            return $"{tens} {ones}";
        }
    }
}
