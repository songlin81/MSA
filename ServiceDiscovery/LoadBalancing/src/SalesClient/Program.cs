using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SalesClient
{
    public class Program
    {
        private static IConfigurationRoot _configuration;
        private static ServiceClient _apiClient;
        public static void Main(string[] args)
        {
            LoadConfig();

            var logger = new LoggerFactory().AddConsole().CreateLogger<ServiceClient>();
            _apiClient = new ServiceClient(_configuration, logger);

            using (_apiClient)
            {
                try
                {
                    ListOrders();
                    ListParts();
                }
                catch (Exception)
                {
                    logger.LogError("Unable to request resource");
                }
                Console.ReadLine();
            }
        }

        private static void LoadConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            _configuration = builder.Build();
        }

        private static void ListOrders()
        {
            var orders = _apiClient.GetOrders();
            Console.WriteLine($"Order Count: {orders.Count()}");
        }

        private static void ListParts()
        {
            var parts = _apiClient.GetParts();
            Console.WriteLine($"Part Count: {parts.Count()}");
        }
    }
}
