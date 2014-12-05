using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Kbm : ViewBasePaging
    {
        protected override void Initialize()
        {
            var kbms = LocalDatabase.Data["kbm"].Childs.Select(i => i.Value).ToList();

            CreatePages(Mathf.CeilToInt(kbms.Count / (Size.x * Size.y)));

            for (var i = 0; i < kbms.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (Size.x * Size.y));
                var j = i % (Size.x * Size.y);
                var position = new Vector2(Step.x * Mathf.Floor(j / Size.y) - Position.x, Position.y - Step.y * (j % Size.y));
                var instance = PrefabsHelper.InstantiateLink(Pages[page].transform, position, 240);
                var kbm = kbms[i];

                instance.name = kbm;
                instance.GetComponent<UILabel>().text = kbm;
                instance.GetComponent<GameButton>().Up += () => GetComponent<Engine>().SelectKbm(GetKbm(kbm));
            }
        }

        public static float GetKbm(string value)
        {
            return float.Parse(Regex.Match(value, @"\((?<value>[\d\.]+)\)").Groups["value"].Value);
        }
    }
}