﻿using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Models : ViewBasePaging
    {
        protected override void Initialize()
        {
            var models = Profile.Cars.Childs.Single(i => int.Parse(i["id"].Value) == Profile.Make)["models"];

            CreatePages(Mathf.CeilToInt(models.Count / (Size.x * Size.y)));

            for (var i = 0; i < models.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (Size.x * Size.y));
                var j = i % (Size.x * Size.y);
                var instance = PrefabsHelper.InstantiateLink(Pages[page].transform);
                var model = models[i]["name"];
                var id = int.Parse(models[i]["id"]);

                instance.name = model;
                instance.GetComponent<UILabel>().text = model;
                instance.GetComponent<GameButton>().Up += () => GetComponent<Engine>().SelectModel(id);
                instance.transform.localPosition =
                    new Vector2(Step.x * Mathf.Floor(j / Size.y) - Position.x, Position.y - Step.y * (j % Size.y));
            }
        }
    }
}