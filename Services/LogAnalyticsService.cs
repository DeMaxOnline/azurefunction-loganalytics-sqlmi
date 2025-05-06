using Azure.Monitor.Query;
using Azure.Monitor.Query.Models;
using Microsoft.Extensions.Logging;
using TobaHR.Models;

namespace TobaHR.Services
{
    public class LogAnalyticsService : ILogAnalyticsService
    {
        private readonly LogsQueryClient _queryClient;
        private readonly LogAnalyticsSettings _settings;
        private readonly ILogger<LogAnalyticsService> _logger;

        public LogAnalyticsService(LogsQueryClient queryClient, LogAnalyticsSettings settings, ILogger<LogAnalyticsService> logger)
        {
            _queryClient = queryClient;
            _settings = settings;
            _logger = logger;
        }

        public async Task<LogsQueryResult> QueryLogsAsync(string kql, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            var timeRange = new QueryTimeRange(startTime, endTime);
            return await _queryClient.QueryWorkspaceAsync(_settings.WorkspaceId, kql, timeRange);
        }
    }
}
