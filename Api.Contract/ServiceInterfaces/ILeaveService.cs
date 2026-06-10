using System.Collections.Generic;
using Api.Contract.Commands;
using Api.Contract.DTO;
using Api.Contract.Queries;

namespace Api.Contract.ServiceInterfaces;

public interface ILeaveService
{
    CreateLeaveApplicationResult CreateLeaveApplication(CreateLeaveApplicationCommand cmd);

    EditLeaveApplicationResult EditLeaveApplication(EditLeaveApplicationCommand cmd);

    DeleteLeaveApplicationResult DeleteLeaveApplication(DeleteLeaveApplicationCommand cmd);

    SubmitLeaveApplicationResult SubmitLeaveApplication(SubmitLeaveApplicationCommand cmd);

    CancelLeaveApplicationResult CancelLeaveApplication(CancelLeaveApplicationCommand cmd);

    MyLeaveListResultDTO GetMyLeaveList(GetMyLeaveListQuery query);

    LeaveApplicationDTO GetMyLeaveDetail(GetMyLeaveDetailQuery query);

    IList<LeaveQuotaDTO> GetMyLeaveQuota(GetMyLeaveQuotaQuery query);
}
