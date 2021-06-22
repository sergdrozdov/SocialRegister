using Newtonsoft.Json;
using SocialRegister.Lib;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace SocialRegister.WebAPI.Controllers
{
    public class DistrictsController : ApiController
    {
        private Districts _districts { get; set; }

        public DistrictsController()
        {
            _districts = new Districts();
        }

        /// <summary>
        /// Get list of districts.
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Get()
        {
            var districts = await _districts.GetDistrictsAsync();
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(districts, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return response;
        }
    }
}
