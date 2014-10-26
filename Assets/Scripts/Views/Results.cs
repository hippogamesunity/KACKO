using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.Common;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Results : ViewBasePaging
    {
        protected override Vector2 Size { get { return new Vector2(3, 4); } }
        protected override Vector2 Step { get { return new Vector2(170, 200); } }
        protected override Vector2 Position { get { return new Vector2(170, 380); } }

        protected override void Initialize()
        {
            var companies = JSON.Parse(File.ReadAllText(@"d:\companies.json"));
            var kasko = JSON.Parse(File.ReadAllText(@"d:\kasko.json"));

            var results = new List<Result>();

            for (int k = 0; k < kasko["results"].Count; k++)
            {
                var result = kasko["results"][k];
                var price = result["result"]["total"]["premium"];
                var regions = result["values"]["region"].Childs.Select(i => i.Value).ToList();
                var companyCode = result["info"]["code"];

                var company = companies.Childs.FirstOrDefault(i => i["calculators"].Childs.Any(j => j["code"].Value == companyCode.Value));

                if (company == null) continue;

                var companyName = company["name"];
                var companyShortName = company["nameshort"];
                var companyRating = company["rating"];

                results.Add(new Result
                {
                    CompanyName = companyName.Value,
                    CompanyShortName = companyShortName.Value,
                    Rating = int.Parse(companyRating.Value),
                    Price = int.Parse(price.Value.Split(Convert.ToChar("."))[0]),
                    Regions = regions
                });
            }

            results = results.OrderBy(i => i.Price).ToList();

            CreatePages(Mathf.CeilToInt(results.Count / (Size.x * Size.y)));

            for (var i = 0; i < results.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (Size.x * Size.y));
                var j = i % (Size.x * Size.y);
                var instance = PrefabsHelper.InstantiateCompanyButton(Pages[page].transform);
                var companyButton = instance.GetComponent<CompanyButton>();

                var result = results.ElementAt(i);

                companyButton.Name.SetText(result.CompanyName);
                companyButton.Price.SetText(result.Price);
                companyButton.Icon.spriteName = GetCompanyLogo(result.CompanyName);
                companyButton.Button.Up += () => Debug.Log("TEST");
                instance.transform.localPosition = new Vector2(i % 3 * Step.x - Position.x, Position.y - Mathf.Floor(j / Size.x) * Step.y);
            }
        }

        private string GetCompanyLogo(string company)
        {
            switch (company)
            {
                case "Итиль": return "Итиль";
                case "Эстер": return "Эстер";
                case "Региональный Страховой Центр": return "Региональный Страховой Центр";
                case "Авеста": return "Авеста";
                case "Помощь": return "Помощь";
                case "Капитал-Полис": return "Капитал-Полис";
                default: return "Default";
            }
        }
    }
}