using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Api.Common.Exceptions;
using Api.Contract.Commands;
using Api.Contract.DTO;
using Api.Contract.Model;
using Api.Contract.Queries;
using Api.Contract.ServiceInterfaces;
using Api.DAL;

namespace Api.BLL.Services;

public class LeaveService : ILeaveService
{
    private readonly UnitOfWorkLeaveSystem unitOfWork;

    public LeaveService(UnitOfWorkLeaveSystem unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public CreateLeaveApplicationResult CreateLeaveApplication(CreateLeaveApplicationCommand cmd)
    {
        EnsureSaveAsDraft(cmd.SaveAsDraft);
        ValidateLeavePayload(cmd.LeaveType, cmd.StartAt, cmd.EndAt, cmd.Reason);

        var applicant = GetEmployeeOrThrow(cmd.Username);
        var durationDays = CalculateDurationDays(cmd.StartAt, cmd.EndAt);
        var now = DateTime.Now;

        var entity = new LeaveRequest
        {
            ApplicantId = applicant.Id,
            LeaveType = cmd.LeaveType,
            StartTime = cmd.StartAt,
            EndTime = cmd.EndAt,
            Duration = durationDays,
            Reason = cmd.Reason,
            Status = "Draft",
            CreatedAt = now,
            UpdatedAt = now,
            Applicant = applicant
        };

        unitOfWork.GetRepository<LeaveRequest>().Add(entity);
        unitOfWork.Save();

        return new CreateLeaveApplicationResult
        {
            LeaveRequestId = entity.Id,
            Status = entity.Status
        };
    }

    public EditLeaveApplicationResult EditLeaveApplication(EditLeaveApplicationCommand cmd)
    {
        EnsureSaveAsDraft(cmd.SaveAsDraft);
        ValidateLeavePayload(cmd.LeaveType, cmd.StartAt, cmd.EndAt, cmd.Reason);

        var applicant = GetEmployeeOrThrow(cmd.Username);
        var leaveId = ParseLeaveId(cmd.LeaveId);
        var leaveRequest = GetOwnedLeaveRequestOrThrow(applicant.Id, leaveId);

        if (leaveRequest.Status != "Draft" && leaveRequest.Status != "Rejected")
        {
            throw new LeaveApiException(
                "INVALID_STATUS_TRANSITION",
                "only draft or rejected leave can be edited",
                400,
                "status",
                $"current status is {leaveRequest.Status}");
        }

        leaveRequest.LeaveType = cmd.LeaveType;
        leaveRequest.StartTime = cmd.StartAt;
        leaveRequest.EndTime = cmd.EndAt;
        leaveRequest.Duration = CalculateDurationDays(cmd.StartAt, cmd.EndAt);
        leaveRequest.Reason = cmd.Reason;
        leaveRequest.Status = "Draft";
        leaveRequest.UpdatedAt = DateTime.Now;

        unitOfWork.Save();

        return new EditLeaveApplicationResult
        {
            Id = FormatLeaveId(leaveRequest.Id),
            Status = leaveRequest.Status,
            UpdatedAt = leaveRequest.UpdatedAt
        };
    }

    public DeleteLeaveApplicationResult DeleteLeaveApplication(DeleteLeaveApplicationCommand cmd)
    {
        var applicant = GetEmployeeOrThrow(cmd.Username);
        var leaveId = ParseLeaveId(cmd.LeaveId);
        var leaveRequest = GetOwnedLeaveRequestOrThrow(applicant.Id, leaveId);

        if (leaveRequest.Status != "Draft")
        {
            throw new LeaveApiException(
                "INVALID_STATUS_TRANSITION",
                "only draft leave can be deleted",
                400,
                "status",
                $"current status is {leaveRequest.Status}");
        }

        unitOfWork.GetRepository<LeaveRequest>().Delete(leaveRequest);
        unitOfWork.Save();

        return new DeleteLeaveApplicationResult { Deleted = true };
    }

    public SubmitLeaveApplicationResult SubmitLeaveApplication(SubmitLeaveApplicationCommand cmd)
    {
        var applicant = GetEmployeeOrThrow(cmd.Username);
        var leaveId = ParseLeaveId(cmd.LeaveId);
        var leaveRequest = GetOwnedLeaveRequestOrThrow(applicant.Id, leaveId);

        if (leaveRequest.Status != "Draft")
        {
            throw new LeaveApiException(
                "INVALID_STATUS_TRANSITION",
                "only draft leave can be submitted",
                400,
                "status",
                $"current status is {leaveRequest.Status}");
        }

        if (string.Equals(leaveRequest.LeaveType, "Annual", StringComparison.OrdinalIgnoreCase))
        {
            EnsureAnnualQuotaAvailable(applicant.Id, leaveRequest.Duration);
        }

        leaveRequest.Status = "Pending";
        leaveRequest.UpdatedAt = DateTime.Now;

        unitOfWork.Save();

        return new SubmitLeaveApplicationResult
        {
            Id = FormatLeaveId(leaveRequest.Id),
            Status = leaveRequest.Status
        };
    }

    public CancelLeaveApplicationResult CancelLeaveApplication(CancelLeaveApplicationCommand cmd)
    {
        var applicant = GetEmployeeOrThrow(cmd.Username);
        var leaveId = ParseLeaveId(cmd.LeaveId);
        var leaveRequest = GetOwnedLeaveRequestOrThrow(applicant.Id, leaveId);

        if (leaveRequest.Status != "Pending")
        {
            throw new LeaveApiException(
                "INVALID_STATUS_TRANSITION",
                "only pending leave can be cancelled",
                400,
                "status",
                $"current status is {leaveRequest.Status}");
        }

        leaveRequest.Status = "Cancelled";
        leaveRequest.ApprovalComment = cmd.Reason;
        leaveRequest.UpdatedAt = DateTime.Now;

        unitOfWork.Save();

        return new CancelLeaveApplicationResult
        {
            Id = FormatLeaveId(leaveRequest.Id),
            Status = leaveRequest.Status
        };
    }

    public MyLeaveListResultDTO GetMyLeaveList(GetMyLeaveListQuery query)
    {
        var applicant = GetEmployeeOrThrow(query.Username);
        var leaveRepository = unitOfWork.GetRepository<LeaveRequest>();

        var filteredQuery = leaveRepository
            .GetQueryable(x => x.ApplicantId == applicant.Id, null, "Applicant,Approver", true)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            filteredQuery = filteredQuery.Where(x => x.Status == query.Status);
        }

        var total = filteredQuery.Count();
        var items = filteredQuery
            .OrderByDescending(x => x.UpdatedAt)
            .ThenByDescending(x => x.Id)
            .Skip((query.PageNo - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList()
            .Select(MapToListItem)
            .ToList();

        return new MyLeaveListResultDTO
        {
            Items = items,
            PageNo = query.PageNo,
            PageSize = query.PageSize,
            Total = total
        };
    }

    public LeaveApplicationDTO GetMyLeaveDetail(GetMyLeaveDetailQuery query)
    {
        var applicant = GetEmployeeOrThrow(query.Username);
        var leaveId = ParseLeaveId(query.LeaveId);
        var leaveRequest = GetOwnedLeaveRequestOrThrow(applicant.Id, leaveId, true);

        return MapToApplicationDto(leaveRequest);
    }

    public IList<LeaveQuotaDTO> GetMyLeaveQuota(GetMyLeaveQuotaQuery query)
    {
        var applicant = GetEmployeeOrThrow(query.Username);
        var quotaRepository = unitOfWork.GetRepository<LeaveQuota>();

        var quotas = quotaRepository
            .GetQueryable(x => x.UserId == applicant.Id, null, "User", true)
            .OrderBy(x => x.LeaveType)
            .ToList()
            .Select(MapToQuotaDto)
            .ToList();

        return quotas;
    }

    private static void EnsureSaveAsDraft(bool saveAsDraft)
    {
        if (!saveAsDraft)
        {
            throw new ValidationException("saveAsDraft must be true");
        }
    }

    private static void ValidateLeavePayload(string leaveType, DateTime startAt, DateTime endAt, string reason)
    {
        if (startAt < DateTime.Now)
        {
            throw new ValidationException("startAt must be greater than or equal to current time");
        }

        if (endAt <= startAt)
        {
            throw new ValidationException("endAt must be later than startAt");
        }

        if (string.IsNullOrWhiteSpace(reason) || reason.Length < 10 || reason.Length > 500)
        {
            throw new ValidationException("reason must be between 10 and 500 characters");
        }

        if (!IsValidLeaveType(leaveType))
        {
            throw new ValidationException("leaveType is invalid");
        }

        var durationDays = CalculateDurationDays(startAt, endAt);
        if (durationDays < 0.5m)
        {
            throw new ValidationException("durationDays must be at least 0.5");
        }
    }

    private static bool IsValidLeaveType(string leaveType)
    {
        return string.Equals(leaveType, "Annual", StringComparison.OrdinalIgnoreCase)
            || string.Equals(leaveType, "Sick", StringComparison.OrdinalIgnoreCase)
            || string.Equals(leaveType, "Personal", StringComparison.OrdinalIgnoreCase)
            || string.Equals(leaveType, "Compensatory", StringComparison.OrdinalIgnoreCase);
    }

    private User GetEmployeeOrThrow(string? username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ValidationException("Missing user context");
        }

        var applicant = unitOfWork
            .GetRepository<User>()
            .GetQueryable(x => x.Username == username && x.Role == "Employee")
            .FirstOrDefault();

        if (applicant == null)
        {
            throw new LeaveApiException("NOT_FOUND", "employee user not found", 404, "user", "employee user not found");
        }

        return applicant;
    }

    private LeaveRequest GetOwnedLeaveRequestOrThrow(int applicantId, int leaveId, bool includeApprover = false, bool noTracking = false)
    {
        var includeProperties = includeApprover ? "Applicant,Approver" : "Applicant";
        var leaveRequest = unitOfWork
            .GetRepository<LeaveRequest>()
            .GetQueryable(x => x.Id == leaveId && x.ApplicantId == applicantId, null, includeProperties, noTracking)
            .FirstOrDefault();

        if (leaveRequest == null)
        {
            throw new LeaveApiException("NOT_FOUND", "leave request not found", 404, "leaveId", "leave request not found or not owned by current user");
        }

        return leaveRequest;
    }

    private void EnsureAnnualQuotaAvailable(int applicantId, decimal durationDays)
    {
        var quota = unitOfWork
            .GetRepository<LeaveQuota>()
            .GetQueryable(x => x.UserId == applicantId && x.LeaveType == "Annual", null, "User", true)
            .FirstOrDefault();

        var remainingDays = quota?.RemainingDays ?? 0m;
        if (remainingDays < durationDays)
        {
            throw new LeaveApiException(
                "LEAVE_QUOTA_EXCEEDED",
                "年假额度不足",
                400,
                "leaveType",
                $"申请 {durationDays:0.0} 天, 剩余 {remainingDays:0.0} 天");
        }
    }

    private static decimal CalculateDurationDays(DateTime startAt, DateTime endAt)
    {
        return decimal.Round((decimal)(endAt - startAt).TotalDays, 1, MidpointRounding.AwayFromZero);
    }

    private static int ParseLeaveId(string leaveId)
    {
        if (string.IsNullOrWhiteSpace(leaveId))
        {
            throw new ValidationException("leaveId is required");
        }

        var normalized = leaveId.StartsWith("L", StringComparison.OrdinalIgnoreCase)
            ? leaveId[1..]
            : leaveId;

        if (!int.TryParse(normalized, out var id))
        {
            throw new ValidationException("leaveId format is invalid");
        }

        return id;
    }

    private static string FormatLeaveId(int id)
    {
        return $"L{id:D12}";
    }

    private static string BuildEmployeeId(int userId)
    {
        return $"U{1000 + userId:D4}";
    }

    private static LeaveApplicationDTO MapToApplicationDto(LeaveRequest leaveRequest)
    {
        return new LeaveApplicationDTO
        {
            Id = FormatLeaveId(leaveRequest.Id),
            EmployeeId = BuildEmployeeId(leaveRequest.ApplicantId),
            EmployeeName = leaveRequest.Applicant?.Username ?? string.Empty,
            LeaveType = leaveRequest.LeaveType,
            StartAt = leaveRequest.StartTime,
            EndAt = leaveRequest.EndTime,
            DurationDays = leaveRequest.Duration,
            Reason = leaveRequest.Reason,
            Status = leaveRequest.Status,
            ManagerComment = leaveRequest.ApprovalComment,
            CreatedAt = leaveRequest.CreatedAt,
            UpdatedAt = leaveRequest.UpdatedAt
        };
    }

    private static LeaveListItemDTO MapToListItem(LeaveRequest leaveRequest)
    {
        return new LeaveListItemDTO
        {
            Id = FormatLeaveId(leaveRequest.Id),
            LeaveType = leaveRequest.LeaveType,
            StartAt = leaveRequest.StartTime,
            EndAt = leaveRequest.EndTime,
            DurationDays = leaveRequest.Duration,
            Status = leaveRequest.Status,
            UpdatedAt = leaveRequest.UpdatedAt
        };
    }

    private static LeaveQuotaDTO MapToQuotaDto(LeaveQuota leaveQuota)
    {
        return new LeaveQuotaDTO
        {
            EmployeeId = BuildEmployeeId(leaveQuota.UserId),
            EmployeeName = leaveQuota.User?.Username ?? string.Empty,
            LeaveType = leaveQuota.LeaveType,
            TotalDays = leaveQuota.TotalDays,
            UsedDays = decimal.Max(0m, leaveQuota.TotalDays - leaveQuota.RemainingDays),
            RemainingDays = leaveQuota.RemainingDays,
            UpdatedAt = leaveQuota.UpdatedAt == default ? DateTime.Now : leaveQuota.UpdatedAt
        };
    }
}
