﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace SocialRegister.Lib
{
    /// <summary>
    /// External API OData response object.
    /// </summary>
    public class DistrictsRootObjectExtApi
    {
        [JsonProperty("odata.metadata")]
        public string ODataMetadata { get; set; }

        [JsonProperty("value")]
        public List<DistrictInfoExtApi> Value { get; set; }

        [JsonProperty("odata.nextLink")]
        public string ODataNextLink { get; set; }
    }
}
