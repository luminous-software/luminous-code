using static System.Environment;

namespace Luminous.Code.Strings
{
    public static class Concatenation
    {
        //***

        public static string JoinStrings(string first, string second, string separator = null)
        {
            if (separator == null) separator = NewLine + NewLine;

            var separatorText = (first == null || second == null)
                ? ""
                : separator;

            return $"{first}{separatorText}{second}";
        }

        //***
    }
}