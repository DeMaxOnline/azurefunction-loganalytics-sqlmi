using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TobaHR.Mapper;
using TobaHR.Services;

namespace TimeTriggerTransfer
{
    public class TransferWebRequests
    {
        private readonly ILogger<TransferWebRequests> _logger;
        private readonly ILogAnalyticsService _analyticsService;
        private readonly IDatabaseService _dbService;
        public TransferWebRequests(
            ILoggerFactory loggerFactory,
            ILogAnalyticsService analyticsService, 
            IDatabaseService dbService)
        {
            _logger = loggerFactory.CreateLogger<TransferWebRequests>();
            _analyticsService = analyticsService;
            _dbService = dbService;
        }

        [Function("TransferWebRequests")]
        public async Task RunAsync([TimerTrigger("0 0 2 * * *")] TimerInfo myTimer)
        {
            try 
            {
                DateTime currentStart = await _dbService.GetLastTimeRecordWebRequest();
                DateTime now = DateTime.Today;

                while (currentStart < now)
                {
                    DateTime currentEnd = currentStart.AddMinutes(15);
                    if (currentEnd > now)
                        currentEnd = now;

                    _logger.LogInformation("Starting log data migration for period {Start} to {End}", currentStart, currentEnd);

                    // 1. Query logs
                    var logs = await _analyticsService.QueryLogsAsync("W3CIISLog", currentStart, currentEnd);
                    int logsCount = logs.Table.Rows.Count;
                    _logger.LogInformation("Retrieved {Count} log records.", logsCount);

                    if (logsCount > 0)
                    {
                        var webRequestLaw = LogAnalyticsMapper.LogAnalyticsToModelMapper(logs);
                        var webRequestSqlModel = LogAnalyticsMapper.MapToSqlModels(webRequestLaw);

                        await _dbService.InsertLogsAsync(webRequestSqlModel);
                    }

                    currentStart = currentEnd;
                }
            } 
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Log ingestion function failed.");
            }
        }
    }
}
