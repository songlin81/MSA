﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using SalesClient.Models;

namespace SalesClient
{
    public class ApiClient
    {
        private readonly List<Uri> _serverUrls;
        private readonly IConfigurationRoot _configuration;
        private readonly HttpClient _apiClient;
        private RetryPolicy _serverRetryPolicy;
        private readonly ConsulClient _consulClient;
        private int _currentConfigIndex;
        private readonly ILogger<ApiClient> _logger;

        public ApiClient(IConfigurationRoot configuration, ILogger<ApiClient> logger)
        {
            _logger = logger;
            _configuration = configuration;

            _apiClient = new HttpClient();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _serverUrls = new List<Uri>();

            _consulClient = new ConsulClient(c =>
            {
                var uri = new Uri(_configuration["consulConfig:address"]);
                c.Address = uri;
            });


        }

        public async Task Initialize()
        {
            _logger.LogInformation("Discovering Services from Consul.");

            var services = await _consulClient.Agent.Services();
            foreach (var service in services.Response)
            {
                var isSalesApi = service.Value.Service == _configuration["consulConfig:serviceName"];
                if (isSalesApi)
                {
                    var checks = await _consulClient.Health.Checks(_configuration["consulConfig:serviceName"]);
                    _logger.LogInformation($"{service.Value.Address}:{service.Value.Port}--status: {checks}");
                    var serviceUri = new Uri($"{service.Value.Address}:{service.Value.Port}");
                    _serverUrls.Add(serviceUri);
                }
            }

            _logger.LogInformation($"{_serverUrls.Count} endpoints found.");
            var retries = _serverUrls.Count * 2 - 1;
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
                Console.WriteLine("Trying next server... \n");
                _currentConfigIndex++;

                if (_currentConfigIndex > _serverUrls.Count - 1)
                    _currentConfigIndex = 0;
            }
        }

        public virtual async Task<IEnumerable<Order>> GetOrders()
        {
            // You really don't have to call CheckServerHealth every time. Just here for the demo
            await CheckServerHealth();
            return await _serverRetryPolicy.ExecuteAsync(async () =>
                {
                    var serverUrl = _serverUrls[_currentConfigIndex];
                    var requestPath = $"{serverUrl}api/orders";

                    _logger.LogInformation($"Making request to {requestPath}");
                    var response = await _apiClient.GetAsync(requestPath).ConfigureAwait(false);
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    return JsonConvert.DeserializeObject<IEnumerable<Order>>(content);
                });
        }

        public virtual async Task<IEnumerable<Part>> GetParts()
        {
            await CheckServerHealth();
            return await _serverRetryPolicy.ExecuteAsync(async () =>
            {
                var serverUrl = _serverUrls[_currentConfigIndex];
                var requestPath = $"{serverUrl}api/parts";

                _logger.LogInformation($"Making request to {requestPath}");
                var response = await _apiClient.GetAsync(requestPath).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return JsonConvert.DeserializeObject<IEnumerable<Part>>(content);
            });
        }

        private async Task CheckServerHealth()
        {
            var checks = await _consulClient.Health.Service(_configuration["consulConfig:serviceName"]);
            foreach (var entry in checks.Response)
            {
                var check = entry.Checks.SingleOrDefault(c => c.ServiceName == "sales-api");
                if (check == null) continue;
                var isPassing = check.Status == HealthStatus.Passing;
                var serviceUri = new Uri($"{entry.Service.Address}:{entry.Service.Port}");
                if (isPassing)
                {
                    if (!_serverUrls.Contains(serviceUri))
                    {
                        _serverUrls.Add(serviceUri);
                    }
                }
                else
                {
                    if (_serverUrls.Contains(serviceUri))
                    {
                        _serverUrls.Remove(serviceUri);
                    }
                }
            }
        }
    }
}
