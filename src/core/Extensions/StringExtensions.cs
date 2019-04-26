using System;
using System.ComponentModel;
using static System.Environment;

namespace Luminous.Code.Extensions.StringExtensions
{
    public static class StringExtensions
    {
        public static T To<T>(this string instance)
        {
            //http://www.hanselman.com/blog/TypeConvertersTheresNotEnoughTypeDescripterGetConverterInTheWorld.aspx

            try
            {
                var type = TypeDescriptor.GetConverter(typeof(T));

                return (T)(type.ConvertFromInvariantString(instance));
            }
            catch (Exception)
            {
                return default(T);
            }
        }
        public static string JoinWith(this string first, string second, string separator = null)
        {
            if (separator == null) separator = NewLine + NewLine;

            var separatorText = (first == null || second == null)
                ? ""
                : separator;

            return $"{first}{separatorText}{second}";
        }
    }
}