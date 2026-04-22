using System;
using System.ComponentModel.DataAnnotations;
using Api.Common.Commands;
using Api.Common.Exceptions;
using Api.Common.Queries;
using Api.Contract.Commands;
using Api.Contract.Queries;
using Api.Web;
using Microsoft.AspNetCore.Mvc;

namespace Api.Web.Controllers;

[ApiController]
[Route("api/v1/leaves")]
public class LeaveController : BaseController
{
    public LeaveController(IQueryProcessor queryProcessor, IWebCommandBus commandBus) : base(queryProcessor, commandBus)
    {
    }

    [HttpPost]
    public IActionResult CreateDraft(
        [FromBody] CreateLeaveApplicationCommand command,
        [FromHeader(Name = "X-User-Name")] string? userName,
        [FromHeader(Name = "X-User-Role")] string? userRole)
    {
        if (!TryGetEmployeeContext(userName, userRole, out var errorResult))
        {
            return errorResult!;
        }

        command.Username = userName;
        return ExecuteCommand(command, result => new
        {
            code = "0",
            message = "success",
            data = new
            {
                id = $"L{result.LeaveRequestId:D12}",
                status = result.Status
            },
            traceId = HttpContext.TraceIdentifier
        });
    }

    [HttpPut("{leaveId:regex(^L\\d+$)}")]
    public IActionResult Edit(
        string leaveId,
        [FromBody] EditLeaveApplicationCommand command,
        [FromHeader(Name = "X-User-Name")] string? userName,
        [FromHeader(Name = "X-User-Role")] string? userRole)
    {
        if (!TryGetEmployeeContext(userName, userRole, out var errorResult))
        {
            return errorResult!;
        }

        command.LeaveId = leaveId;
        command.Username = userName;
        return ExecuteCommand(command, result => new
        {
            code = "0",
            message = "success",
            data = new
            {
                id = result.Id,
                status = result.Status,
                updatedAt = result.UpdatedAt
            },
            traceId = HttpContext.TraceIdentifier
        });
    }

    [HttpDelete("{leaveId:regex(^L\\d+$)}")]
    public IActionResult Delete(
        string leaveId,
        [FromHeader(Name = "X-User-Name")] string? userName,
        [FromHeader(Name = "X-User-Role")] string? userRole)
    {
        if (!TryGetEmployeeContext(userName, userRole, out var errorResult))
        {
            return errorResult!;
        }

        var command = new DeleteLeaveApplicationCommand
        {
            LeaveId = leaveId,
            Username = userName
        };

        return ExecuteCommand(command, _ => new
        {
            code = "0",
            message = "success",
            data = new { deleted = true },
            traceId = HttpContext.TraceIdentifier
        });
    }

    [HttpPost("{leaveId:regex(^L\\d+$)}/submit")]
    public IActionResult Submit(
        string leaveId,
        [FromHeader(Name = "X-User-Name")] string? userName,
        [FromHeader(Name = "X-User-Role")] string? userRole)
    {
        if (!TryGetEmployeeContext(userName, userRole, out var errorResult))
        {
            return errorResult!;
        }

        var command = new SubmitLeaveApplicationCommand
        {
            LeaveId = leaveId,
            Username = userName
        };

        return ExecuteCommand(command, result => new
        {
            code = "0",
            message = "success",
            data = new
            {
                id = result.Id,
                status = result.Status
            },
            traceId = HttpContext.TraceIdentifier
        });
    }

    [HttpPost("{leaveId:regex(^L\\d+$)}/cancel")]
    public IActionResult Cancel(
        string leaveId,
        [FromBody] CancelLeaveApplicationCommand command,
        [FromHeader(Name = "X-User-Name")] string? userName,
        [FromHeader(Name = "X-User-Role")] string? userRole)
    {
        if (!TryGetEmployeeContext(userName, userRole, out var errorResult))
        {
            return errorResult!;
        }

        command.LeaveId = leaveId;
        command.Username = userName;
        return ExecuteCommand(command, result => new
        {
            code = "0",
            message = "success",
            data = new
            {
                id = result.Id,
                status = result.Status
            },
            traceId = HttpContext.TraceIdentifier
        });
    }

    [HttpGet("my")]
    public IActionResult MyList(
        [FromQuery] GetMyLeaveListQuery query,
        [FromHeader(Name = "X-User-Name")] string? userName,
        [FromHeader(Name = "X-User-Role")] string? userRole)
    {
        if (!TryGetEmployeeContext(userName, userRole, out var errorResult))
        {
            return errorResult!;
        }

        query.Username = userName;
        return ExecuteQuery(query, result => new
        {
            code = "0",
            message = "success",
            data = new
            {
                items = result.Items,
                pageNo = result.PageNo,
                pageSize = result.PageSize,
                total = result.Total
            },
            traceId = HttpContext.TraceIdentifier
        });
    }

    [HttpGet("{leaveId:regex(^L\\d+$)}")]
    public IActionResult MyDetail(
        string leaveId,
        [FromHeader(Name = "X-User-Name")] string? userName,
        [FromHeader(Name = "X-User-Role")] string? userRole)
    {
        if (!TryGetEmployeeContext(userName, userRole, out var errorResult))
        {
            return errorResult!;
        }

        var query = new GetMyLeaveDetailQuery
        {
            LeaveId = leaveId,
            Username = userName
        };

        return ExecuteQuery(query, result => new
        {
            code = "0",
            message = "success",
            data = result,
            traceId = HttpContext.TraceIdentifier
        });
    }

    private IActionResult ExecuteCommand<TResult>(ICommand<TResult> command, Func<TResult, object> onSuccess)
    {
        try
        {
            var result = CommandBus.SubmitAndReturnResult(command);
            return Ok(onSuccess(result));
        }
        catch (ValidationException ex)
        {
            return BuildError(400, "VALIDATION_FAILED", ex.Message, "request", ex.Message);
        }
        catch (LeaveApiException ex)
        {
            return BuildError(ex.StatusCode, ex.Code, ex.Message, ex.Field, ex.Detail ?? ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BuildError(403, "FORBIDDEN", ex.Message, "user", ex.Message);
        }
    }

    private IActionResult ExecuteQuery<TResult>(ISingleQuery<TResult> query, Func<TResult, object> onSuccess)
    {
        try
        {
            var result = QueryProcessor.ProcessSingleQuery(query);
            return Ok(onSuccess(result));
        }
        catch (ValidationException ex)
        {
            return BuildError(400, "VALIDATION_FAILED", ex.Message, "request", ex.Message);
        }
        catch (LeaveApiException ex)
        {
            return BuildError(ex.StatusCode, ex.Code, ex.Message, ex.Field, ex.Detail ?? ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BuildError(403, "FORBIDDEN", ex.Message, "user", ex.Message);
        }
    }

    private IActionResult BuildError(int statusCode, string code, string message, string? field, string detail)
    {
        object? errors = string.IsNullOrWhiteSpace(field)
            ? null
            : new[] { new { field, detail } };

        return StatusCode(statusCode, new
        {
            code,
            message,
            errors,
            traceId = HttpContext.TraceIdentifier
        });
    }

    private IActionResult Forbidden(string field, string message)
    {
        return StatusCode(403, new
        {
            code = "FORBIDDEN",
            message,
            errors = new[] { new { field, detail = message } },
            traceId = HttpContext.TraceIdentifier
        });
    }

    private IActionResult ValidationError(string field, string message)
    {
        return BadRequest(new
        {
            code = "VALIDATION_FAILED",
            message,
            errors = new[] { new { field, detail = message } },
            traceId = HttpContext.TraceIdentifier
        });
    }

    private bool TryGetEmployeeContext(string? userName, string? userRole, out IActionResult? errorResult)
    {
        if (!string.Equals(userRole, "Employee", StringComparison.OrdinalIgnoreCase))
        {
            errorResult = Forbidden("X-User-Role", "Employee role required");
            return false;
        }

        if (string.IsNullOrWhiteSpace(userName))
        {
            errorResult = ValidationError("X-User-Name", "X-User-Name is required");
            return false;
        }

        errorResult = null;
        return true;
    }
}
