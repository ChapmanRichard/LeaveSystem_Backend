namespace Api.Common.Hubs
{
    public enum SubmissionCaseFlowEnum
    {
        Draft,
        Start,
        Cancel
    }
    public enum SubmissionCaseAmendmentFlowEnum
    {
        Accept,
        Reject,
        Return,
        Close
    }
    public enum SubmissionCaseTaskFlowEnum
    {
        Draft,
        Start,
        Cancel
    }
    public enum TaskReplyToApplicantFlowEnum
    {
        NORIGHT,
        A_STEP_RIGHT,
        SA_STEP_RIGHT,
        CA_STEP_RIGHT,
        STO_STEP_RIGHT,
        PTO_STEP_RIGHT,
    }
}
