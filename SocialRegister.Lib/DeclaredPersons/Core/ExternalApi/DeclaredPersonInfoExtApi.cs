using Newtonsoft.Json;

namespace SocialRegister.Lib
{
    /// <summary>
    /// Data model for external API entity.
    /// </summary>
    public class DeclaredPersonInfoExtApi
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }
        
        [JsonProperty("month")]
        public int Month { get; set; }

        [JsonProperty("day")]
        public int Day { get; set; }

        [JsonProperty("value")]
        public int PersonsCount { get; set; }

        [JsonProperty("district_id")]
        public int DistrictId { get; set; }

        [JsonProperty("district_name")]
        public string DistrictName { get; set; }
    }
}
