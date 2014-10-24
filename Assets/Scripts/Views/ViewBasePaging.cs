﻿using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class ViewBasePaging : ViewBase
    {
        public Transform Paging;
        protected readonly List<GameObject> Pages = new List<GameObject>();
        protected readonly List<GameObject> Pagings = new List<GameObject>();
        protected int Index;
        protected Vector2 Size = new Vector2(14, 3);
        protected Vector2 Step = new Vector2(170, 45);
        protected Vector2 Position = new Vector2(230, 430);

        protected void CreatePages(int count)
        {
            foreach (var pages in Pages)
            {
                Destroy(pages.gameObject);
            }

            foreach (var paging in Pagings)
            {
                Destroy(paging.gameObject);
            }

            Pages.Clear();
            Pagings.Clear();

            for (var i = 0; i < count; i++)
            {
                var instance = PrefabsHelper.InstantiatePage(TweenPanel.transform);

                instance.name += "#" + (i + 1);
                instance.transform.localPosition = i == 0 ? new Vector2(0, 0) : new Vector2(800, 0);

                Pages.Add(instance);
            }

            if (Pages.Count == 1) return;

            var random = CRandom.GetRandom(1000);

            for (var i = 0; i < Pages.Count; i++)
            {
                var instance = PrefabsHelper.InstantiatePaging(Paging);
                var selectButton = instance.GetComponent<SelectButton>();
                var next = i;

                instance.transform.localPosition = new Vector2(-50 * (Pages.Count - 1) + 100 * i, 0);
                selectButton.Tag = (int) random;

                if (i == 0)
                {
                    selectButton.Pressed = true;
                }

                selectButton.Selected += () =>
                {
                    Pages[Index].GetComponent<TweenPanel>().Hide(next > Index ? TweenDirection.Left : TweenDirection.Right);
                    Pages[next].GetComponent<TweenPanel>().Show(next > Index ? TweenDirection.Right : TweenDirection.Left);
                    Index = next;
                };

                Pagings.Add(instance);
            }
        }
    }
}