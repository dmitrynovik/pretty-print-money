using System;
using PrettyPrintMoney;

namespace PrettyPrintMoney.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    System.Console.ForegroundColor = ConsoleColor.Cyan;
                    var str = System.Console.ReadLine();
                    double money;
                    if (double.TryParse(str, out money))
                        System.Console.WriteLine(money.FormatMoney());
                    else
                        PrintError();
                }
                catch (Exception)
                {
                    PrintError();
                }
            }
        }

        static void PrintError()
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("Expected input: 0.00 to 2000000000.00");
        }
    }
}
