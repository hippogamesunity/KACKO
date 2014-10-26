using System;
using System.Linq;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Years : ViewBasePaging
    {
        protected override Vector2 Size { get { return new Vector2(3, 18); } }
        protected override Vector2 Step { get { return new Vector2(180, 45); } }
        protected override Vector2 Position { get { return new Vector2(180, 430); } }

        protected override void Initialize()
        {
            var years = Enumerable.Range(0, 40).Select(i => DateTime.Now.Year - i).ToList();

            CreatePages(Mathf.CeilToInt(years.Count / (Size.x * Size.y)));

            for (var i = 0; i < years.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (Size.x * Size.y));
                var j = i % (Size.x * Size.y);
                var instance = PrefabsHelper.InstantiateLinkYear(Pages[page].transform);
                var year = years[i];

                instance.name = Convert.ToString(year);
                instance.GetComponent<UILabel>().SetText(year);
                instance.GetComponent<GameButton>().Up += () => GetComponent<Engine>().SelectYear(year);
                instance.transform.localPosition =
                    new Vector2(Step.x * Mathf.Floor(j / Size.y) - Position.x, Position.y - Step.y * (j % Size.y));
            }
        }
    }
}