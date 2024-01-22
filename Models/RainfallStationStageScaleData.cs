namespace SortedCodingTest.Models
{
    public class RainfallStationStageScaleData
    {
        public string @id { get; set; }
        public double datum { get; set; }
        public RainfallStationHighestRecent highestRecent { get; set; }
        public RainfallStationMaxOnRecord maxOnRecord { get; set; }
        public RainfallStationMinOnRecord minOnRecord { get; set; }
        public int scaleMax { get; set; }
        public double typicalRangeHigh { get; set; }
        public double typicalRangeLow { get; set; }
    }
}
