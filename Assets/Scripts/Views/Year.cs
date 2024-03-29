﻿using System;
using System.Linq;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Year : ViewBasePaging
    {
        public string Bounds;

        protected override void Initialize()
        {
            var years = Enumerable.Range(0, 40).Select(i => DateTime.Now.Year - i).ToList();

            CreatePages(Mathf.CeilToInt(years.Count / (Size.x * Size.y)));

            for (var i = 0; i < years.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (Size.x * Size.y));
                var j = i % (Size.x * Size.y);
                var position = new Vector2(Step.x * Mathf.Floor(j / Size.y) - Position.x, Position.y - Step.y * (j % Size.y));
                var instance = PrefabsHelper.InstantiateLink(Pages[page].transform, position);
                var year = years[i];

                instance.name = Convert.ToString(year);
                instance.GetComponent<UILabel>().SetText(year);
                instance.GetComponent<GameButton>().Up += () => GetComponent<Engine>().SelectYear(year);

                if (Bounds == null) continue;

                int from, to;

                ParseProduction(Bounds, out from, out to);
                instance.GetComponent<GameButton>().Enabled = year >= from && year <= to;
            }
        }

        private static void ParseProduction(string production, out int from, out int to)
        {
            if (string.IsNullOrEmpty(production))
            {
                from = to = 0;
            }
            else
            {
                var parts = production.Split(Convert.ToChar("-"));

                int.TryParse(parts[0], out from);
                int.TryParse(parts[1], out to);

                if (to == 0)
                {
                    to = 9999;
                }
            }
        }
    }
}