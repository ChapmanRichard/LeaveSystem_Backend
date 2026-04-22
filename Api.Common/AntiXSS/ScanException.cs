using System;

namespace Api.Common.AntiXSS
{
    public class ScanException : Exception
    {
        public ScanException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ScanException(string message)
            : base(message)
        {
        }
    }
}
