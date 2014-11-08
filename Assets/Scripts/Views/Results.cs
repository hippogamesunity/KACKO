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
                var companyCode = result["info"]["code"];

                if (result["error"].Count > 0)
                {
                    LogFormat("Error in {0}: {1}", companyCode.Value, result["error"]["message"]);

                    continue;
                }

                var price = result["result"]["total"]["premium"];
                var regions = result["values"]["region"].ToList<string>();
                var company = companies.Childs.FirstOrDefault(i => i["calculators"].Childs.Any(j => j["code"].Value == companyCode.Value));

                if (company == null) continue;

                var companyName = company["name"];
                var companyShortName = company["nameshort"];
                var companyRating = company["rating"];

                if (SkipByRegion(regions, companyName)) continue;

                results.Add(new Result
                {
                    CompanyName = companyName.Value,
                    CompanyShortName = companyShortName.Value,
                    Rating = GetInt(companyRating),
                    Price = GetInt(price),
                    Regions = regions
                });
            }

            if (osago)
            {
                var price = GetInt(calc["results"].Childs.Single(i => i["info"]["code"].Value == "OSAGO")["result"]["total"]["premium"]);

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
                var company = GetCompany(result.CompanyName);

                companyButton.Name.SetText(result.CompanyShortName);
                companyButton.Price.SetText(result.Price > 0 ? result.Price.ToPriceInt() : "?");
                companyButton.Icon.spriteName = company["icon"].Value;
                companyButton.Button.Up += () =>
                {
                    var companyResult = GetComponent<CompanyResult>();

                    companyResult.Initialize(result, company);
                    companyResult.Open(TweenDirection.Right);
                };
                instance.transform.localPosition = new Vector2(i % 3 * Step.x - Position.x, Position.y - Mathf.Floor(j / Size.x) * Step.y);
            }
        }

        public static JSONNode GetCompany(string companyName)
        {
            var companyInfo = LocalDatabase.Data["companies"][companyName];

            return companyInfo.Count > 0 ? companyInfo : new JSONClass { { "icon", "Default" } };
        }

        private static bool SkipByRegion(List<string> regions, string companyName)
        {
            if (Profile.Region == Regions.AnyRegion)
            {
                return false;
            }

            var customRegions = LocalDatabase.Data["companies"][companyName]["regions"];

            if (customRegions.Count > 0)
            {
                regions.AddRange(customRegions.ToList<string>());
                regions = regions.Distinct().ToList();
            }

            if (regions.Any(region => LocalDatabase.Data["regions"].Childs.Single(i => i[0].Value == Profile.Region).Childs.Any(r => region.Contains(r))))
            {
                return false;
            }

            LogFormat("Company {0} was skipped by region {1}. Company regions: {2}",
                companyName, Profile.Region, string.Join(", ", regions.ToArray()));

            return true;
        }

        private static int GetInt(JSONNode node)
        {
            try
            {
                return (int) float.Parse(node.Value.Replace(",", ".").Replace(" ", null));
            }
            catch
            {
                return 0;
            }
        }
    }
}