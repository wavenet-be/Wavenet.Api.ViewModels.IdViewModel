namespace TestWebApi
{
    using System.Web;

    using Wavenet.Api.ViewModels;
    using Wavenet.Api.ViewModels.ProtectionProviders;

    public static class IdViewModelConfig
    {
        public static void Configure()
        {
            if (HttpContext.Current.IsDebuggingEnabled)
            {
                IdViewModelInitializer.Configure<DevelopmentNoProtectionProvider>();
            }
            else
            {
                //IdViewModelInitializer.Configure(new HashidsProtectionProvider("The SALT should be stored in appsettings, database, key vault, ... and not hardcoded in source code.", minHashLength: 4, alphabet: "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"));
                IdViewModelInitializer.Configure<MachineKeyProtectionProvider>();
            }
        }
    }
}