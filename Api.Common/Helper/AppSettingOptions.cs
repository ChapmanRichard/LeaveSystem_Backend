namespace Api.Common.Helper
{
    public class AppSettingOptions
    {
        public string ChatUrl { get; set; }
        public bool IsCheckSignName { get; set; }
        public int UserStorageCapacity { get; set; }
        //public List<BIM> BIM { get; set; }
        public string DigitalSignWord { get; set; }
        public string APDigitalSignWord { get; set; }
        public string RSEDigitalSignWord { get; set; }
        //public bool IsUAT { get; set; }

        public bool IsUseNormalEmail { get; set; }
        public bool IsDevelopment { get; set; }
        public string DigitalSignToolUrl { get; set; }
        //public bool IsUATEmail { get; set; }
        public bool IsASDMailServer { get; set; }
        public bool IsUATEmailPrefix { get; set; }
        public bool IsProducationInitData { get; set; }
        public bool IsRecordEmailInfoToFile { get; set; }
        public int SCCUInitProjectNumber { get; set; }
        public int SCUInitProjectNumber { get; set; }
        public int DCInitProjectNumber { get; set; }
        public int ENQInitProjectNumber { get; set; }
    }

    //public class BIM
    //{
    //    public string Name { get; set; }
    //    public List<int> Version { get; set; }
    //}
}
