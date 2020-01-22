namespace Wavenet.Api.ViewModels.IdViewModelTests
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Wavenet.Api.ViewModels.ProtectionProviders;

    internal class TestStartup
    {
        public TestStartup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void Configure()
        {
            // nothing todo.
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddIdViewModel<DevelopmentNoProtectionProvider>();
        }
    }
}