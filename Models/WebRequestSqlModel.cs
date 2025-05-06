namespace TobaHR.Models{
    public class WebRequestSqlModel
    {
        public DateOnly? Date { get; set; }
        public TimeOnly? Time { get; set; }          
        public string? Server { get; set; }
        public string? CsMethod { get; set; }
        public string? CsUriStem { get; set; }
        public string? CsUriQuery { get; set; }
        public int? Port { get; set; }
        public string? CsUsername { get; set; }
        public string? ClientIP { get; set; }
        public string? ClientBrowser { get; set; }
        public string? ClientReference { get; set; }
        public int? ClientStatus { get; set; }
        public int? ClientSubStatus { get; set; }
        public string? ClientHost { get; set; }
        public int? W32Status { get; set; }
        public int? TimeTaken { get; set; }
        public int? ClientsBytesSent { get; set; }
        public int? ClientsBytesReceived { get; set; }
        public decimal? ClientIPLongitude { get; set; }
        public decimal? ClientIPLatitude { get; set; }
        public string? ClientIPCountry { get; set; }
        public DateTime? RunDate { get; set; }
    }
}

