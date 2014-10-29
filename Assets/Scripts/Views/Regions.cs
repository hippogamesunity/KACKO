using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Regions : ViewBasePaging
    {
        public const string AnyRegion = "Любой";

        protected override void Initialize()
        {
            CreatePages(Mathf.CeilToInt(LocalDatabase.RegionList.Count / (Size.x * Size.y)));

            for (var i = 0; i < LocalDatabase.RegionList.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (Size.x * Size.y));
                var j = i % (Size.x * Size.y);
                var instance = PrefabsHelper.InstantiateLink(Pages[page].transform);
                var region = LocalDatabase.RegionList[i][0];

                instance.name = region;
                instance.GetComponent<UILabel>().text = region;
                instance.GetComponent<GameButton>().Up += () => GetComponent<Engine>().SelectRegion(region);
                instance.transform.localPosition =
                    new Vector2(Step.x * Mathf.Floor(j / Size.y) - Position.x, Position.y - Step.y * (j % Size.y));
            }
        }
    }
}