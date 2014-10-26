using System;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Models : ViewBasePaging
    {
        protected override void Initialize()
        {
            var models = Profile.Models[Convert.ToString(Profile.Make)];

            CreatePages(Mathf.CeilToInt(models.Count / (Size.x * Size.y)));

            for (var i = 0; i < models.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (Size.x * Size.y));
                var j = i % (Size.x * Size.y);
                var instance = PrefabsHelper.InstantiateLink(Pages[page].transform);
                var model = models[i]["text"];
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