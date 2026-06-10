using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Api.Common.Commands;

namespace Api.Contract.Commands;

public class EditLeaveApplicationCommand : ICommand<EditLeaveApplicationResult>
{
    [Required]
    public required string LeaveId { get; set; }

    [Required]
    [RegularExpression("^(Annual|Sick|Personal|Compensatory)$")]
    public required string LeaveType { get; set; }

    [Required]
    public DateTime StartAt { get; set; }

    [Required]
    public DateTime EndAt { get; set; }

    [Required]
    [MinLength(10)]
    [MaxLength(500)]
    public required string Reason { get; set; }

    [Required]
    public bool SaveAsDraft { get; set; }

    [JsonIgnore]
    public string? Username { get; set; }
}

public class DeleteLeaveApplicationCommand : ICommand<DeleteLeaveApplicationResult>
{
    [Required]
    public required string LeaveId { get; set; }

    [JsonIgnore]
    public string? Username { get; set; }
}

public class SubmitLeaveApplicationCommand : ICommand<SubmitLeaveApplicationResult>
{
    [Required]
    public required string LeaveId { get; set; }

    [JsonIgnore]
    public string? Username { get; set; }
}

public class CancelLeaveApplicationCommand : ICommand<CancelLeaveApplicationResult>
{
    [Required]
    public required string LeaveId { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(500)]
    public required string Reason { get; set; }

    [JsonIgnore]
    public string? Username { get; set; }
}

public class EditLeaveApplicationResult
{
    public string Id { get; set; } = string.Empty;

    public string Status { get; set; } = "Draft";

    public DateTime UpdatedAt { get; set; }
}

public class DeleteLeaveApplicationResult
{
    public bool Deleted { get; set; }
}

public class SubmitLeaveApplicationResult
{
    public string Id { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";
}

public class CancelLeaveApplicationResult
{
    public string Id { get; set; } = string.Empty;

    public string Status { get; set; } = "Cancelled";
}