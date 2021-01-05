using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace SalesClient
{
    public class Program
    {
        const string API_CONFIG_SECTION = "sales-api";
        const string API_CONFIG_NAME_BASEURL = "baseUrl";
        const string API_CONFIG_NAME_ORDERS_RESOURCE = "orders";
        const string API_CONFIG_NAME_PARTS_RESOURCE = "parts";

        private static IConfigurationRoot configuration;
        private static HttpClient apiClient;
        public static void Main(string[] args)
        {
            LoadConfig();
            SetupHttpClient();
            ListOrders();
            ListParts();

            Console.ReadLine();
            Console.ResetColor();
        }

        private static void LoadConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            configuration = builder.Build();
        }

        private static void SetupHttpClient()
        {
            apiClient = new HttpClient
            {
                BaseAddress = new Uri(configuration.GetSection(API_CONFIG_SECTION)[API_CONFIG_NAME_BASEURL])
            };

            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static void ListOrders()
        {
            Console.WriteLine($"Making request to {apiClient.BaseAddress}{API_CONFIG_NAME_ORDERS_RESOURCE}");
            var response = apiClient.GetAsync(API_CONFIG_NAME_ORDERS_RESOURCE).Result;
            var orders = response.Content.ReadAsStringAsync().Result;
             Console.WriteLine($"Order Count: {orders.Count()}\n");
        }

        private static void ListParts()
        {
             Console.WriteLine($"Making request to {apiClient.BaseAddress}{API_CONFIG_NAME_PARTS_RESOURCE}");
            var response = apiClient.GetAsync(API_CONFIG_NAME_PARTS_RESOURCE).Result;
            var parts = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine($"Part Count: {parts.Count()}\n");
        }
    }
}
