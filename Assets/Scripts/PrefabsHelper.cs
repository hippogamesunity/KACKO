using System;
using UnityEngine;

namespace Assets.Scripts
{
	public static class PrefabsHelper
	{
        public static GameObject InstantiateLink(Transform parent)
        {
            return Instantiate("Link", parent);
        }

        public static GameObject InstantiateLinkYear(Transform parent)
        {
            return Instantiate("LinkYear", parent);
        }

        public static GameObject InstantiateLinkRegion(Transform parent)
        {
            return Instantiate("LinkRegion", parent);
        }

        public static GameObject InstantiateLinkGeneration(Transform parent)
        {
            return Instantiate("LinkGeneration", parent);
        }

        public static GameObject InstantiateLinkEngine(Transform parent)
        {
            return Instantiate("LinkEngine", parent);
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