using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using Polly;

namespace PollyRetry
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Resilience policies -- Retry

            Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.BadGateway)
                .Retry(3, (exception, retryCount, context) =>
                {
                    Console.WriteLine($"retry {retryCount} times");
                })
                .Execute(ExecuteMockRequest);
            Console.WriteLine("Enter to terminate");
            Console.ReadKey();

            #endregion
        }

        static HttpResponseMessage ExecuteMockRequest()
        {
            Console.WriteLine("Web request...");
            Thread.Sleep(3000);
            return new HttpResponseMessage(HttpStatusCode.BadGateway);
        }
    }
}
