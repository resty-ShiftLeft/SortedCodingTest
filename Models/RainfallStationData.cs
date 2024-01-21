namespace SortedCodingTest.Models
{
    public class RainfallStationData
    {
        public string @id { get; set; }
        public int RLOIid { get; set; }
        public string catchmentName { get; set; }
        public DateOnly dateOpened { get; set; }
        public string eaAreaName { get; set; }
        public string eaRegionName { get; set; }
        public int easting { get; set; }
        public string label { get; set; }
        public string lat { get; set; }
        public string long_ { get; set; }

        public List<RainfallStationMeasures> measures { get; set;} = new List<RainfallStationMeasures>();

        public int northing { get; set; }
        public string notation { get; set; }
        public string riverName { get; set; }
        public string stationReference { get; set;}
        public string status { get; set;}
        public string town { get; set; }
        public string wiskiID { get; set; }
    }
}
