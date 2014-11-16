using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Autotests.Utilities.Tags;
using Iteco.Autotests.Common.Utilities;
using Iteco.Autotests.Common.Utilities.WebElement;
using SimpleJSON;

namespace CarDatabase
{
    class Program
    {
        private static readonly WebElement DeleteRegion = new WebElement().ByClass("ico i-close-stoty i-pointer");
        private static readonly WebElement ShowAll = new WebElement().ByText("Показать все марки");   
        private static readonly WebElement Makes = new WebElement().ByClass("marks-col-a marks-col-a_sm");
        private static readonly WebElement Models = new WebElement().ByXPath("//strong[@class='showcase-modify-title']/a");
        private static readonly WebElement Generations = new WebElement().ByXPath("//strong[@class='showcase-modify-title']/a");
        private const string EngineTable = ".showcase-list";

        static void Main()
        {
            while (true)
            {
                try
                {
                    if (Grab())
                    {
                        break;
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        static bool Grab()
        {
            const string output = "Makes";

            //if (Directory.Exists(output))
            //{
            //    Directory.Delete(output, recursive: true);
            //}

            //Directory.CreateDirectory(output);

            Browser.Dispose();
            Browser.SelectedBrowser = Browsers.Chrome;
            Browser.Navigate(new Uri("http://moscow.auto.ru/"));
            DeleteRegion.ForEach(i => i.Click());
            Browser.Navigate(new Uri("http://auto.ru/"));
            ShowAll.Click();
            
            var makes = Makes.Select(i => new Make { Name = i.Text, Url = new Uri(i.GetAttribute(TagAttributes.Href).Replace("/all", null)) })
                .Where(i => !File.Exists(Path.Combine(output, i.Name + ".json"))).ToList();
            //const string startFrom = "SEAT";
            //var start = false;

            if (makes.Count == 0)
            {
                return true;
            }

            foreach (var make in makes)
            {
                //if (!start)
                //{
                //    start = make.Name == startFrom;
                //}

                //if (!start) continue;

                Browser.Navigate(make.Url);
                Thread.Sleep(2000);
                make.Models = GrabModels();
                File.WriteAllText(Path.Combine(output, make.Name + ".json"), make.ToJson().ToString());
            }

            var results = new JSONArray();

            foreach (var make in makes)
            {
                results.Add(make.ToJson());
            }

            File.WriteAllText(Path.Combine(output, "All.json"), results.ToString());
            Browser.Dispose();

            return false;
        }

        private static List<Model> GrabModels()
        {
            var result = new List<Model>();
            var models = Models.Select(i => new Model { Name = i.Text, Url = new Uri(i.GetAttribute(TagAttributes.Href).Replace("/all", null)) }).ToList();

            foreach (var model in models)
            {
                Browser.Navigate(model.Url);

                var generations = Generations.Select(i => new Generation { Name = i.Text, Url = new Uri(i.GetAttribute(TagAttributes.Href)) }).ToList();

                if (generations.Count == 0)
                {
                    model.Generations.Add(new Generation { Name = "All", Engines = GrabEngines() });
                }
                else
                {
                    foreach (var generation in generations)
                    {
                        Browser.Navigate(generation.Url);
                        generation.Engines = GrabEngines();
                        model.Generations.Add(generation);
                    }
                }

                result.Add(model);
            }

            return result;
        }

        private static List<Engine> GrabEngines()
        {
            var result = new List<Engine>();

            Browser.WaitAjax();
            Browser.WaitReadyState();

            var table = TableHelper.ToList(EngineTable);

            foreach (var row in table)
            {
                var temp = row[0].Split(Convert.ToChar("("));
                var name = temp[0].Trim();
                var power = temp[1].Split(new[] { " л.с." }, StringSplitOptions.None)[0]; // TODO: Fix кВт
                var price = "";
                var priceText = row[1];

                if (priceText.Contains(" р."))
                {
                    price = priceText.Replace(" ", null).Split(new[] { " р." }, StringSplitOptions.None)[0];
                }

                var engine = new Engine
                {
                    Name = name,
                    Power = power,
                    Price = price,
                    Type = row[2],
                    Drive = row[3],
                    Acceleration = Clean(row[4]).Replace(" с", null),
                    Speed = Clean(row[5]).Replace(" км/ч", null),
                    Consumption = Clean(row[6]).Replace(" л", null),
                    Production = row[7]
                };

                result.Add(engine);
            }

            return result;
        }

        private static string Clean(string value)
        {
            return value.Replace("–", null).Replace(",", ".").Trim();
        }

        private static void Clean(string make, ref string json)
        {
            var dict = new Dictionary<string, string>
            {
                { "Ресталинг", "(Restyling)" },
                { "Рестайлинг", "(Restyling)" },

                { "Седан-хардтоп", "Hardtop" },
                { "Седан", "Sedan" },
                { "Хардтоп", "Hardtop" },
                { "Хэтчбек", "Hatchback" },
                { "Лифтбек", "Liftback" },
                { "Фастбек", "Fastback" },
                { "Универсал", "Wagon" },
                { "Купе", "Coupe" },
                { "Кабриолет", "Cabriolet" },
                { "Родстер", "Roadster" },
                { "Спидстер", "Speedster" },
                { "Тарга", "Targa" },
                { "Лимузин", "Limousine" },
                { "Компактвэн", "Compact Van" },
                { "Фургон", "Van" },
                { "Внедорожник открытый", "SUV Open" },
                { "Внедорожник", "SUV" },
                { "Пикап Одинарная кабина", "Single Cab Pickup" },
                { "Пикап Полуторная кабина", "King Cab Pickup" },
                { "Пикап Двойная кабина", "Double Cab Pickup" },

                { "1 дв.", "3 door" },
                { "2 дв.", "3 door" },
                { "3 дв.", "3 door" },
                { "4 дв.", "3 door" },
                { "5 дв.", "5 door" },

                { "\"передний\"", "\"fwd\"" },
                { "\"задний\"", "\"rwd\"" },
                { "\"полный\"", "\"awd\"" },

                { "\"бензин\"", "\"petrol\"" },
                { "\"дизель\"", "\"diesel\"" },
                { "\"гибрид\"", "\"hybrid\"" },
                { "\"электро\"", "\"electro\"" },
                { "\"бензин / газ\"", "\"petrol / gas\"" },
                { "\"двухтактный\"", "\"duple\"" },

                { "н.в.", "" },
                { "кВт", "kW" },

                { " ", " " },
                { "–", "-" },
            };

            foreach (var key in dict.Keys)
            {
                json = Regex.Replace(json, key, dict[key], RegexOptions.IgnoreCase);
            }

            if (!new List<string> { "Hyundai", "Бронто", "ВАЗ", "ГАЗ", "ЗАЗ", "ЗИЛ", "ИЖ", "КамАЗ", "ЛУАЗ",
                "Москвич", "СеАЗ", "СМЗ", "ТагАЗ", "УАЗ", "Эксклюзив" }.Contains(make))
            {
                foreach (var c in new List<string> { "а", "б", "в", "г", "д", "е", "к", "л", "м", "н" })
                {
                    if (json.Contains(c)) throw new Exception(string.Format("{0}: {1}", c, json));
                }
            }
        }
    }
}