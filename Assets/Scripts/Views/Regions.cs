using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Regions : ViewBasePaging
    {
        public const string AnyRegion = "Любой";

        protected override void Initialize()
        {
            var regions = LocalDatabase.Data["regions"];

            CreatePages(Mathf.CeilToInt(regions.Count / (Size.x * Size.y)));

            for (var i = 0; i < regions.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (Size.x * Size.y));
                var j = i % (Size.x * Size.y);
                var instance = PrefabsHelper.InstantiateLink(Pages[page].transform);
                var region = regions[i][0];

                instance.name = region;
                instance.GetComponent<UILabel>().text = region;
                instance.GetComponent<GameButton>().Up += () => GetComponent<Engine>().SelectRegion(region);
                instance.transform.localPosition =
                    new Vector2(Step.x * Mathf.Floor(j / Size.y) - Position.x, Position.y - Step.y * (j % Size.y));

                if (i >= 2)
                {
                    instance.GetComponent<GameButton>().Enabled = false;
                }
            }
        }
    }
}