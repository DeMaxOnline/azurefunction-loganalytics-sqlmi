namespace TobaHR.Models{
    public class WebRequestLawModel
    {
        public DateTime TimeGenerated { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string sIP { get; set; }
        public string csMethod { get; set; }
        public string csUriStem { get; set; }
        public string csUriQuery { get; set; }
        public int? sPort { get; set; }
        public string csUserName { get; set; }
        public string cIP { get; set; }
        public string csUserAgent { get; set; }
        public string csReferer { get; set; }
        public string csHost { get; set; }
        public int? scStatus { get; set; }
        public int? scSubStatus { get; set; }
        public int? scWin32Status { get; set; }
        public int? scBytes { get; set; }
        public int? csBytes { get; set; }
        public int? TimeTaken { get; set; }
        public string Computer { get; set; }
        public decimal? RemoteIPLongitude { get; set; }
        public decimal? RemoteIPLatitude { get; set; }
        public string RemoteIPCountry { get; set; }
        public string Type { get; set; }
        public string _ResourceId { get; set; }
    }
}

