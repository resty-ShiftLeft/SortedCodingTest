namespace SortedCodingTest.Models
{
    public class RainfallStationMeasures
    {
        public string @id { get; set; }
        public string datumType { get; set; }
        public string label { get; set; }

        public RainfallStationMeasuresLastReading latestReading { get; set;} = new RainfallStationMeasuresLastReading();

        public string notation { get; set; }
        public string parameter { get; set; }
        public string parameterName { get; set; }
        public int period { get; set; }
        public string qualifier { get; set; }
        public string station { get; set; }
        public string stationReference { get; set; }
        public string unit { get; set; }
        public string unitName { get; set; }
        public string valueType { get; set; }

    }
}
