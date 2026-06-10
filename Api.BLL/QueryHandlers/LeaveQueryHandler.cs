using System.Collections.Generic;
using Api.Common.Queries;
using Api.Contract.DTO;
using Api.Contract.Queries;
using Api.Contract.ServiceInterfaces;

namespace Api.BLL.QueryHandlers;

public class LeaveQueryHandler :
    ISingleQueryHandler<GetMyLeaveListQuery, MyLeaveListResultDTO>,
    ISingleQueryHandler<GetMyLeaveDetailQuery, LeaveApplicationDTO>,
    ISingleQueryHandler<GetMyLeaveQuotaQuery, IList<LeaveQuotaDTO>>
{
    private readonly ILeaveService leaveService;

    public LeaveQueryHandler(ILeaveService leaveService)
    {
        this.leaveService = leaveService;
    }

    public MyLeaveListResultDTO Handle(GetMyLeaveListQuery query)
    {
        return leaveService.GetMyLeaveList(query);
    }

    public LeaveApplicationDTO Handle(GetMyLeaveDetailQuery query)
    {
        return leaveService.GetMyLeaveDetail(query);
    }

    public IList<LeaveQuotaDTO> Handle(GetMyLeaveQuotaQuery query)
    {
        return leaveService.GetMyLeaveQuota(query);
    }
}