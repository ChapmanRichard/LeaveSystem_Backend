using System;

namespace Api.Common.Loggers
{
    public interface IAppLogger
    {
        void Info(string message);


        void Error(Exception ex, string message);
        void Debug(string message);
    }
}
