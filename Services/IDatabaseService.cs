using TobaHR.Models;

namespace TobaHR.Services
{
    public interface IDatabaseService
    {
        Task<bool> InsertLogsAsync(IList<WebRequestSqlModel> logs);
        Task<DateTime> GetLastTimeRecordWebRequest();
    }
}