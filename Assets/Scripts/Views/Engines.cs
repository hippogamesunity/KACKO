using System;
using System.Linq;
using Assets.Scripts.Common;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Engines : ViewBasePaging
    {
        public JSONNode JsonGeneration;

        protected override Vector2 Size { get { return new Vector2(1, 13); } }
        protected override Vector2 Step { get { return new Vector2(260, 60); } }
        protected override Vector2 Position { get { return new Vector2(240, 430); } }

        protected override void Initialize()
        {
            var engines = JsonGeneration["engines"].Childs.ToList();

            CreatePages(Mathf.CeilToInt(engines.Count / (Size.x * Size.y)));

            for (var i = 0; i < engines.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (Size.x * Size.y));
                var j = i % (Size.x * Size.y);
                var instance = PrefabsHelper.InstantiateLinkEngine(Pages[page].transform);
                var engine = engines[i]["name"].Value;
                var power = engines[i]["power"].Value;
                var price = GetPrice(engines[i]["price"].Value);
                var production = engines[i]["production"].Value;

                instance.name = engine;
                instance.GetComponent<UILabel>().text = engine;
                instance.transform.FindChild("Details").GetComponent<UILabel>().text = string.Format("{0} л.с. (г.в. {1})", power, production);
                instance.GetComponent<GameButton>().Up += () => GetComponent<Engine>().SelectEngine(engine, power, price, production);
                instance.transform.localPosition =
                    new Vector2(Step.x * Mathf.Floor(j / Size.y) - Position.x, Position.y - Step.y * (j % Size.y));
            }
        }

        private int GetPrice(string price)
        {
            var parts = price.Split(Convert.ToChar("–"));

            if (parts.Length == 1)
            {
                return JsonHelper.GetInt(parts[0]);
            }

            return (JsonHelper.GetInt(parts[0]) + JsonHelper.GetInt(parts[1])) / 2;
        }
    }
}