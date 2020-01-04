using AutoMapper;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Wavenet.Api.ViewModels;
using Wavenet.Api.ViewModels.ProtectionProviders;

namespace TestAspCore2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDataProtectionProvider dataProtectionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddOpenApiDocument();

            if (services.BuildServiceProvider().GetService<IHostingEnvironment>().IsDevelopment())
            {
                services.AddIdViewModel<DevelopmentNoProtectionProvider>();
            }
            else
            {
                //services.AddIdViewModel(new HashidsProtectionProvider("SALT should be stored in environment/appsettings/...and not hardcoded in source code.", minHashLength: 4, alphabet: "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"));
                services.AddIdViewModel<MachineKeyProtectionProvider>();
            }
        }
    }
}