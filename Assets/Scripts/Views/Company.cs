using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Company : ViewBase
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

        public void Initialize(Result result, CompanyInfo companyInfo)
        {
            Name.SetText(result.CompanyName);
            Price.SetText("{0} руб.", result.Price.ToPriceInt());
            
            for (var i = 0; i < Stars.Length; i++)
            {
                Stars[i].color = result.Rating > i
                    ? ColorHelper.GetColor(255, 255, 255)
                    : ColorHelper.GetColor(0, 0, 0, 100);
            }

            Url = companyInfo.Url;
            Icon.spriteName = companyInfo.Name;
            Navigate.Enabled = companyInfo.Url != null;
        }
    }
}