using System;

namespace CurrencyConverter.Exceptions
{
    public class NoPathException:Exception
    {
        public NoPathException()
        {
        }

        public NoPathException(string message)
            : base(message)
        {
        }

        public NoPathException(string @from, string @to)
           : base($"No Path Found From {from} To {to}")
        {
        }

        public NoPathException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
