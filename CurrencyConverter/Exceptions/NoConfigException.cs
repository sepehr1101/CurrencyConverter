using System;

namespace CurrencyConverter.Exceptions
{
    public class NoConfigException : Exception
    {
        public NoConfigException()
        {
        }

        public NoConfigException(string item)
            : base($"{item} is null or empty in currency configuration")
        {
        }
        public NoConfigException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
