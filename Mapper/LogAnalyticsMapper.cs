using Azure.Monitor.Query.Models;
using TobaHR.Extensions;
using TobaHR.Models;

namespace TobaHR.Mapper
{
    public static class LogAnalyticsMapper {
        public static List<WebRequestLawModel> LogAnalyticsToModelMapper(LogsQueryResult logsQuery){
            var logs = new List<WebRequestLawModel>();

            if (logsQuery == null || logsQuery.Table == null)
                return logs;

            var table = logsQuery.Table;
            var columns = table.Columns;
            var rows = table.Rows;

            foreach (var row in rows)
            {
                var model = new WebRequestLawModel();

                for (int i = 0; i < columns.Count; i++)
                {
                    var columnName = columns[i].Name;
                    var value = row[i];

                    MapColumn(model, columnName, value);
                }

                logs.Add(model);
            }

            return logs;
        }

        public static WebRequestSqlModel MapToSqlModel(WebRequestLawModel log)
        {
            return new WebRequestSqlModel
            {
                Date = DateOnly.FromDateTime(log.TimeGenerated),
                Time = TimeOnly.FromDateTime(log.TimeGenerated),
                Server = log.Computer.Truncate(32),
                CsMethod = log.csMethod.Truncate(8),
                CsUriStem = log.csUriStem.Truncate(256),
                CsUriQuery = log.csUriQuery,
                Port = log.sPort,
                CsUsername = log.csUserName.Truncate(128),
                ClientIP = log.cIP.Truncate(32),
                ClientBrowser = log.csUserAgent,
                ClientReference = log.csReferer,
                ClientHost = log.csHost.Truncate(128),
                ClientStatus = log.scStatus,
                ClientSubStatus = log.scSubStatus,
                W32Status = log.scWin32Status,
                TimeTaken = log.TimeTaken,
                ClientsBytesSent = log.scBytes,
                ClientsBytesReceived = log.csBytes,
                ClientIPLongitude = log.RemoteIPLongitude,
                ClientIPLatitude = log.RemoteIPLatitude,
                ClientIPCountry = log.RemoteIPCountry.Truncate(64),
                RunDate = DateTime.UtcNow
            };
        }

        public static List<WebRequestSqlModel> MapToSqlModels(IEnumerable<WebRequestLawModel> laws)
        {
            return [.. laws.Select(MapToSqlModel)];
        }

        private static void MapColumn(WebRequestLawModel model, string columnName, object value)
        {
            switch (columnName)
            {
                case "TimeGenerated":
                    model.TimeGenerated = DateTime.TryParse(value?.ToString(), out var tg) ? tg : default;
                    break;
                case "Date":
                    model.Date = value?.ToString();
                    break;
                case "Time":
                    model.Time = value?.ToString();
                    break;
                case "sIP":
                    model.sIP = value?.ToString();
                    break;
                case "csMethod":
                    model.csMethod = value?.ToString();
                    break;
                case "csUriStem":
                    model.csUriStem = value?.ToString();
                    break;
                case "csUriQuery":
                    model.csUriQuery = value?.ToString();
                    break;
                case "sPort":
                    model.sPort = TryParseInt(value);
                    break;
                case "csUserName":
                    model.csUserName = value?.ToString();
                    break;
                case "cIP":
                    model.cIP = value?.ToString();
                    break;
                case "csUserAgent":
                    model.csUserAgent = value?.ToString();
                    break;
                case "csReferer":
                    model.csReferer = value?.ToString();
                    break;
                case "csHost":
                    model.csHost = value?.ToString();
                    break;
                case "scStatus":
                    model.scStatus = TryParseInt(value);
                    break;
                case "scSubStatus":
                    model.scSubStatus = TryParseInt(value);
                    break;
                case "scWin32Status":
                    model.scWin32Status = TryParseInt(value);
                    break;
                case "scBytes":
                    model.scBytes = TryParseInt(value);
                    break;
                case "csBytes":
                    model.csBytes = TryParseInt(value);
                    break;
                case "TimeTaken":
                    model.TimeTaken = TryParseInt(value);
                    break;
                case "Computer":
                    model.Computer = value?.ToString();
                    break;
                case "RemoteIPLongitude":
                    model.RemoteIPLongitude = TryParseDecimal(value);
                    break;
                case "RemoteIPLatitude":
                    model.RemoteIPLatitude = TryParseDecimal(value);
                    break;
                case "RemoteIPCountry":
                    model.RemoteIPCountry = value?.ToString();
                    break;
                case "Type":
                    model.Type = value?.ToString();
                    break;
                case "_ResourceId":
                    model._ResourceId = value?.ToString();
                    break;
            }
        }

        private static int? TryParseInt(object value)
        {
            return int.TryParse(value?.ToString(), out var result) ? result : null;
        }

        private static decimal? TryParseDecimal(object value)
        {
            return decimal.TryParse(value?.ToString(), out var result) ? result : null;
        }
    }
}