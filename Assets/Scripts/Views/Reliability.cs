using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Reliability : ViewBasePaging
    {
        protected override Vector2 Size { get { return new Vector2(1, 18); } }
        protected override Vector2 Step { get { return new Vector2(260, 60); } }
        protected override Vector2 Position { get { return new Vector2(250, 430); } }

        protected override void Initialize()
        {
            var rating = new Dictionary<string, string>
            {
                { "А++", "Исключительно высокий уровень надежности" },
                { "А+", "Очень высокий уровень надежности" },
                { "А", "Высокий уровень надежности" },
                { "B++", "Приемлемый уровень надежности" },
                { "B+", "Достаточный уровень надежности" },
                { "B", "Удовлетворительный уровень надежности" },
                { "C++", "Низкий уровень надежности" },
                { "C+", "Очень низкий уровень надежности (преддефолтный)" },
                { "D", "Банкротство" },
                { "E", "Отзыв лицензии или ликвидация" },
            };

            CreatePages(Mathf.CeilToInt(rating.Count / (Size.x * Size.y)));
            
            for (var i = 0; i < rating.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (Size.x * Size.y));
                var j = i % (Size.x * Size.y);
                var instance = PrefabsHelper.InstantiateInfo(Pages[page].transform);
                var value = rating.ElementAt(i).Key;
                var description = rating.ElementAt(i).Value;

                instance.name = value;
                instance.GetComponent<UILabel>().text = value;
                instance.transform.FindChild("Details").GetComponent<UILabel>().text = description;
                instance.transform.localPosition =
                    new Vector2(Step.x * Mathf.Floor(j / Size.y) - Position.x, Position.y - Step.y * (j % Size.y));
            }
        }
    }
}