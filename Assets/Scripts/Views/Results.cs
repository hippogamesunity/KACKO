using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Results : ViewBasePaging
    {
        private Vector2 _step = new Vector2(170, 165);
        private Vector2 _position = new Vector2(170, 410);
        private Vector2 _size = new Vector2(4, 3);

        protected override void Initialize()
        {
            var html = Profile.Results.Replace(Environment.NewLine, null).Replace("\t", null);
            var tbody = Regex.Match(html, @"<tbody>.*?</tbody>").Value;
            var cells = Regex.Matches(tbody, "<td>(?<value>.*?)</td>");
            var results = new Dictionary<string, string>();

            for (var i = 0; i < cells.Count; i += 3)
            {
                results.Add(cells[i + 1].Groups["value"].Value.Trim(), cells[i + 2].Groups["value"].Value.Trim());
            }

            CreatePages(Mathf.CeilToInt(results.Count / (_size.x * _size.y)));

            for (var i = 0; i < results.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (_size.x * _size.y));
                var j = i % (_size.x * _size.y);
                var instance = PrefabsHelper.InstantiateCompanyButton(Pages[page].transform);
                var companyButton = instance.GetComponent<CompanyButton>();

                companyButton.Name.text = results.ElementAt(i).Key;
                companyButton.Price.text = results.ElementAt(i).Value;

                instance.GetComponent<GameButton>().Up += () => Debug.Log("TEST");
                instance.transform.localPosition =
                    new Vector2(_step.x * Mathf.Floor(j / _size.x) - _position.x, _position.y - _step.y * (j % _size.x));
            }
        }
    }
}