using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Api.Common.Queries;
using Api.Contract.DTO;

namespace Api.Contract.Queries;

public class GetMyLeaveListQuery : ISingleQuery<MyLeaveListResultDTO>
{
    [RegularExpression("^(Draft|Pending|Approved|Rejected|Cancelled)$")]
    public string? Status { get; set; }

    [Range(1, int.MaxValue)]
    public int PageNo { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;

    [JsonIgnore]
    public string? Username { get; set; }
}

public class GetMyLeaveDetailQuery : ISingleQuery<LeaveApplicationDTO>
{
    [Required]
    public required string LeaveId { get; set; }

    [JsonIgnore]
    public string? Username { get; set; }
}

public class GetMyLeaveQuotaQuery : ISingleQuery<IList<LeaveQuotaDTO>>
{
    [JsonIgnore]
    public string? Username { get; set; }
}