namespace Api.Common.Providers
{
    public class RootUrlProvider
    {
        public static RootUrlProviderEntity Instance { get; set; } = new RootUrlProviderEntity();
        public class RootUrlProviderEntity
        {
            public string DefaultRoot { get; set; }

            public string InternalRoot { get; set; }
        }
    }
}
