using Api.Common.Commands;
using Api.Contract.Commands;
using Api.Contract.ServiceInterfaces;

namespace Api.BLL.CommandHandlers;

public class LeaveCommandHandler :
    ICommandHandler<CreateLeaveApplicationCommand, CreateLeaveApplicationResult>,
    ICommandHandler<EditLeaveApplicationCommand, EditLeaveApplicationResult>,
    ICommandHandler<DeleteLeaveApplicationCommand, DeleteLeaveApplicationResult>,
    ICommandHandler<SubmitLeaveApplicationCommand, SubmitLeaveApplicationResult>,
    ICommandHandler<CancelLeaveApplicationCommand, CancelLeaveApplicationResult>
{
    private readonly ILeaveService leaveService;

    public LeaveCommandHandler(ILeaveService leaveService)
    {
        this.leaveService = leaveService;
    }

    public CreateLeaveApplicationResult Handle(CreateLeaveApplicationCommand cmd)
    {
        return leaveService.CreateLeaveApplication(cmd);
    }

    public EditLeaveApplicationResult Handle(EditLeaveApplicationCommand cmd)
    {
        return leaveService.EditLeaveApplication(cmd);
    }

    public DeleteLeaveApplicationResult Handle(DeleteLeaveApplicationCommand cmd)
    {
        return leaveService.DeleteLeaveApplication(cmd);
    }

    public SubmitLeaveApplicationResult Handle(SubmitLeaveApplicationCommand cmd)
    {
        return leaveService.SubmitLeaveApplication(cmd);
    }

    public CancelLeaveApplicationResult Handle(CancelLeaveApplicationCommand cmd)
    {
        return leaveService.CancelLeaveApplication(cmd);
    }
}
