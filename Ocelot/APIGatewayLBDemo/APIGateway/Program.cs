namespace APIGateway
{
    using System.IO;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Ocelot.DependencyInjection;
    using Ocelot.Middleware;

    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHostBuilder builder = new WebHostBuilder();
            builder.ConfigureServices(s =>
            {
                s.AddSingleton(builder);
            });
            builder.UseKestrel()
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   .UseStartup<Startup>()
                   .UseUrls("http://localhost:9000");

            var host = builder.Build();
            host.Run();
        }
    }
}
