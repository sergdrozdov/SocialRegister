using Newtonsoft.Json;

namespace SocialRegister.Lib
{
    public class DatasheetSummary
    {
        [JsonProperty("max")]
        public int MaxPersonsCount { get; set; }

        [JsonProperty("min")]
        public int MinPersonsCount { get; set; }

        [JsonProperty("average")]
        public int AveragePersonsCount { get; set; }

        [JsonProperty("maxDrop")]
        public PersonsCountChangeInfo MaxPersonsCountDrop { get; set; }

        [JsonProperty("maxIncrease")]
        public PersonsCountChangeInfo MaxPersonsCountIncrease { get; set; }
    }
}
