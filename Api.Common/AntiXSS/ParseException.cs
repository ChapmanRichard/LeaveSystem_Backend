using System;

namespace Api.Common.AntiXSS
{
    public class ParseException : Exception
    {
        public ParseException(string message)
            : base(message)
        {
        }

        public ParseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
