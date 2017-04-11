namespace PrettyPrintMoney
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string s) => string.IsNullOrWhiteSpace(s);
    }
}
