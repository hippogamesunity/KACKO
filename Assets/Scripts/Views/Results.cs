using System;
using System.Collections.Generic;
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

        public void Initialize(JSONNode companies, JSONNode calc)
        {
            var results = new List<Result>();

            for (var k = 0; k < calc["results"].Count; k++)
            {
                var result = calc["results"][k];
                var price = result["result"]["total"]["premium"];
                var regions = result["values"]["region"].Childs.Select(i => i.Value).ToList();

                if (Profile.Region != Regions.AnyRegion && !regions.Contains(Profile.Region)) continue;

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
                var companyInfo = GetCompanyInfo(result.CompanyName);

                companyButton.Name.SetText(result.CompanyName);
                companyButton.Price.SetText(result.Price.ToPriceInt());
                companyButton.Icon.spriteName = companyInfo.Icon;
                companyButton.Button.Up += () =>
                {
                    var company = GetComponent<Company>();

                    company.Initialize(result, companyInfo);
                    company.Open(TweenDirection.Right);
                };
                instance.transform.localPosition = new Vector2(i % 3 * Step.x - Position.x, Position.y - Mathf.Floor(j / Size.x) * Step.y);
            }
        }

        public static CompanyInfo GetCompanyInfo(string company)
        {
            switch (company)
            {
                default: return KnownCompanies[0];
            }
        }

        private static readonly List<CompanyInfo> KnownCompanies = new List<CompanyInfo>
        {
            new CompanyInfo("Default", null)
        };
    }
}