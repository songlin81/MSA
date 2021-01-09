using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using IdentityServer4.AccessTokenValidation;
using Ocelot.Provider.Polly;

namespace OcelotDemo
{
    using Ocelot.Provider.Consul;
    using OcelotDemo.Aggregator;
    using OcelotDemo.Dependency;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<LeaderAdvancedDependency>();
            services.AddOcelot()
                .AddPolly()
                .AddConsul().AddSingletonDefinedAggregator<LeaderAdvancedAggregator>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
            app.UseOcelot().Wait();
        }
    }
}
