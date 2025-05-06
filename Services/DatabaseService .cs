using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TobaHR.Extensions;
using TobaHR.Models;

namespace TobaHR.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString;
        private readonly ILogger<DatabaseService> _logger;

        public DatabaseService(IConfiguration config, ILogger<DatabaseService> logger)
        {
            // Fetch secure connection string from config (e.g., in local.settings.json or Azure app settings)
            _connectionString = config.GetConnectionString("SqlDb") 
                                ?? config["SqlConnectionString"] 
                                ?? throw new InvalidOperationException("Database connection not configured");
            _logger = logger;
        }

        public async Task<DateTime> GetLastTimeRecordWebRequest(){
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand(@"
                SELECT TOP 1 Date, Time 
                FROM dbo.WebRequests 
                ORDER BY Date DESC", connection);

            cmd.CommandTimeout = 60; // 1 minute should be more than enough now

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var date = reader.GetDateTime(0); // First column: Date
                var time = reader.GetTimeSpan(1); // Second column: Time

                // Combine Date + Time into a single DateTime
                return date.Date + time;
            }

            // If no records found
            return new DateTime(3999, 1, 1);
        }

        public async Task<bool> InsertLogsAsync(IList<WebRequestSqlModel> logs)
        {
            if (logs == null || logs.Count == 0) return false;

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var dataTable = logs.ToDataTable();

                using var bulkCopy = new SqlBulkCopy(connection)
                {
                    DestinationTableName = "WebRequests",
                    BulkCopyTimeout = 60
                };

                // Map column names explicitly (recommended)
                bulkCopy.ColumnMappings.Add("Date", "Date");
                bulkCopy.ColumnMappings.Add("Time", "Time");
                bulkCopy.ColumnMappings.Add("Server", "Server");
                bulkCopy.ColumnMappings.Add("CsMethod", "CsMethod");
                bulkCopy.ColumnMappings.Add("CsUriStem", "CsUriStem");
                bulkCopy.ColumnMappings.Add("CsUriQuery", "CsUriQuery");
                bulkCopy.ColumnMappings.Add("Port", "Port");
                bulkCopy.ColumnMappings.Add("CsUsername", "CsUsername");
                bulkCopy.ColumnMappings.Add("ClientIP", "ClientIP");
                bulkCopy.ColumnMappings.Add("ClientBrowser", "ClientBrowser");
                bulkCopy.ColumnMappings.Add("ClientReference", "ClientReference");
                bulkCopy.ColumnMappings.Add("ClientStatus", "ClientStatus");
                bulkCopy.ColumnMappings.Add("ClientSubStatus", "ClientSubStatus");
                bulkCopy.ColumnMappings.Add("ClientHost", "ClientHost");
                bulkCopy.ColumnMappings.Add("W32Status", "W32Status");
                bulkCopy.ColumnMappings.Add("TimeTaken", "TimeTaken");
                bulkCopy.ColumnMappings.Add("ClientsBytesSent", "ClientsBytesSent");
                bulkCopy.ColumnMappings.Add("ClientsBytesReceived", "ClientsBytesReceived");
                bulkCopy.ColumnMappings.Add("ClientIPLongitude", "ClientIPLongitude");
                bulkCopy.ColumnMappings.Add("ClientIPLatitude", "ClientIPLatiude");
                bulkCopy.ColumnMappings.Add("ClientIPCountry", "ClientIPCountry");
                bulkCopy.ColumnMappings.Add("RunDate", "RunDate");

                await bulkCopy.WriteToServerAsync(dataTable);
                _logger.LogInformation("Bulk inserted {Count} log records into SQL MI.", logs.Count);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
