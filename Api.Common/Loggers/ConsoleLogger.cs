using System;

namespace Api.Common.Loggers
{
    public class ConsoleLogger : IAppLogger
    {
        public void Error(Exception ex, string message)
        {
            Console.WriteLine(message);
            Console.WriteLine(ex.StackTrace.ToString());
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
        }
        public void Debug(string message)
        {
            Console.WriteLine(message);
        }
    }
}
