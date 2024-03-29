﻿using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Generation : ViewBasePaging
    {
        public JSONNode JsonModel;
        private const string Skip = "[Пропустить]";

        protected override Vector2 Size { get { return new Vector2(1, 18); } }
        protected override Vector2 Step { get { return new Vector2(260, 45); } }
        protected override Vector2 Position { get { return new Vector2(250, 430); } }

        protected override void Initialize()
        {
            var generations = new List<string> { Skip, null };
            
            generations.AddRange(JsonModel["generations"].Childs.Select(i => i["name"].Value));

            CreatePages(Mathf.CeilToInt(generations.Count / (Size.x * Size.y)));

            for (var i = 0; i < generations.Count; i++)
            {
                if (generations[i] == null) continue;

                var page = (int) Mathf.Floor(i / (Size.x * Size.y));
                var j = i % (Size.x * Size.y);
                var position = new Vector2(Step.x * Mathf.Floor(j / Size.y) - Position.x, Position.y - Step.y * (j % Size.y));
                var instance = PrefabsHelper.InstantiateLink(Pages[page].transform, position, 500);
                var generation = generations[i];

                if (generation == Skip)
                {
                    instance.GetComponent<GameButton>().Up += () =>
                    {
                        GetComponent<Year>().Bounds = null;
                        GetComponent<Year>().Open(TweenDirection.Right);
                    };
                }
                else
                {
                    instance.GetComponent<GameButton>().Up += () => GetComponent<Engine>().SelectGeneration(generation);
                }

                instance.name = generation;
                instance.GetComponent<UILabel>().text = generation;
            }
        }
    }
}