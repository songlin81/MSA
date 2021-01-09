using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Ocelot.Middleware;
using Ocelot.Middleware.Multiplexer;

namespace OcelotDemo.Aggregator
{
    using OcelotDemo.Dependency;

    public class LeaderAdvancedAggregator : IDefinedAggregator
    {
        public LeaderAdvancedDependency _dependency;

        #region version 13.5.1
        public LeaderAdvancedAggregator(LeaderAdvancedDependency dependency)
        {
            _dependency = dependency;
        }
        public async Task<DownstreamResponse> Aggregate(List<DownstreamContext> responses)
        {
            List<string> results = new List<string>();
            var contentBuilder = new StringBuilder();

            contentBuilder.Append("{");

            foreach (var down in responses)
            {
                string content = await down.DownstreamResponse.Content.ReadAsStringAsync();
                results.Add($"\"{down.DownstreamReRoute.Key}\":{content}");
            }
            results.Add($"\"sum\":{{comment:\"Here is the summary.\"}}");

            contentBuilder.Append(string.Join(",", results));
            contentBuilder.Append("}");

            var stringContent = new StringContent(contentBuilder.ToString())
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };

            var headers = responses.SelectMany(x => x.DownstreamResponse.Headers).ToList();
            return new DownstreamResponse(stringContent, HttpStatusCode.OK, headers, "some reason");
        }
        #endregion version 13.5.1
    }
}
