using System.Linq;
using Assets.Scripts.Common;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Generation : ViewBasePaging
    {
        public JSONNode JsonModel;

        protected override void Initialize()
        {
            var generations = JsonModel["generations"].Childs.Select(i => i["name"].Value).ToList();

            CreatePages(Mathf.CeilToInt(generations.Count / (Size.x * Size.y)));

            for (var i = 0; i < generations.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (Size.x * Size.y));
                var j = i % (Size.x * Size.y);
                var instance = PrefabsHelper.InstantiateLink(Pages[page].transform);
                var generation = generations[i];

                instance.name = generation;
                instance.GetComponent<UILabel>().text = generation;
                instance.GetComponent<GameButton>().Up += () => GetComponent<Engine>().SelectGeneration(generation);
                instance.transform.localPosition =
                    new Vector2(Step.x * Mathf.Floor(j / Size.y) - Position.x, Position.y - Step.y * (j % Size.y));
            }
        }
    }
}