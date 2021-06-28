using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SocialRegister.Lib
{
    public class Districts : BaseSocialRegister
    {
        public async Task<List<DistrictInfoExtApi>> GetDistrictsAsync()
        {
            var items = new List<DistrictInfoExtApi>();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetStringAsync($"{ODataServiceBaseAddress}Districts");
                    items = JsonConvert.DeserializeObject<DistrictsRootObjectExtApi>(response).Value;
                }
                catch (Exception ex)
                {
                    throw new HttpRequestException(ex.Message);
                }
            }

            return items;
        }
    }
}
