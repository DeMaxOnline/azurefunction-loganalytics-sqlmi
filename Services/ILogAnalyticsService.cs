using Azure.Monitor.Query.Models;

namespace TobaHR.Services
{
    public interface ILogAnalyticsService
    {
        Task<LogsQueryResult> QueryLogsAsync(string kql, DateTimeOffset startTime, DateTimeOffset endTime);
    }
}