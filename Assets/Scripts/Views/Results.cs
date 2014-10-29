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

        public void Initialize(JSONNode companies, JSONNode calc, bool osago = false)
        {
            var results = new List<Result>();

            for (var k = 0; k < calc["results"].Count; k++)
            {
                var result = calc["results"][k];
                var price = result["result"]["total"]["premium"];
                var regions = result["values"]["region"].Childs.Select(i => i.Value).ToList();
                var companyCode = result["info"]["code"];
                var company = companies.Childs.FirstOrDefault(i => i["calculators"].Childs.Any(j => j["code"].Value == companyCode.Value));

                if (company == null) continue;

                var companyName = company["name"];

                if (LocalDatabase.BadCompanies.Contains(companyName.Value)) continue;

                var companyShortName = company["nameshort"];
                var companyRating = company["rating"];

                if (SkipByRegion(regions, companyName)) continue;

                results.Add(new Result
                {
                    CompanyName = companyName.Value,
                    CompanyShortName = companyShortName.Value,
                    Rating = int.Parse(companyRating.Value),
                    Price = GetPrice(price),
                    Regions = regions
                });
            }

            if (osago)
            {
                var price = GetPrice(calc["results"].Childs
                    .Single(i => i["info"]["code"].Value == "OSAGO")["result"]["total"]["premium"]);

                results.ForEach(i => i.Price = price);
                results = results.OrderByDescending(i => i.Rating).ToList();
            }
            else
            {
                results = results.OrderBy(i => i.Price).ToList();
            }

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
                companyButton.Icon.spriteName = companyInfo.Name;
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
            var companyInfo = LocalDatabase.KnownCompanies.FirstOrDefault(i => i.Name == company);

            return companyInfo ?? LocalDatabase.KnownCompanies[0];
        }

        private static bool SkipByRegion(List<string> regions, string companyName)
        {
            if (Profile.Region == Regions.AnyRegion)
            {
                return false;
            }

            if (LocalDatabase.CompanyAdditionalRegions.ContainsKey(companyName))
            {
                regions.AddRange(LocalDatabase.CompanyAdditionalRegions[companyName]);
            }

            foreach (var region in regions)
            {
                foreach (var r in LocalDatabase.RegionList.Single(i => i.Contains(Profile.Region)))
                {
                    if (region.Contains(r))
                    {
                        return false;
                    }
                }
            }

            Debug.Log("SkipByRegion: " + companyName + " = " + string.Join(", ", regions.ToArray()));

            return true;
        }

        private static int GetPrice(JSONNode node)
        {
            return int.Parse(node.Value.Split(Convert.ToChar("."))[0]);
        }
    }
}