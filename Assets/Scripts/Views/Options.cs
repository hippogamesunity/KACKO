using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Options : ViewBasePaging
    {
        public JSONNode CompanyResult;

        protected override Vector2 Size { get { return new Vector2(1, 18); } }
        protected override Vector2 Step { get { return new Vector2(260, 60); } }
        protected override Vector2 Position { get { return new Vector2(250, 430); } }

        protected override void Initialize()
        {
            var options = CompanyResult["options"]["Условия страхования"];

            //foreach (var key in options.AsObject.Keys)
            //{
            //    Debug.Log(string.Format("{0}: {1}", options[key]["title"], CompanyResult["data"][key]));
            //}

            CreatePages(Mathf.CeilToInt(options.Count / (Size.x * Size.y)));
            
            for (var i = 0; i < options.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (Size.x * Size.y));
                var j = i % (Size.x * Size.y);
                var instance = PrefabsHelper.InstantiateInfo(Pages[page].transform);
                var key = options.AsObject.Keys[i];
                var title = options[key]["title"].Value;
                var value = CompanyResult["data"][key].Value;

                if (value == "12") value = "1 год";
                if (value == "true") value = "Да";
                if (value == "false") value = "Нет";

                instance.name = title;
                instance.GetComponent<UILabel>().text = title;
                instance.transform.FindChild("Details").GetComponent<UILabel>().text = value;
                instance.transform.localPosition =
                    new Vector2(Step.x * Mathf.Floor(j / Size.y) - Position.x, Position.y - Step.y * (j % Size.y));
            }
        }
    }
}