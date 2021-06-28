using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SocialRegister.Lib
{
    public class DeclaredPersons : BaseSocialRegister
    {
        public InputParameters Parameters { get; set; }

        /// <summary>
        /// Raw data retrieved from external API.
        /// </summary>
        public List<DeclaredPersonInfoExtApi> RawData { get; set; }

        public Datasheet Datasheet { get; set; }

        /// <summary>
        /// Information about maximal declared persons count.
        /// </summary>
        public PersonsCountChangeInfo MaxDropInfo { get; set; }

        /// <summary>
        /// Information about minimal declared persons count.
        /// </summary>
        public PersonsCountChangeInfo MaxIncreaseInfo { get; set; }        

        public DeclaredPersons()
        {
            Parameters = new InputParameters();
            RawData = new List<DeclaredPersonInfoExtApi>();
            Datasheet = new Datasheet();
            MaxDropInfo = new PersonsCountChangeInfo();
            MaxIncreaseInfo = new PersonsCountChangeInfo();
        }

        public string GenerateExternalApiQuery(InputParameters parameters)
        {
            // build OData query
            var query = new StringBuilder();
            query.Append($"$filter=district_id eq {parameters.DistrictId}");
            if (parameters.Year > 0)
                query.Append($" and year eq {parameters.Year}");
            if (parameters.Month > 0)
                query.Append($" and month eq {parameters.Month}");
            if (parameters.Day > 0)
                query.Append($" and day eq {parameters.Day}");
            query.Append("&$orderby=year asc, month asc, day asc");

            return $"{ODataServiceBaseAddress}DeclaredPersons?{query}";
        }

        public async Task<string> GetDataFromApiAsync(InputParameters parameters)
        {
            var response = "";
            using (var httpClient = new HttpClient())
            {
                try
                {
                    response = await httpClient.GetStringAsync(GenerateExternalApiQuery(parameters));
                }
                catch (Exception ex)
                {
                    throw new HttpRequestException(ex.Message);
                }
            }

            return response;
        }

        /// <summary>
        /// Transform external API JSON response to the working data object.
        /// </summary>
        /// <param name="json"></param>
        public void FillDataObject(string json)
        {
            if (string.IsNullOrEmpty(json))
                return;

            RawData = JsonConvert.DeserializeObject<DeclaredPersonsRootObjectExtApi>(json).Value;
        }

        public void ProcessDataObject()
        {
            List<DatasheetDataItem> items = null;
            switch (Parameters.GroupBy.ToLower())
            {
                default:
                case "y":
                    items = (from data in RawData group data by new { data.Year } into d
                              select new DatasheetDataItem
                              {
                                  DistrictName = d.Select(x => x.DistrictName).Take(1).Single(),
                                  Year = d.Key.Year,
                                  PersonsCount = d.Sum(x => x.PersonsCount)
                              }).ToList();
                    break;

                case "m":
                    items = (from data in RawData group data by new { data.Month } into d
                              select new DatasheetDataItem
                              {
                                  DistrictName = d.Select(x => x.DistrictName).Take(1).Single(),
                                  Month = d.Key.Month,
                                  PersonsCount = d.Sum(x => x.PersonsCount)
                              }).ToList();
                    break;

                case "d":
                    items = (from data in RawData group data by new { data.Day } into d
                              select new DatasheetDataItem
                              {
                                  DistrictName = d.Select(x => x.DistrictName).Take(1).Single(),
                                  Month = d.Key.Day,
                                  PersonsCount = d.Sum(x => x.PersonsCount)
                              }).ToList();
                    break;

                case "ymd":
                    items = (from data in RawData group data by new { data.Year, data.Month, data.Day } into d
                              select new DatasheetDataItem
                              {
                                  DistrictName = d.Select(x => x.DistrictName).Take(1).Single(),
                                  Year = d.Key.Year,
                                  Month = d.Key.Month,
                                  Day = d.Key.Day,
                                  PersonsCount = d.Sum(x => x.PersonsCount)
                              }).ToList();
                    break;

                case "ym":
                    items = (from data in RawData group data by new { data.Year, data.Month } into d
                              select new DatasheetDataItem
                              {
                                  DistrictName = d.Select(x => x.DistrictName).Take(1).Single(),
                                  Year = d.Key.Year,
                                  Month = d.Key.Month,
                                  PersonsCount = d.Sum(x => x.PersonsCount)
                              }).ToList();
                    break;

                case "yd":
                    items = (from data in RawData
                              group data by new { data.Year, data.Day } into d
                              select new DatasheetDataItem
                              {
                                  DistrictName = d.Select(x => x.DistrictName).Take(1).Single(),
                                  Year = d.Key.Year,
                                  Day = d.Key.Day,
                                  PersonsCount = d.Sum(x => x.PersonsCount)
                              }).ToList();
                    break;

                case "md":
                    items = (from data in RawData
                              group data by new { data.Month, data.Day } into d
                              select new DatasheetDataItem
                              {
                                  DistrictName = d.Select(x => x.DistrictName).Take(1).Single(),
                                  Month = d.Key.Month,
                                  Day = d.Key.Day,
                                  PersonsCount = d.Sum(x => x.PersonsCount)
                              }).ToList();
                    break;
            }

            if (items.Count == 0)
                return;

            var newPersonsCount = 0;
            var personsCountTotalSum = 0;
            foreach (var item in items.Take(Parameters.Limit))
            {
                personsCountTotalSum += item.PersonsCount;

                // define min and max declared persons count for selected period
                var groupValueChange = 0;
                var prevPersonsValue = newPersonsCount;
                newPersonsCount = item.PersonsCount;
                if (prevPersonsValue > 0)
                {
                    groupValueChange = newPersonsCount - prevPersonsValue;

                    if (groupValueChange < MaxDropInfo.PersonsCount)
                    {
                        MaxDropInfo.PersonsCount = groupValueChange;
                        MaxDropInfo.DistrictName = item.DistrictName;
                        MaxDropInfo.Group = FormatGroup(Parameters.GroupBy, item);
                    }
                    if (groupValueChange > MaxIncreaseInfo.PersonsCount)
                    {
                        MaxIncreaseInfo.PersonsCount = groupValueChange;
                        MaxIncreaseInfo.DistrictName = item.DistrictName;
                        MaxIncreaseInfo.Group = FormatGroup(Parameters.GroupBy, item);
                    }
                }

                item.PersonsCountChange = groupValueChange;
                Datasheet.Data.Add(item);
            }
                        
            Datasheet.Summary.AveragePersonsCount = Convert.ToInt32(Math.Round((decimal)personsCountTotalSum / Datasheet.Data.Count()));
        }

        public void ProcessDatasheetSummary()
        {
            if (Datasheet.Data.Count == 0)
                return;

            Datasheet.Summary.MaxPersonsCount = Datasheet.Data.Max(x => x.PersonsCount);
            Datasheet.Summary.MinPersonsCount = Datasheet.Data.Min(x => x.PersonsCount);

            Datasheet.Summary.MaxPersonsCountDrop = new PersonsCountChangeInfo
            {
                PersonsCount = MaxDropInfo.PersonsCount,
                DistrictName = MaxDropInfo.DistrictName,
                Group = MaxDropInfo.Group
            };
            Datasheet.Summary.MaxPersonsCountIncrease = new PersonsCountChangeInfo
            {
                PersonsCount = MaxIncreaseInfo.PersonsCount,
                DistrictName = MaxIncreaseInfo.DistrictName,
                Group = MaxIncreaseInfo.Group
            };
        }

        public string FormatGroup(string groupBy, DatasheetDataItem data)
        {
            if (string.IsNullOrEmpty(groupBy))
                return "";

            var groupParts = groupBy.ToCharArray();

            return string.Join(".", groupParts)
                .Replace("y", data.Year.ToString())
                .Replace("m", data.Month.ToString())
                .Replace("d", data.Day.ToString());
        }
    }
}
