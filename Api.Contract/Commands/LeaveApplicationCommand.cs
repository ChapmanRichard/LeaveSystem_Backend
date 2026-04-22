using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Api.Common.Commands;

namespace Api.Contract.Commands;

public class CreateLeaveApplicationCommand : ICommand<CreateLeaveApplicationResult>
{
    [Required]
    [RegularExpression("^(Annual|Sick|Personal|Compensatory)$")]
    public required string LeaveType { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    [Required]
    [MinLength(10)]
    [MaxLength(500)]
    public required string Reason { get; set; }

    public bool SaveAsDraft { get; set; }

    [JsonIgnore]
    public string? Username { get; set; }
}

public class CreateLeaveApplicationResult
{
    public int LeaveRequestId { get; set; }

    public string Status { get; set; } = "Draft";
}
