using System;
using System.ComponentModel.DataAnnotations;
using Api.Common.Exceptions;
using Api.Common.Queries;
using Api.Contract.Queries;
using Api.Web;
using Microsoft.AspNetCore.Mvc;

namespace Api.Web.Controllers;

[ApiController]
[Route("api/v1/quotas")]
public class QuotaController : BaseController
{
    public QuotaController(IQueryProcessor queryProcessor, IWebCommandBus commandBus) : base(queryProcessor, commandBus)
    {
    }

    [HttpGet("my")]
    public IActionResult MyQuota(
        [FromHeader(Name = "X-User-Name")] string? userName,
        [FromHeader(Name = "X-User-Role")] string? userRole)
    {
        if (!TryGetEmployeeContext(userName, userRole, out var errorResult))
        {
            return errorResult!;
        }

        var query = new GetMyLeaveQuotaQuery
        {
            Username = userName
        };

        try
        {
            var result = QueryProcessor.ProcessSingleQuery(query);
            return Ok(new
            {
                code = "0",
                message = "success",
                data = result,
                traceId = HttpContext.TraceIdentifier
            });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new
            {
                code = "VALIDATION_FAILED",
                message = ex.Message,
                errors = new[] { new { field = "request", detail = ex.Message } },
                traceId = HttpContext.TraceIdentifier
            });
        }
        catch (LeaveApiException ex)
        {
            return StatusCode(ex.StatusCode, new
            {
                code = ex.Code,
                message = ex.Message,
                errors = string.IsNullOrWhiteSpace(ex.Field)
                    ? null
                    : new[] { new { field = ex.Field, detail = ex.Detail ?? ex.Message } },
                traceId = HttpContext.TraceIdentifier
            });
        }
    }

    private bool TryGetEmployeeContext(string? userName, string? userRole, out IActionResult? errorResult)
    {
        if (!string.Equals(userRole, "Employee", StringComparison.OrdinalIgnoreCase))
        {
            errorResult = StatusCode(403, new
            {
                code = "FORBIDDEN",
                message = "Employee role required",
                errors = new[] { new { field = "X-User-Role", detail = "Employee role required" } },
                traceId = HttpContext.TraceIdentifier
            });
            return false;
        }

        if (string.IsNullOrWhiteSpace(userName))
        {
            errorResult = BadRequest(new
            {
                code = "VALIDATION_FAILED",
                message = "X-User-Name is required",
                errors = new[] { new { field = "X-User-Name", detail = "header is required" } },
                traceId = HttpContext.TraceIdentifier
            });
            return false;
        }

        errorResult = null;
        return true;
    }
}