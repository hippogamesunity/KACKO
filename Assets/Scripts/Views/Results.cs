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

                if (result["error"].Count > 0 && !osago)
                {
                    LogFormat("Error in {0}: {1}", companyCode.Value, result["error"]["message"]);

                    continue;
                }

                Debug.Log(companyCode);

                var price = result["result"]["total"]["premium"];
                var company = companies.Childs.FirstOrDefault(i => i["calculators"].Childs.Any(j => j["code"].Value == companyCode.Value));

                if (company == null) continue;

                var companyName = company["name"];
                var companyShortName = company["nameshort"];
                var companyRating = company["rating"];

                results.Add(new Result
                {
                    Json = result,
                    CompanyName = companyName.Value,
                    CompanyShortName = companyShortName.Value,
                    Rating = JsonHelper.GetInt(companyRating),
                    Price = JsonHelper.GetInt(price)
                });
            }

            if (osago)
            {
                var price = JsonHelper.GetInt(calc["results"].Childs.Single(i => i["info"]["code"].Value == "OSAGO")["result"]["total"]["premium"]);

                results.ForEach(i => i.Price = (int) (price * Profile.Kbm));
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
    }
}