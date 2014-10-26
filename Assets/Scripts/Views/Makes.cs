using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Makes : ViewBasePaging
    {
        protected override void Initialize()
        {
            var makes = Profile.Cars; //.Childs.Where(i => HasModels(Profile.Models[i["id"]])).ToList();

            CreatePages(Mathf.CeilToInt(makes.Count / (Size.x * Size.y)));

            for (var i = 0; i < makes.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (Size.x * Size.y));
                var j = i % (Size.x * Size.y);
                var instance = PrefabsHelper.InstantiateLink(Pages[page].transform);
                var make = makes[i]["name"].Value;
                var id = int.Parse(makes[i]["id"].Value);

                instance.name = make;
                instance.GetComponent<UILabel>().text = make;
                instance.GetComponent<GameButton>().Up += () => GetComponent<Engine>().SelectMake(id);
                instance.transform.localPosition =
                    new Vector2(Step.x * Mathf.Floor(j / Size.y) - Position.x, Position.y - Step.y * (j % Size.y));
            }
        }

        //private static bool HasModels(JSONNode models)
        //{
        //    if (models.Count == 1 && models[0]["name"] != "\"*\"") return false;

        //    return models.Count > 0;
        //}
    }
}