namespace Api.Common.Helper
{
    public class AttachmentOptions
    {
        public AttachmentConfig User { get; set; }
        public AttachmentConfig Template { get; set; }
        public AttachmentConfig Channel { get; set; }
        public AttachmentConfig ChannelComment { get; set; }
        public AttachmentConfig SubmissionCoverLetter { get; set; }
        public AttachmentConfig SubmissionDisclaimer { get; set; }
        public AttachmentConfig SubmissionInputForBuilding { get; set; }
        public AttachmentConfig SubmissionDrawingList { get; set; }
        public AttachmentConfig SubmissionDrawingList_Import { get; set; }
        public AttachmentConfig SubmissionReportAndCalculation { get; set; }
        public AttachmentConfig SubmissionSupportingDocument { get; set; }
        public AttachmentConfig SubmissionOthers { get; set; }
        public AttachmentConfig SubmissionCaseAmendment { get; set; }
        public AttachmentConfig SubmissionCaseAcknowledgement { get; set; }
        public AttachmentConfig SCCU3Form { get; set; }
        public AttachmentConfig SCCU5Form { get; set; }
        public AttachmentConfig SCCU5AForm { get; set; }
        public AttachmentConfig SCCU6Form { get; set; }
        public AttachmentConfig SCCU6AForm { get; set; }
        public AttachmentConfig SCCU7Form { get; set; }
        public AttachmentConfig SCCU9Form { get; set; }
        public AttachmentConfig MasterDrawingList { get; set; }
        public AttachmentConfig SCCUFormMergePdf { get; set; }
        public AttachmentConfig StructuredComment { get; set; }
        public AttachmentConfig SystemReference { get; set; }
        public AttachmentConfig MeetingAgendaItem { get; set; }
        public AttachmentConfig MeetingMinutes { get; set; }
        public AttachmentConfig MeetingReply { get; set; }
        public AttachmentConfig MeetingAgendaItemComment { get; set; }
        public AttachmentConfig MeetingAgendaItemMin { get; set; }
        public AttachmentConfig HolidayIcs { get; set; }
        public AttachmentConfig SubmissionCaseTaskUpload { get; set; }
        public AttachmentConfig WorkingSpace { get; set; }
        public AttachmentConfig FormSCCU3ProcessingSCCU102 { get; set; }
        public AttachmentConfig FormSCCU3ProcessingCaseNotes { get; set; }
        public AttachmentConfig FormSCCU3ProcessingCaseMinutes { get; set; }
        //public AttachmentConfig SCCU3Form_Interim { get; set; }
        public AttachmentConfig ProjectRegistration { get; set; }
        public AttachmentConfig ProjectRegistrationRemark { get; set; }
        public AttachmentConfig ProjectRegistrationAORemark { get; set; }
        public AttachmentConfig ProjectRegistrationAO { get; set; }
        public AttachmentConfig UserPFX { get; set; }
        public AttachmentConfig RFA { get; set; }
        public AttachmentConfig MarkUpDrawings { get; set; }
        public AttachmentConfig MarkUpCalculation { get; set; }
        public AttachmentConfig SubmissionBIM { get; set; }
        public AttachmentConfig TaskReplyToApplicantDocument { get; set; }
        public AttachmentConfig CheckerInputDocument_Project { get; set; }
        public AttachmentConfig CheckerInputDocument_SubmissionCase { get; set; }
        public AttachmentConfig SESSECSERecommendation { get; set; }

        public AttachmentConfig CSEReplyLetter { get; set; }
        public AttachmentConfig SERecommendation { get; set; }
        public AttachmentConfig SSERecommendation { get; set; }
        public AttachmentConfig CSERecommendation { get; set; }
        public AttachmentConfig MeetingAgendaItemRemark { get; set; }
        public AttachmentConfig CommentToSCCUViaSSEAttachment { get; set; }
        public AttachmentConfig SCUReport { get; set; }
        public AttachmentConfig CalendarLegend { get; set; }
        public AttachmentConfig CheckingReportDocument { get; set; }
        public AttachmentConfig SubmissionCaseTaskRemarkUpload { get; set; }
        public AttachmentConfig TaskCommentToApplicantDocument { get; set; }
        public AttachmentConfig SCUMinutesSSE_MarkUpDrawings { get; set; }
        public AttachmentConfig SCUMinutesSSE_MarkUpCalculation { get; set; }
        public AttachmentConfig RFAReply { get; set; }
        public AttachmentConfig RFAConclusion { get; set; }
        public AttachmentConfig RFARemark { get; set; }
        public AttachmentConfig SCCUCommentToApplicantDocument { get; set; }
        public AttachmentConfig SCCUCommentToApplicantReplyDocument { get; set; }

        public AttachmentConfig ObservationReport { get; set; }
        public AttachmentConfig ProjectInformationRemarkDocument { get; set; }
        public AttachmentConfig SubmissionInformationRemarkDocument { get; set; }
        public AttachmentConfig StructuredCommentPhoto { get; set; }

        public AttachmentConfig WordTemplate { get; set; }
        public AttachmentConfig ProjectRegistrationProjectRefNo { get; set; }
        public AttachmentConfig RefCode { get; set; }
        public AttachmentConfig RefCodeCase { get; set; }
        public AttachmentConfig Meeting { get; set; }
        public AttachmentConfig Supplementary { get; set; }
        public AttachmentConfig MeetingAgendaItemDecision { get; set; }
        public AttachmentConfig MeetingDistributed { get; set; }

        public AttachmentConfig RecordOfSubmissionAttachment { get; set; }
        public AttachmentConfig RecordAttachment { get; set; }
        public AttachmentConfig InputRegister { get; set; }

        public AttachmentConfig FormSCCU10 { get; set; }
        public AttachmentConfig SCCUTargetRepleyDate { get; set; }

        public AttachmentConfig FormSCCU10Proccessing { get; set; }

        public AttachmentConfig SCCU10AckReplyDocument { get; set; }

        public AttachmentConfig MeetingMinutesReplyRemark { get; set; }

        public AttachmentConfig UsefulLinkNewDocument { get; set; }
    }
    /// <summary>
    /// Enumeration values should be the same as those configured in appsetting（OwnerType）
    /// </summary>
    public enum AttachmentOwnerType
    {
        User = 0,
        Template = 1,
        Channel = 2,
        ChannelComment = 3,
        SubmissionCoverLetter = 4,
        SubmissionDisclaimer = 5,
        SubmissionInputForBuilding = 6,
        SubmissionDrawingList = 7,
        SubmissionReportAndCalculation = 8,
        SubmissionSupportingDocument = 9,
        SubmissionOthers = 10,
        SubmissionCaseAmendment = 11,
        SCCU3Form = 12,
        StructuredComment = 13,
        SystemReference = 14,
        MeetingAgendaItem = 15,
        MeetingMinutes = 16,
        MeetingReply = 17,
        MeetingAgendaItemComment = 18,
        MeetingAgendaItemMin = 19,
        HolidayIcs = 20,
        SubmissionCaseTaskUpload = 21,
        WorkingSpace = 22,
        FormSCCU3ProcessingSCCU102 = 23,
        FormSCCU3ProcessingCaseNotes = 24,
        FormSCCU3ProcessingCaseMinutes = 25,
        SCCUFormMergePdf = 27,
        //SCCU3Form_Interim = 26,
        SubmissionDrawingList_Import = 28,
        MeetingAgendaItemRemark = 29,
        ProjectRegistration = 30,
        UserPFX = 31,
        //SCCU1FormMergePdf = 32,
        RFA = 33,
        SubmissionCaseAcknowledgement = 34,
        MarkUpDrawings = 35,
        MarkUpCalculation = 36,
        MasterDrawingList = 37,
        SubmissionBIM = 38,
        TaskReplyToApplicantDocument = 39,
        SESSECSERecommendation = 40,
        CheckerInputDocument_Project = 41,
        CheckerInputDocument_SubmissionCase = 42,
        CSEReplyLetter = 43,
        SERecommendation = 44,
        SSERecommendation = 45,
        CSERecommendation = 46,
        CommentToSCCUViaSSEAttachment = 47,
        SCUReport = 48,
        FormSCCU3ProcessingCaseAll = 49,
        CalendarLegend = 50,
        CheckingReportDocument = 51,
        SubmissionCaseTaskRemarkUpload = 52,
        TaskCommentToApplicantDocument = 53,
        SCUMinutesSSE_MarkUpDrawings = 54,
        SCUMinutesSSE_MarkUpCalculation = 55,
        ProjectRegistrationRemark = 56,
        RFAReply = 57,
        RFAConclusion = 58,
        RFARemark = 59,
        SCCUCommentToApplicantDocument = 60,
        ProjectInformationRemarkDocument = 61,
        SubmissionInformationRemarkDocument = 62,
        StructuredCommentPhoto = 63,
        ObservationReport = 64,
        WordTemplate = 65,
        ProjectRegistrationProjectRefNo = 66,
        Meeting = 67,
        Supplementary = 68,
        MeetingAgendaItemDecision = 69,
        MeetingDistributed = 70,
        ProjectRegistrationAORemark = 71,
        ProjectRegistrationAO = 72,
        RecordOfSubmissionAttachment = 73,
        RecordAttachment = 74,
        InputRegister = 75,
        SCCUCommentToApplicantReplyDocument = 76,
        FormSCCU10 = 77,
        SCCUTargetRepleyDate = 78,
        FormSCCU10Proccessing = 79,
        SCCU10AckReplyDocument = 80,
        AllMeetingUploadDocument = 81
        , SCCU5Form = 82
        , SCCU5AForm = 83
        , SCCU6Form = 84
        , SCCU6AForm = 85
        , SCCU9Form = 86,
        MeetingMinutesReplyRemark = 87,
        AllMeetingAgendaDocument = 88,
        RefCode = 89,
        RefCodeCase = 90,
        SCCU7Form = 91,
        SubmissionAllDrawingList = 92,
        MasterDrawingListAllSigned = 93,
        UsefulLinkNewDocument = 94,
        MeetingAgendaCommentDocumentAll = 95,
        AllSCUMinutesSSEAttachments = 96,
        AllSCUMinutesCSEAttachments = 97,
        AboutDASH = 98,
        FAQ = 99,
        UserManual = 100,
        TermsAndContidtions = 101,
        PrivacyPolicy = 102,
        ImportantNotice = 103,
        Help = 104,
    }
    public class AttachmentConfig
    {
        public AttachmentOwnerType OwnerType { get; set; }
        public double MaxFileSize { get; set; }
        public double MaxFileWidth { get; set; }
        public double MaxFileHeight { get; set; }
        public int MaxFileNum { get; set; }
        public string FileSavePath { get; set; }
        public string AcceptFileType { get; set; }
        public bool MultipleSelect { get; set; }
        public string LableTitle { get; set; }

        public bool IsShowProgressBar { get; set; } = false;
        public string PageSize { get; set; }
        public int MaxPageNum { get; set; }
        public int MaxLayerNum { get; set; }
        public bool NotRotation { get; set; }
    }
}
