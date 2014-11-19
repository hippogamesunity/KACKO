using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
	public static class PrefabsHelper
	{
        public static GameObject InstantiateLink(Transform parent, Vector2 position, int width = 150, UIWidget.Pivot pivot = UIWidget.Pivot.BottomLeft)
        {
            var link = Instantiate("Link", parent);
            var lables = new List<UILabel>
            {
                link.GetComponent<UILabel>(),
                link.GetComponentInChildren<UILabel>()
            };

            foreach (var label in lables.Where(i => i != null))
            {
                label.width = width;
                label.pivot = pivot;
            }

            link.transform.localPosition = position;

            var collider = link.GetComponent<BoxCollider2D>();

            collider.size = new Vector2(width, 40);
            collider.center = pivot == UIWidget.Pivot.Center ? Vector2.zero : new Vector2(width / 2f, 0);

            return link;
        }

        public static GameObject InstantiateLinkEngine(Transform parent)
        {
            return Instantiate("LinkEngine", parent);
        }

        public static GameObject InstantiateInfo(Transform parent)
        {
            return Instantiate("Info", parent);
        }

        public static GameObject InstantiateOption(Transform parent)
        {
            return Instantiate("Option", parent);
        }

        public static GameObject InstantiatePage(Transform parent)
        {
            return Instantiate("Page", parent);
        }

        public static GameObject InstantiatePaging(Transform parent)
        {
            return Instantiate("Paging", parent);
        }

        public static GameObject InstantiateCompanyButton(Transform parent)
        {
            return Instantiate("CompanyButton", parent);
        }

        private static GameObject Instantiate(string name, Transform parent)
        {
            try
            {
                var instance = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Prefabs/" + name, typeof(GameObject)));

                instance.name = name;
                instance.transform.parent = parent;
                instance.transform.localScale = Vector3.one;

                return instance;
            }
            catch
            {
                throw new Exception("Prefab not found: " + name);
            }
        }
	}
}