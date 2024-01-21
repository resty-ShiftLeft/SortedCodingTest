namespace SortedCodingTest.Models
{
    public class RainfallData
    {
        public string @id { get; set; }
        public string description { get; set; }
        public string eaAreaName { get; set; }
        public string eaRegionName { get; set; }

        public RainfallFloodAreaData floodArea { get; set; } = new RainfallFloodAreaData();

        public string floodAreaID { get; set; }
        public bool isTidal { get; set; }
        public string message { get; set; }
        public string severity { get; set; }
        public int severityLevel { get; set; }
        public DateTime timeMessageChanged { get; set; }
        public DateTime timeRaised { get; set; }
        public DateTime timeSeverityChanged { get; set; }
    }
}
