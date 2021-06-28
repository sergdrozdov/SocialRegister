using Newtonsoft.Json;

namespace SocialRegister.Lib
{
    public class DistrictInfoExtApi
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("district_name")]
        public string Name { get; set; }
    }
}
