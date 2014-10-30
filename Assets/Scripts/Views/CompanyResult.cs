using Assets.Scripts.Common;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class CompanyResult : ViewBase
    {
        public UILabel Name;
        public UISprite Icon;
        public GameButton Navigate;
        public UISprite[] Stars;
        public UILabel Price;
        public string Url;

        public void Start()
        {
            Navigate.Up += () => Application.OpenURL(Url);
        }

        public void Initialize(Result result, JSONNode company)
        {
            Name.SetText(result.CompanyName);
            Price.SetText("{0} руб.", result.Price.ToPriceInt());
            
            for (var i = 0; i < Stars.Length; i++)
            {
                Stars[i].color = result.Rating > i
                    ? ColorHelper.GetColor(255, 255, 255)
                    : ColorHelper.GetColor(0, 0, 0, 100);
            }

            Url = company["site"].Value;
            Icon.spriteName = company["icon"].Value;
            Navigate.Enabled = company["site"].Value != "";
        }
    }
}