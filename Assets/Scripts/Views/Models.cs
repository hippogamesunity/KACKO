using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Models : ViewBasePaging
    {
        private Vector2 _size = new Vector2(14, 3);

        protected override void Initialize()
        {
            var models = Profile.Models[Convert.ToString(Profile.Make)];

            CreatePages(Mathf.CeilToInt(models.Count / (_size.x * _size.y)));

            for (var i = 0; i < models.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (_size.x * _size.y));
                var j = i % (_size.x * _size.y);
                var instance = PrefabsHelper.InstantiateLink(Pages[page].transform);
                var model = models[i]["text"];
                var id = int.Parse(models[i]["id"]);

                instance.name = model;
                instance.GetComponent<UILabel>().text = model;
                instance.GetComponent<SelectButton>().Selected += () => GetComponent<Engine>().SelectModel(id);
                instance.transform.localPosition =
                    new Vector2(Step.x * Mathf.Floor(j / _size.x) - Position.x, Position.y - Step.y * (j % _size.x));
            }
        }
    }
}