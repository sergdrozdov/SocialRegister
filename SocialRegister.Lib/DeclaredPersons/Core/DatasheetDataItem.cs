using Newtonsoft.Json;

namespace SocialRegister.Lib
{
    /// <summary>
    /// Data model for local data processing.
    /// </summary>
    public class DatasheetDataItem
    {
        [JsonProperty("districtName")]
        public string DistrictName { get; set; }

        [JsonProperty("year")]
        public int? Year { get; set; }

        [JsonProperty("month")]
        public int? Month { get; set; }

        [JsonProperty("day")]
        public int? Day { get; set; }

        [JsonProperty("value")]
        public int PersonsCount { get; set; }

        [JsonProperty("change")]
        public int PersonsCountChange { get; set; }
    }
}
