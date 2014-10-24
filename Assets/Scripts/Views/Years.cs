using System;
using System.Linq;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Years : ViewBasePaging
    {
        private Vector2 _size = new Vector2(14, 3);

        protected override void Initialize()
        {
            var years = Enumerable.Range(0, 40).Select(i => DateTime.Now.Year - i).ToList();

            CreatePages(Mathf.CeilToInt(years.Count / (_size.x * _size.y)));

            for (var i = 0; i < years.Count; i++)
            {
                var page = (int)Mathf.Floor(i / (_size.x * _size.y));
                var j = i % (_size.x * _size.y);
                var instance = PrefabsHelper.InstantiateLink(Pages[page].transform);
                var year = years[i];

                instance.name = Convert.ToString(year);
                instance.GetComponent<UILabel>().SetText(year);
                instance.GetComponent<SelectButton>().Selected += () => GetComponent<Engine>().SelectYear(year);
                instance.transform.localPosition =
                    new Vector2(Step.x * Mathf.Floor(j / _size.x) - Position.x, Position.y - Step.y * (j % _size.x));
            }
        }
    }
}