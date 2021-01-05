using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using SalesClient.Models;

namespace SalesClient
{
    public class Config
    {
        public string BaseUrl { get; set; }
        public string OrdersResource { get; set; }
        public string PartsResource { get; set; }
    }

    public class ApiClient
    {
        private const string API_CONFIG_SECTION = "sales-api";

        private readonly List<Config> _serverConfigs;
        private readonly HttpClient _apiClient;
        private readonly RetryPolicy _serverRetryPolicy;
        private int _currentConfigIndex;
        private ILogger<ApiClient> _logger;

        public ApiClient(IConfigurationRoot configuration, ILogger<ApiClient> logger)
        {
            _apiClient = new HttpClient();
            _logger = logger;

            _serverConfigs = new List<Config>();
            configuration.GetSection(API_CONFIG_SECTION).Bind(_serverConfigs);

            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var retries = _serverConfigs.Count() * 2 - 1;
            _logger.LogInformation($"Retry count set to {retries}");
            _serverRetryPolicy = Policy.Handle<HttpRequestException>()
               .RetryAsync(retries, (exception, retryCount) =>
               {
                   ChooseNextServer(retryCount);
               });
        }

        private void ChooseNextServer(int retryCount)
        {
            if (retryCount % 2 == 0)
            {
                _logger.LogWarning("Trying Next Server... \n");
                _currentConfigIndex++;

                if (_currentConfigIndex > _serverConfigs.Count - 1)
                    _currentConfigIndex = 0;
            }
        }

        public virtual Task<IEnumerable<Order>> GetOrders()
        {
            return _serverRetryPolicy.ExecuteAsync(async () =>
                {
                    var config = _serverConfigs[_currentConfigIndex];
                    var requestPath = $"{config.BaseUrl}{config.OrdersResource}";

                    _logger.LogInformation($"Making request to {requestPath}");

                    var response = await _apiClient.GetAsync(requestPath).ConfigureAwait(false);
                    var orders = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    return JsonConvert.DeserializeObject<IEnumerable<Order>>(orders);
                });
        }

        public virtual Task<IEnumerable<Part>> GetParts()
        {
            return _serverRetryPolicy.ExecuteAsync(async () =>
            {
                var config = _serverConfigs[_currentConfigIndex];
                var requestPath = $"{config.BaseUrl}{config.PartsResource}";

                _logger.LogInformation($"Making request to {requestPath}");

                var response = await _apiClient.GetAsync(requestPath).ConfigureAwait(false);
                var parts = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return JsonConvert.DeserializeObject<IEnumerable<Part>>(parts);
            });
        }
    }
}
