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
            var companyInfo = KnownCompanies.FirstOrDefault(i => i.Name == company);

            return companyInfo ?? KnownCompanies[0];
        }

        private static readonly List<CompanyInfo> KnownCompanies = new List<CompanyInfo>
        {
            new CompanyInfo("Default", null),
            new CompanyInfo("Allianz", "http://www.allianz.ru/"),
            new CompanyInfo("АМКОполис", "http://www.sk-amko.ru/"),
            new CompanyInfo("Алроса", "http://www.ic-alrosa.ru/"),
            new CompanyInfo("Альфа-Страхование", "http://www.alfastrah.ru/"),
            new CompanyInfo("Британский Страховой Дом", "http://bihouse.ru/"),
            new CompanyInfo("ВСК Страховой Дом", "http://www.vsk.ru/"),
            new CompanyInfo("ВТБ", "http://www.vtbins.ru/"),
            new CompanyInfo("Гайде", "http://www.guideh.com/"),
            new CompanyInfo("Геополис", "http://www.geopolis.ru/"),
            new CompanyInfo("Гефест", "http://www.gefest.ru/"),
            new CompanyInfo("ЖАСО", "http://www.zhaso.ru/"),
            new CompanyInfo("Инвест-Альянс", "http://ins-invest.ru/"),
            new CompanyInfo("Ингосстрах", "http://www.ingos.ru/ru/"),
            new CompanyInfo("Компаньон", "http://companion-group.ru/"),
            new CompanyInfo("Купеческое", "http://www.kupecheskoe.ru/"),
            new CompanyInfo("Либерти Страхование (бывший КИТ Финанс)", "http://www.liberty24.ru/"),
            new CompanyInfo("МАКС", "http://www.makc.ru/"),
            new CompanyInfo("НАСКО", "http://nasko.ru/"),
            new CompanyInfo("Объединённая Страховая Компания", "http://www.osk-ins.ru/"),
            new CompanyInfo("Оранта Страхование", "http://www.oranta-sk.ru/"),
            new CompanyInfo("РЕСО-Гарантия", "http://www.reso.ru/"),
            new CompanyInfo("Ренессанс", "http://www.renins.com/"),
            new CompanyInfo("Росгосстрах", "http://www.rgs.ru/"),
            new CompanyInfo("СГ Московская Страховая Компания", "http://sgmsk.ru/"),
            new CompanyInfo("СОГАЗ", "http://www.sogaz.ru/"),
            new CompanyInfo("Северная Казна", "http://www.kazna.com/"),
            new CompanyInfo("Согласие", "http://www.soglasie.ru/"),
            new CompanyInfo("Сургутнефтегаз", "https://www.sngi.ru/"),
            new CompanyInfo("УралСиб", "http://www.uralsibins.ru"),
            new CompanyInfo("Цюрих", "http://www.zurich.ru/"),
            new CompanyInfo("Энергогарант", "http://www.energogarant.ru/"),
            new CompanyInfo("Эрго Русь", "http://www.ergorussia.ru/")
        };

        private static int GetPrice(JSONNode node)
        {
            return int.Parse(node.Value.Split(Convert.ToChar("."))[0]);
        }
    }
}