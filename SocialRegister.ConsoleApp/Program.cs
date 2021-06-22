using Newtonsoft.Json;
using SocialRegister.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SocialRegister
{
    class Program
    {
        private const string MsgAnyKeyToExit = "Press any key to exit...";

        static void Main(string[] args)
        {
            Console.WriteLine("**********************************");
            Console.WriteLine("Practice test by Sergey Drozdov");
            Console.WriteLine("**********************************");
            Console.WriteLine("Email:\t\tsergey.drozdov.1980@gmail.com");
            Console.WriteLine("Website:\thttp://sd.blackball.lv/sergey-drozdov");
            Console.WriteLine();

            var paramDistrictId = 0; // required parameter
            var paramYear = 0;
            var paramMonth = 0;
            var paramDay = 0;
            var paramLimit = 0;
            var paramGroup = "";
            var paramOutputFile = "";

            if (args.Length == 0)
            {
                DisplayErrorMessage("Arguments list cannot be empty. At least -district [id] is required. For example, Rīga: -district 516");
                return;
            }
            else
            {
                // There is no arguments validation.
                // For this test, it is assumed that the parameters will be correct.
                try
                {
                    for (var i = 0; i < args.Length; i++)
                    {
                        switch (args[i].ToLower())
                        {
                            case "-district":
                                paramDistrictId = Convert.ToInt32(args[i + 1]);
                                break;
                            case "-year":
                                paramYear = Convert.ToInt32(args[i + 1]);
                                break;
                            case "-month":
                                paramMonth = Convert.ToInt32(args[i + 1]);
                                break;
                            case "-day":
                                paramDay = Convert.ToInt32(args[i + 1]);
                                break;
                            case "-limit":
                                paramLimit = Convert.ToInt32(args[i + 1]);
                                break;
                            case "-group":
                                paramGroup = args[i + 1];
                                break;
                            case "-out":
                                paramOutputFile = args[i + 1];
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    DisplayErrorMessage(ex.Message);
                    return;
                }

                // validate required parameters
                if (paramDistrictId <= 0)
                {
                    DisplayErrorMessage("District ID must be greater than 0.");
                    return;
                }

                var declaredPersons = new DeclaredPersons();
                var inputParameters = new InputParameters();
                declaredPersons.Parameters.DistrictId = paramDistrictId;
                declaredPersons.Parameters.Year = paramYear;
                declaredPersons.Parameters.Month = paramMonth;
                declaredPersons.Parameters.Day = paramDay;
                declaredPersons.Parameters.Limit = paramLimit;
                if (!string.IsNullOrEmpty(paramGroup))
                    declaredPersons.Parameters.GroupBy = paramGroup;

                var jsonResponse = Task.Run(() => declaredPersons.GetDataFromApiAsync(declaredPersons.Parameters)).Result;
                if (string.IsNullOrEmpty(jsonResponse))
                {
                    DisplayErrorMessage("No data found.");
                    return;
                }
                
                declaredPersons.FillDataObject(jsonResponse);
                if (declaredPersons.RawData.Count == 0)
                {
                    DisplayErrorMessage("No data found.");
                    return;
                }

                // create data template for output
                var dataTemplate = new List<DataTemplateItem>();
                dataTemplate.Add(new DataTemplateItem { Name = "district_name", Width = 20 });
                if (!string.IsNullOrEmpty(declaredPersons.Parameters.GroupBy))
                {
                    foreach (var groupPart in declaredPersons.Parameters.GroupBy)
                    {
                        switch (groupPart)
                        {
                            case 'y':
                                dataTemplate.Add(new DataTemplateItem { Prefix = "y", Name = "year", Width = 8 });
                                break;
                            case 'm':
                                dataTemplate.Add(new DataTemplateItem { Prefix = "m", Name = "month", Width = 8 });
                                break;
                            case 'd':
                                dataTemplate.Add(new DataTemplateItem { Prefix = "d", Name = "day", Width = 8 });
                                break;
                        }
                    }
                }
                dataTemplate.Add(new DataTemplateItem { Name = "value", Width = 12 });
                dataTemplate.Add(new DataTemplateItem { Name = "change", Width = 10 });

                // print headers
                foreach (var item in dataTemplate)
                {
                    Console.Write($"{{0,-{item.Width}}}", item.Name);
                }
                Console.WriteLine();

                // transform data loaded from external API
                declaredPersons.ProcessDataObject();

                // prepare data row for console output
                foreach (var item in declaredPersons.Datasheet.Data)
                {
                    // fill row data depending on dynamic grouping
                    var counter = 0;
                    var row = new string[dataTemplate.Count];
                    row[counter] = item.DistrictName;
                    foreach (var data in dataTemplate)
                    {
                        if (!string.IsNullOrEmpty(data.Prefix))
                        {
                            switch (data.Prefix.ToLower())
                            {
                                case "y":
                                    counter++;
                                    row[counter] = item.Year.ToString();
                                    break;
                                case "m":
                                    counter++;
                                    row[counter] = item.Month.ToString();
                                    break;
                                case "d":
                                    counter++;
                                    row[counter] = item.Day.ToString();
                                    break;
                            }
                        }
                    }
                    counter++;
                    row[counter] = item.PersonsCount.ToString();
                    counter++;
                    row[counter] = item.PersonsCountChange.ToString();

                    for (var i = 0; i < row.Length; i++)
                    {
                        Console.Write($"{{0,-{dataTemplate[i].Width}}}", row[i]);
                    }
                    Console.WriteLine();
                }

                declaredPersons.ProcessDatasheetSummary();                

                // print summary
                Console.WriteLine();
                Console.WriteLine($"Max:\t\t{declaredPersons.Datasheet.Summary.MaxPersonsCount}");
                Console.WriteLine($"Min:\t\t{declaredPersons.Datasheet.Summary.MinPersonsCount}");
                Console.WriteLine($"Average:\t{declaredPersons.Datasheet.Summary.AveragePersonsCount}");
                Console.WriteLine();
                Console.WriteLine(string.Format("{0, -17} {1, 10} {2}", "Max drop:", declaredPersons.Datasheet.Summary.MaxPersonsCountDrop.PersonsCount, declaredPersons.Datasheet.Summary.MaxPersonsCountDrop.Group));
                Console.WriteLine(string.Format("{0, -17} {1, 10} {2}", "Max increase:", declaredPersons.Datasheet.Summary.MaxPersonsCountIncrease.PersonsCount, declaredPersons.Datasheet.Summary.MaxPersonsCountIncrease.Group));

                // write report to output file
                if (!string.IsNullOrEmpty(paramOutputFile))
                {
                    using (var file = File.CreateText($"{Directory.GetCurrentDirectory()}\\{paramOutputFile}"))
                    {
                        file.Write(JsonConvert.SerializeObject(declaredPersons.Datasheet, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
                    }
                }
            }

            Console.WriteLine();
            Console.Write(MsgAnyKeyToExit);
            Console.ReadKey();
        }

        private static void DisplayErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine();
            Console.Write(MsgAnyKeyToExit);
            Console.ReadKey();
        }
    }
}