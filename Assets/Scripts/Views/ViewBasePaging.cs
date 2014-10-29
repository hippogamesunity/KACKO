using System.Collections.Generic;
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

        protected virtual Vector2 Size { get { return new Vector2(3, 18); } }
        protected virtual Vector2 Step { get { return new Vector2(180, 45); } }
        protected virtual Vector2 Position { get { return new Vector2(250, 430); } }

        public void SlideRight()
        {
            if (Index > 0)
            {
                Pagings[Index - 1].GetComponent<SelectButton>().Pressed = true;
            }
        }

        public void SlideLeft()
        {
            if (Index < Pagings.Count - 1)
            {
                Pagings[Index + 1].GetComponent<SelectButton>().Pressed = true;
            }
        }

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
                    var hide = next > Index ? TweenDirection.Left : TweenDirection.Right;
                    var show = next > Index ? TweenDirection.Right : TweenDirection.Left;

                    var p1 = Pages[Index].GetComponent<TweenPanel>();
                    var p2 = Pages[next].GetComponent<TweenPanel>();

                    p1.GetComponent<TweenPanel>().Hide(hide);
                    p2.GetComponent<TweenPanel>().Hide(show, 0);
                    p2.GetComponent<TweenPanel>().Show(show);

                    Index = next;
                };

                Pagings.Add(instance);
            }
        }
    }
}