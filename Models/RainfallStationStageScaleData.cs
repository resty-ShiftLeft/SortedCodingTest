namespace SortedCodingTest.Models
{
    public class RainfallStationStageScaleData
    {
        public string @id { get; set; }
        public double datum { get; set; }
        public List<RainfallStationHighestRecent> highestRecent { get; set; }
        public List<RainfallStationMaxOnRecord> maxOnRecord { get; set; }
        public List<RainfallStationMinOnRecord> minOnRecord { get; set; }
        public int scaleMax { get; set; }
        public double typicalRangeHigh { get; set; }
        public double typicalRangeLow { get; set; }
    }
}
