using System.Linq;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Model : ViewBasePaging
    {
        protected override void Initialize()
        {
            var models = Profile.Cars.Childs.Single(i => i["name"].Value == Profile.Make)["models"];

            CreatePages(Mathf.CeilToInt(models.Count / (Size.x * Size.y)));

            for (var i = 0; i < models.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (Size.x * Size.y));
                var j = i % (Size.x * Size.y);
                var position = new Vector2(Step.x * Mathf.Floor(j / Size.y) - Position.x, Position.y - Step.y * (j % Size.y));
                var instance = PrefabsHelper.InstantiateLink(Pages[page].transform, position);
                var model = models[i]["name"];

                instance.name = model;
                instance.GetComponent<UILabel>().text = model;
                instance.GetComponent<GameButton>().Up += () => GetComponent<Engine>().SelectModel(model);
            }
        }
    }
}