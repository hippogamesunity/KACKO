using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.Common;
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
            var html = Profile.Results.Replace(Environment.NewLine, null).Replace("\t", null);
            var tbody = Regex.Match(html, @"<tbody>.*?</tbody>").Value;
            var cells = Regex.Matches(tbody, "<td>(?<value>.*?)</td>");
            var results = new Dictionary<string, int>();

            for (var i = 0; i < cells.Count; i += 3)
            {
                results.Add(cells[i + 1].Groups["value"].Value.Trim(), int.Parse(cells[i + 2].Groups["value"].Value.Replace(" ", null)));
            }

            results = results.Where(i => i.Value > 0).OrderBy(i => i.Value).ToDictionary(i => i.Key, j => j.Value);

            CreatePages(Mathf.CeilToInt(results.Count / (Size.x * Size.y)));

            for (var i = 0; i < results.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (Size.x * Size.y));
                var j = i % (Size.x * Size.y);
                var instance = PrefabsHelper.InstantiateCompanyButton(Pages[page].transform);
                var companyButton = instance.GetComponent<CompanyButton>();

                var result = results.ElementAt(i);

                companyButton.Name.SetText(result.Key);
                companyButton.Price.SetText(result.Value);
                companyButton.Icon.spriteName = GetCompanyLogo(result.Key);
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