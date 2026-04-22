using System;

namespace Api.Common.Exceptions;

public class LeaveApiException : Exception
{
    public LeaveApiException(string code, string message, int statusCode = 400, string? field = null, string? detail = null)
        : base(message)
    {
        Code = code;
        StatusCode = statusCode;
        Field = field;
        Detail = detail;
    }

    public string Code { get; }

    public int StatusCode { get; }

    public string? Field { get; }

    public string? Detail { get; }
}