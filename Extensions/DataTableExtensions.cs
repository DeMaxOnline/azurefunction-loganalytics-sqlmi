using System.Data;
using TobaHR.Models;

namespace TobaHR.Extensions
{
    public static class DataTableExtensions
    {
        public static DataTable ToDataTable(this IEnumerable<WebRequestSqlModel> data)
        {
            var table = new DataTable();

            table.Columns.Add("Date", typeof(DateOnly));
            table.Columns.Add("Time", typeof(TimeOnly));
            table.Columns.Add("Server", typeof(string));
            table.Columns.Add("CsMethod", typeof(string));
            table.Columns.Add("CsUriStem", typeof(string));
            table.Columns.Add("CsUriQuery", typeof(string));
            table.Columns.Add("Port", typeof(int));
            table.Columns.Add("CsUsername", typeof(string));
            table.Columns.Add("ClientIP", typeof(string));
            table.Columns.Add("ClientBrowser", typeof(string));
            table.Columns.Add("ClientReference", typeof(string));
            table.Columns.Add("ClientStatus", typeof(int));
            table.Columns.Add("ClientSubStatus", typeof(int));
            table.Columns.Add("ClientHost", typeof(string));
            table.Columns.Add("W32Status", typeof(int));
            table.Columns.Add("TimeTaken", typeof(int));
            table.Columns.Add("ClientsBytesSent", typeof(int));
            table.Columns.Add("ClientsBytesReceived", typeof(int));
            table.Columns.Add("ClientIPLongitude", typeof(decimal));
            table.Columns.Add("ClientIPLatitude", typeof(decimal));
            table.Columns.Add("ClientIPCountry", typeof(string));
            table.Columns.Add("RunDate", typeof(DateTime));

            foreach (var item in data)
            {
                table.Rows.Add(
                    item.Date,
                    item.Time,
                    item.Server,
                    item.CsMethod,
                    item.CsUriStem,
                    item.CsUriQuery,
                    item.Port ?? (object)DBNull.Value,
                    item.CsUsername,
                    item.ClientIP,
                    item.ClientBrowser,
                    item.ClientReference,
                    item.ClientStatus ?? (object)DBNull.Value,
                    item.ClientSubStatus ?? (object)DBNull.Value,
                    item.ClientHost,
                    item.W32Status ?? (object)DBNull.Value,
                    item.TimeTaken ?? (object)DBNull.Value,
                    item.ClientsBytesSent ?? (object)DBNull.Value,
                    item.ClientsBytesReceived ?? (object)DBNull.Value,
                    item.ClientIPLongitude ?? (object)DBNull.Value,
                    item.ClientIPLatitude ?? (object)DBNull.Value,
                    item.ClientIPCountry,
                    item.RunDate
                );
            }

            return table;
        }
    }
}