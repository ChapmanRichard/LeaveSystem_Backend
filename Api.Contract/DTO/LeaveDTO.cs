using System;
using System.Collections.Generic;

namespace Api.Contract.DTO;

public class LeaveApplicationDTO
{
    public string Id { get; set; } = string.Empty;

    public string EmployeeId { get; set; } = string.Empty;

    public string EmployeeName { get; set; } = string.Empty;

    public string LeaveType { get; set; } = string.Empty;

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public decimal DurationDays { get; set; }

    public string Reason { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string? ManagerComment { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

public class LeaveListItemDTO
{
    public string Id { get; set; } = string.Empty;

    public string LeaveType { get; set; } = string.Empty;

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public decimal DurationDays { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime UpdatedAt { get; set; }
}

public class LeaveQuotaDTO
{
    public string EmployeeId { get; set; } = string.Empty;

    public string EmployeeName { get; set; } = string.Empty;

    public string LeaveType { get; set; } = string.Empty;

    public decimal TotalDays { get; set; }

    public decimal UsedDays { get; set; }

    public decimal RemainingDays { get; set; }

    public DateTime UpdatedAt { get; set; }
}

public class MyLeaveListResultDTO
{
    public IList<LeaveListItemDTO> Items { get; set; } = new List<LeaveListItemDTO>();

    public int PageNo { get; set; }

    public int PageSize { get; set; }

    public int Total { get; set; }
}