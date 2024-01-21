namespace SortedCodingTest.Models
{
    public class RainfallStationMeasuresLastReading
    {
        public string @id { get; set; }
        public DateOnly date { get; set; }
        public DateTime dateTime { get; set; }
        public string measure { get; set; }
        public string value { get; set; }
    }
}
