using System;

namespace Luminous.Code.Exceptions.ExceptionExtensions
{
    public static class ExceptionExtensions
    {
        public static string ExtendedMessage(this Exception instance)
        {
            if (instance == null) return "";

            var innerException = instance.InnerException;

            return (innerException == null)
                ? instance.Message
                : innerException.ExtendedMessage();
        }
    }
}