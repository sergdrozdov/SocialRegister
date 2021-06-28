using System.Collections.Generic;
using Newtonsoft.Json;

namespace SocialRegister.Lib
{
    public class Datasheet
    {
        [JsonProperty("data")]
        public List<DatasheetDataItem> Data { get; set; }

        [JsonProperty("summary")]
        public DatasheetSummary Summary { get; set; }

        public Datasheet()
        {
            Data = new List<DatasheetDataItem>();
            Summary = new DatasheetSummary();
        }
    }
}
