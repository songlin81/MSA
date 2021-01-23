using Castle.Facilities.AspNetCore;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CQRSlite.Cache;
using CQRSlite.Domain;
using CQRSlite.Events;
using CustomerApi.Controllers;
using CustomerApi.ReadModels.Repositories;
using CustomerApi.Services;
using CustomerApi.WriteModels.Commands.Handlers;
using CustomerApi.WriteModels.Domain.Bus;
using CustomerApi.WriteModels.Events.Handlers;
using CustomerApi.WriteModels.EventStore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace CustomerApi
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private static readonly WindsorContainer Container = new WindsorContainer();

        public Startup(IHostingEnvironment env, IConfiguration config)
        {
            _env = env;
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // Setup component model contributors for making windsor services available to IServiceProvider
            Container.AddFacility<AspNetCoreFacility>(f => f.CrossWiresInto(services));

            // Custom application component registrations, ordering is important here
            RegisterApplicationComponents(services);

            // Castle Windsor integration, controllers, tag helpers and view components, this should always come after RegisterApplicationComponents
            return services.AddWindsor(Container,
                opts => opts.UseEntryAssembly(typeof(CustomersController).Assembly), // <- Recommended
                () => services.BuildServiceProvider(validateScopes: false)); // <- Optional
        }

        private void RegisterApplicationComponents(IServiceCollection services)
        {
            // Application components
            Container.Register(
                    Component.For<IEventPublisher>().ImplementedBy<AMQPEventPublisher>().LifeStyle.Singleton,
                    Component.For<AMQPEventSubscriber>().LifeStyle.Singleton,
                    Component.For<CustomerCommandHandler>().LifeStyle.Transient,
                    Component.For<CustomerReadModelRepository>().LifeStyle.Singleton,
                    Component.For<ICustomerService>().ImplementedBy<CustomerService>().LifeStyle.Transient,
                    Component.For<CQRSlite.Domain.ISession>().ImplementedBy<Session>(),
                    Component.For<IEventStore>().ImplementedBy<CustomerEventStore>().LifeStyle.Singleton,
                    Component.For<IBusEventHandler>().ImplementedBy<CustomerCreatedEventHandler>()
                        .Named("CustomerCreatedEventHandler").LifeStyle.Singleton,
                    Component.For<IBusEventHandler>().ImplementedBy<CustomerUpdatedEventHandler>()
                        .Named("CustomerUpdatedEventHandler").LifeStyle.Singleton,
                    Component.For<IBusEventHandler>().ImplementedBy<CustomerDeletedEventHandler>()
                        .Named("CustomerDeletedEventHandler").LifeStyle.Singleton,
                    Component.For<IRepository>().UsingFactoryMethod(
                        kernel =>
                        {
                            return new CacheRepository(new Repository( //
                                kernel.Resolve<IEventStore>(), kernel.Resolve<IEventPublisher>()), kernel.Resolve<IEventStore>());
                        }));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // For making component registrations of middleware easier
            Container.GetFacility<AspNetCoreFacility>().RegistersMiddlewareInto(app);

            app.UseMvc();
        }
    }

    // Example of framework configured middleware component, can't consume types registered in Windsor
    public class FrameworkMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next(context);
        }
    }

}
