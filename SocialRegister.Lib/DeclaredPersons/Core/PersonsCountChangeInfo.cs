using Newtonsoft.Json;

namespace SocialRegister.Lib
{
    public class PersonsCountChangeInfo
    {
        [JsonProperty("value")]
        public int PersonsCount { get; set; }

        [JsonProperty("districtName")]
        public string DistrictName { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }
    }
}
