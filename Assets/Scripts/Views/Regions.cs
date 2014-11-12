using System.Linq;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Regions : ViewBasePaging
    {
        public const string Default = "Москва";
        public const string RequestRegion = "Вашего нет?\nСообщите нам!";

        protected override Vector2 Size { get { return new Vector2(2, 18); } }
        protected override Vector2 Step { get { return new Vector2(260, 45); } }
        protected override Vector2 Position { get { return new Vector2(240, 430); } }

        protected override void Initialize()
        {
            var regions = LocalDatabase.Data["regions"].Childs.Select(i => i.Value).ToList();

            CreatePages(Mathf.CeilToInt(regions.Count / (Size.x * Size.y)));

            for (var i = 0; i < regions.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (Size.x * Size.y));
                var j = i % (Size.x * Size.y);
                var instance = PrefabsHelper.InstantiateLinkRegion(Pages[page].transform);
                var region = regions[i];

                instance.name = region;
                instance.GetComponent<UILabel>().text = region;

                if (region == RequestRegion)
                {
                    instance.GetComponent<GameButton>().Up += () => GetComponent<Engine>().OpenStore();
                    j++;
                }
                else
                {
                    instance.GetComponent<GameButton>().Up += () => GetComponent<Engine>().SelectRegion(region);
                }

                instance.transform.localPosition =
                    new Vector2(Step.x * Mathf.Floor(j / Size.y) - Position.x, Position.y - Step.y * (j % Size.y));
            }
        }
    }
}