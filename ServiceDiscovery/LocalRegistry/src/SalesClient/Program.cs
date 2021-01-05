using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SalesClient
{
    public class Program
    {
        private static IConfigurationRoot _configuration;
        private static ApiClient _apiClient;
        public static void Main(string[] args)
        {
            LoadConfig();
            var logger = new LoggerFactory().AddConsole().CreateLogger<ApiClient>();
            _apiClient = new ApiClient(_configuration, logger);

            try
            {
                ListOrders().Wait();
                ListParts().Wait();
            }
            catch (Exception)
            {
                logger.LogError("Unable to request resource");
            }

            Console.ReadLine();
        }

        private static void LoadConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            _configuration = builder.Build();
        }

        private static async Task ListOrders()
        {
            var orders = await _apiClient.GetOrders();
            Console.WriteLine($"Order Count: {orders.Count()}");
        }

        private static async Task ListParts()
        {
            var parts = await _apiClient.GetParts();
            Console.WriteLine($"Part Count: {parts.Count()}");
        }
    }
}
