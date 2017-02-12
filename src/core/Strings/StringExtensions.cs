using System;
using System.ComponentModel;

namespace Luminous.Code.Strings.Extensions
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
    }
}