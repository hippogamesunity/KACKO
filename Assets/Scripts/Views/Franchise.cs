using System.Linq;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Franchise : ViewBasePaging
    {
        protected override void Initialize()
        {
            var franchises = LocalDatabase.Data["franchise"].Childs.Select(i => i.Value).ToList();

            CreatePages(Mathf.CeilToInt(franchises.Count / (Size.x * Size.y)));

            for (var i = 0; i < franchises.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (Size.x * Size.y));
                var j = i % (Size.x * Size.y);
                var instance = PrefabsHelper.InstantiateLink(Pages[page].transform);
                var franchise = franchises[i];

                instance.name = franchise;
                instance.GetComponent<UILabel>().text = franchise;
                instance.GetComponent<GameButton>().Up += () => GetComponent<Engine>().SelectFranchise(int.Parse(franchise));
                instance.transform.localPosition =
                    new Vector2(Step.x * Mathf.Floor(j / Size.y) - Position.x, Position.y - Step.y * (j % Size.y));
            }
        }
    }
}