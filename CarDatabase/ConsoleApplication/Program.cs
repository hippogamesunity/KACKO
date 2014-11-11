using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autotests.Utilities.Tags;
using Iteco.Autotests.Common.Utilities;
using Iteco.Autotests.Common.Utilities.WebElement;

namespace ConsoleApplication
{
    class Program
    {
        static void Main()
        {
            const string startFrom = "ТагАЗ";

            Browser.SelectedBrowser = Browsers.Chrome;
            Browser.Navigate(new Uri("http://moscow.auto.ru/"));

            var makes = new WebElement().ByClass("marks-col-a marks-col-a_sm").Select(i => new Make
            {
                Name = i.Text,
                Url = new Uri(i.GetAttribute(TagAttributes.Href))
            }).ToList();

            var start = false;

            foreach (var make in makes)
            {
                if (!start)
                {
                    start = make.Name == startFrom;
                }

                if (!start) continue;

                Browser.Navigate(make.Url);

                var models = new WebElement().ByClass("showcase-modify-title-link").Select(i => new Model
                {
                    Name = i.Text,
                    Url = new Uri(i.GetAttribute(TagAttributes.Href).Replace("/all", null))
                }).ToList();

                foreach (var model in models)
                {
                    Browser.Navigate(model.Url);

                    var generations = new WebElement().ByXPath("//strong[@class='showcase-modify-title']/a").Select(i => new Generation
                    {
                        Name = i.Text,
                        Url = new Uri(i.GetAttribute(TagAttributes.Href))
                    }).ToList();

                    if (generations.Count == 0)
                    {
                        model.Generations.Add(new Generation
                        {
                            Name = "",
                            Engines = ReadEngines()
                        });
                    }
                    else
                    {
                        foreach (var generation in generations)
                        {
                            Browser.Navigate(generation.Url);
                            generation.Engines.AddRange(ReadEngines());
                            model.Generations.Add(generation);
                        }
                    }

                    make.Models.Add(model);
                }

                var json = make.ToJson().ToString();

                File.WriteAllText(make.Name + ".json", json);
            }
            
            Browser.Dispose();
        }

        private static List<Engine> ReadEngines()
        {
            var engines = new List<Engine>();

            Browser.WaitAjax();
            Browser.WaitReadyState();

            var table = TableHelper.ToList(".showcase-list");

            foreach (var row in table)
            {
                var temp = row[0].Split(Convert.ToChar("("));
                var name = temp[0].Trim();
                var power = temp[1].Split(new[] { " л.с.)" }, StringSplitOptions.None)[0];
                var price = "";
                var priceText = row[1];

                if (priceText.Contains(" р."))
                {
                    price = priceText.Replace(" ", null).Split(new[] { " р." }, StringSplitOptions.None)[0];

                    if (priceText.Contains("–"))
                    {
                        var parts = price.Split(Convert.ToChar("–"));

                        price = Convert.ToString((int.Parse(parts[0]) + int.Parse(parts[1])) / 2);
                    }
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

                engines.Add(engine);
            }

            return engines;
        }

        private static string Clean(string value)
        {
            return value.Replace("–", null).Replace(",", ".").Trim();
        }
    }
}