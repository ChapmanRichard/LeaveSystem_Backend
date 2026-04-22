using System;
using System.ComponentModel;

namespace Api.Common
{
    public enum BadgeTheme
    {
        [Description("primary")]
        Primary,
        [Description("secondary")]
        Secondary,
        [Description("success")]
        Success,
        [Description("danger")]
        Danger,
        [Description("info")]
        Info,
        [Description("warning")]
        Warning
    }
    public enum TableTheme
    {
        [Description("bg-gray bg-darken-3")]
        Gray,
        [Description("bg-red bg-darken-3")]
        Red,
        [Description("bg-blackBlue bg-darken-3")]
        blackBlue,
        [Description("bg-deep-blue")]
        deepBlue
    }
    public enum EnumMenu
    {
        Home,
        Calendar,
        Submission,
        Project,
        ProjectRegistration,
        WorkingSpace,
        Channel,
        Template,
        User,
        MetaData,
        Report,
        OtherUI,
        Inbox,
        ManageProject,
        ManageProjectTeam,
        MyAccount,
        ShareInformation,
        Meeting,
        Searching,
        RFA,
        SystemSetting,
        QuickLinks,
        QuickLinks1,
        Dashboard,
        SCCUDashboard,
        News,
        Notes,
        ExportCSVTool,
        SubmissionRegister,
        DisplayRecord,
        InternalSCUDashboard
    }

    [Flags]
    public enum EnumCertificatePrep
    {
        Plans = 1,
        StructuralDetails = 2,
        Calculations = 4
    }
    [Flags]
    public enum EnumReasonsforReturn
    {
        [Description("Insufficient Document(s)")]
        Insufficient = 1,
        [Description("Type Identifier(s) for Structural Drawing(s) NOT comply with ArchSD Manual")]
        TypeIdentifier = 2,
        [Description("Incorrect Drawing Title(s) / Number(s)")]
        DrawingIncorrect = 4,
        [Description("Incorrect Drawing Revision(s)")]
        Revision = 8,
        //[Description("Drawing under checking in previous submission")]
        //Previous = 16,
        [Description("Others")]
        Others = 16
    }
    [Flags]
    public enum EnumSeRecommendationChecked
    {
        [Description("Large amount of error and deficency")]
        Large = 1,
        [Description("The comments have not been fully addressed within the set timeframe")]
        NotFully = 2,
        [Description("Further Design Review")]
        FurtherDesign = 4,
        [Description("Discrepancy in design bearing capacity from GEO's memo for site formation amendment")]
        Discrepancy = 8,
        [Description("The submission not tally with the GBP")]
        NotTally = 16,
        [Description("Building was found on a slope, Geotechnical Assessment Submission was not approved by GEO")]
        Building = 32,
        [Description("The record as-built drawing not for SCU approval")]
        SCUApproval = 64
    }
    public enum EnumSignPerson
    {
        AP,
        RSE,
        OnlyOne,
        CU
    }

    public enum EnumWarningReminderMsg
    {
        [Description("!! Revision period for submission case# {0} has reached {1} working days. Prolonged delay in returning your submission may lead to the termination of checking process.")]
        R1,
        [Description("!! Processing time of the submission case# {0} has reached {1} working days.")]
        R2,
        [Description("!! Maximum {0} working days for each extension of time (EoT) application. You cannot apply for an EoT if the actual revision period for a submission case has reached {1} working days.")]
        R3,
        [Description("!! The submission case# {0} revision time has been overdue and allowed to be terminated by SCU.")]
        R4,
        [Description("!! The submission case# {0} revision time has been overdue and could be terminated by SCU.")]
        R5,

        [Description("!! You have not returned the submission case# {0} by the expected return date({1}). You must apply for an extension of time (EoT) to extend revision period by TODAY or else the checking process will be terminated. ")]
        W1,
        [Description("!! You have not returned the submission case# {0} by the deadline date({1}). As the total revision period has reached 10 working days, no further extension of time (EoT) can be applied. You must upload the amended documents by TODAY or else the checking process will be terminated.")]
        W2,
        [Description("!! Revision period for submission case# {0} has reached {1} working days.")]
        W3,
        [Description("!! You must apply for an extension of processing time (EoP) before uploading the amended documents to continue processing your submission case# {0}.  Otherwise, the checking process will be terminated.")]
        W4,
        [Description("!! The amended documents you returned did not fully address SCU comments. You must apply for an extension of processing time (EoP) to continue processing your submission case# {0} by {1} (Day 20), or else the checking process will be terminated.")]
        W5,
        [Description("!! Revision period for submission case# {0} has reached {1} working days.  Select further processing or the submission will be terminated.")]
        W6,

    }

    public enum EnumRecordOfSubmission
    {
        Submission,
        SubmissionCaseDocument,
        SubmissionCaseAmendment,
        Revision,
    }
    public enum EnumRecord
    {
        Submission,
        SubmissionCaseDocument,
        SubmissionCaseAmendment,
        Revision,
    }





    public enum EnumCertWay
    {
        MyAccount_Add,
        Registration_Add,
        Sign_Already,
        InviteSign_Already,
        Login,
        MockSign
    }
    public enum EnumMeetingPage
    {
        Meeting,
        Agenda,
        Comment,
        Reply,
        CommentDetail,
        DecisionSecretary
    }

    public enum EumSubmissionOperation
    {
        Edit,
        Upload,
        Submit,
        Import,
    }
    public enum EnumEmailOperation
    {
        SCUAssignCheckingTeamEvent,
        SubmissionCaseAcknowledgementEvent_Returned,
        SubmissionCaseAcknowledgementEvent_Returned_CcMessage,
        SubmissionCaseAcknowledgementEvent_Acknowledged,
        SubmissionCaseAcknowledgementEvent_Acknowledged_CcMessage,
        SendNotificationForApplicantChange,
        SendInvitationEmail,
        SendInvitationToInternalUser,
        SendProjectRegistrationAcknowledgeToInternalUser,
        SCUCommentToApplicantEvent,
        SCUCommentToApplicantEventCcMessage,
        ReviseTaskAfterAcceptanceOfRequest,
        ReviseTaskAfterAcceptanceOfRequest_CcMessage,
        SendEOT,
        SendEOP,
        SubmitAmendmentDocumentEvent,
        ReturnProjectRegistrationEvent,
        SendInvitationCodeEvent,
        ResendInvitationCodeEvent,
        InvitationCompletedEvent,
        SendReplyEmailAfterSubmit,
        SendInfoAfterSubmit,
        SendInfoAfterSubmitForSCUAdmin,
        SendInfoAfterRevise,
        SendInfoAfterAcceptOrReject,
        SCUMinutesCSEEvent_Submit,
        SCUMinutesCSEEvent_IssueReply_Submit,
        SCUMinutesCSEEvent_Return,
        SCUWithdrawRequestNotifySE,
        SCUMinutesCSEEvent_IssueReply_Return,
        SCUMinutesCSEEvent_NoAgree,
        SCUMinutesCSEEvent_Support,
        SCUMinutesCSEEvent_Endorse,
        SCUMinutesCSEEvent_Endorse_CcMessage,
        SCUMinutesCSEEvent_Sign,
        SCUMinutesCSEEvent_Sign_CcMessage,
        SCUMinutesSEEvent_AskSupport,
        SCUMinutesSSEEvent_Return,
        SCUReplySSEEvent_Return,
        SCUMinutesSEEvent_SEWithdrawn,
        SCUReplySEEvent_SEWithdrawn,
        SCUMinutesSSEEvent_SSEWithdrawn,
        AssignSubmissionCaseSignSubmit,
        W1ApplicantApplyEOTWarning,
        W2ApplicantNotAllowApplyEOTWarning,
        ApplicantCreateSubmissionCaseAmendmentRequest,
        W3ApplicantTotalRevisionTenDay,
        W5ApplicantWarning,
        NotifySESSEDayLeftThreeWarning,
        AmendmentStatusChangeNotify,
        RegistrationOfAccountWillExpirySoon,
        RegistrationIsNotCompleteOnTime,
        SCUMinutesSEEvent_Submit,
        SCUIssueReplySEEvent_Submit,
        SubmissionCaseWithdrawn_ResultAcceptNotification,
        SubmissionCaseCreate,
        SubmissionCaseResultNotifyApplicantAndProcessingSide,
        SubmissionCaseResultNotifyApplicantAndProcessingSide_CcMessage,
        SubmissionCaseSubmitToApplicant,
        SubmissionCaseSubmitToCCMessage,
        SubmissionStatusToCCMessage,
        NotifyCheckingTeamToAcknowledge,
        NotifyCheckingTeamToAllocateTeam,
        NotifyRefSCUCheckingTeamToAcknowledge,
        NotifyApplicantSubmissionResult,
        NotifyApplicantSubmissionResult_CcMessage,
        ChatRoomSubmitForInvitedUser,
        RFASubmitForUserReply,
        RFASubmitForUserReplyCcMessage,
        CcMessageRFASubmitNotice,
        RFAReminderForUserReply,
        SCCUCommentToApplicant,
        SCCUCommentToApplicantCcMessage,
        ChangeSubmissionTargetReplyDateNotify,
        SCUReAssignCheckingTeamEvent,
        SubmissionApproveNotifySubmissionSide,
        InvitationAcknowledge,
        InvitationReturnToInternalApplicant,
        InvitationReturnToPRO,
        RequestAsssignedFinish,
        RequestAssignedUnassigned,
        SubmissionReceiptForRevisedSubmission,
        SubmissionReceiptForRevisedSubmissionCcMessage,
        ReminderCheckingTeamRevised,
        ReviseTaskWithCommentsFromProcessingSide,
        ReviseTaskWithCommentsFromProcessingSideCcMessage,
        ReminderCheckingTeamTaskOverdue,
        InvitedAsCollaboratorToCollaborator,
        InvitedDeletedCollaboratorToApplicantAndDelegatedCollaborator,
        CollaboratorPermissionChangedToCollaborator,
        CollaboratorPermissionChangedToApplicantAndDelegatedCollaborator,
        SendOtherInvitationCodeEvent,
        RFARequestForActionConcluded,
        RFARequestForActionConcludedCcMessage,
        RequestToReviseRejectNotification,
        ReminderRegistrationNoExpiry,
        MeetingIssueRevisedAgenda,
        SendOtherInvitationCodeEventAgain,
        ResponseToSubmissionRequest,
        ResponseToSubmissionRequestCcMessage,
        SubmissionReceiptRequestSubmittedRevise,
        SubmissionReceiptRequestSubmittedReviseCcMessage,
        SubmissionReceiptRequestSubmittedWithdraw,
        SubmissionReceiptRequestSubmittedWithdrawCcMessage,
        NotifySESSEDayLeftFive_SCCU56Warning,
        NotifySESSEDayLeftThree_SCCU4Warning,
        SubmissionResultReplyLetterMessage,
        SubmissionResultReplyLetterMessageCcMessage,
        ReminderNDayTargetReplyDates,
        SubmissionApplicantChangeReturnedMessage,
        ReviseTargetReplyDateMessage,
        AfterBookingConfirm,
        AfterBookingConfirmCcMessage,
        WhenRFAIsReplied,
        WhenRFAIsRepliedCcMessage,
        MeetingInviteForIndoor,
        MeetingInviteForOther,
        MeetingMinutesReply,
        MeetingCommentToSecretary,
        MeetingDecisionUpdatedNotify,
        ActingArrangementNotify,
        WhenAccountProfileIsChanged,
        SCCUWhenActingArrangementAssignedEditedUserBeingActed,
        SCCUWhenActingArrangementAssignedEditedUserToAct,
        SCUReplyIsProvided,
        SCUReplyToApplicantDirectly,
        SCUCommentPendingAfterTarget,
        SCCU56ReminderForSCCU4NotSubmit,
        RegistrationIsNotCompleteOnTimeCu,
        ProjectTargetCompletionDateNotify,
        SCCU56FollowUpReminder,
        ReminderForCheckingTeamTargetReplyDate,
        NotifySCUCheckingTeamAcknowledged,
        SCURequestForFurtherInformation,
        SCURequestForFurtherInformationCcMessage,
        SCUSubmissionReceiptForFurtherInformationSubmit,
        SCUSubmissionReceiptForFurtherInformationSubmitCcMessage,
        CalendarEventReminder,
    }

    public enum EnumSpecificationPage
    {
        TodoItem,
        Inbox,
        Submission,
        ManagePermSubmission,
        Workspace,
        Meeting,
        SubmissionRight,
        ProjectDetail,
        SubmissionTypeSetting,
        SubmissionListAsRoleOthers,
    }
    public enum EnumRFAPage
    {
        LandingPage,
        PeojectOrSubmission,
        SCURefLanding,
        Meeting,
    }
}
