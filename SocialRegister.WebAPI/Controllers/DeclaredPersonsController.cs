using Newtonsoft.Json;
using SocialRegister.Lib;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace SocialRegister.WebAPI.Controllers
{
    [Route("api/declaredpersons")]
    public class DeclaredPersonsController : ApiController
    {
        private DeclaredPersons _declaredPersons = new DeclaredPersons();

        /// <summary>
        /// Get information about declared persons count.
        /// </summary>
        /// <param name="district">District ID.</param>
        /// <param name="year">Filter by year.</param>
        /// <param name="month">Filter by month.</param>
        /// <param name="day">Filter by day.</param>
        /// <param name="limit"></param>
        /// <param name="group">Group by parameter: y, m, d, ymd, ym, yd, md.</param>
        /// <returns></returns>

        [HttpGet]
        public async Task<HttpResponseMessage> Get(int district, int? year = null, int? month = null, int? day = null, int? limit = null, string group = "")
        {
            if (district <= 0)
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = new StringContent(JsonConvert.SerializeObject(new { status = 404, message = "District ID cannot be less or equals 0." }));
                errorResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                
                return errorResponse;
            }

            _declaredPersons.Parameters.DistrictId = district;
            if (year.HasValue)
                _declaredPersons.Parameters.Year = year.Value;
            if (month.HasValue)
                _declaredPersons.Parameters.Month = month.Value;
            if (day.HasValue)
                _declaredPersons.Parameters.Day = day.Value;
            if (limit.HasValue)
                _declaredPersons.Parameters.Limit = limit.Value;
            if (!string.IsNullOrEmpty(group))
                _declaredPersons.Parameters.GroupBy = group.Trim();
            var jsonResponse = await _declaredPersons.GetDataFromApiAsync(_declaredPersons.Parameters);
            _declaredPersons.FillDataObject(jsonResponse);
            _declaredPersons.ProcessDataObject();
            _declaredPersons.ProcessDatasheetSummary();

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(_declaredPersons.Datasheet, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
            return response;
        }
    }
}
