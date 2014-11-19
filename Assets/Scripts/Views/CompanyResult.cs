using Assets.Scripts.Common;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class CompanyResult : ViewBase
    {
        public UILabel Name;
        public UISprite Icon;
        public GameButton Options;
        public GameButton Navigate;
        public GameButton Call;
        public UISprite[] Stars;
        public UILabel Reliability;
        public UILabel Price;
        public string Url;
        public string Phone;

        public void Start()
        {
            Navigate.Up += () => Application.OpenURL(Url);
            Call.Up += () => Application.OpenURL(string.Format("tel:{0}", Phone));
        }

        public void Initialize(Result result, JSONNode company)
        {
            Name.SetText(result.CompanyName);
            Price.SetText("{0} руб. ({1:F2}%)", result.Price.ToPriceInt(), 100 * (float) result.Price / Profile.Price);
            
            for (var i = 0; i < Stars.Length; i++)
            {
                Stars[i].color = result.Rating > i
                    ? ColorHelper.GetColor(255, 255, 255)
                    : ColorHelper.GetColor(0, 0, 0, 100);
            }

            Reliability.text = company["reliability"].Value;
            Url = company["site"].Value;
            Phone = company["phone"]["Москва"].Value;
            Icon.spriteName = company["icon"].Value;

            Navigate.Enabled = company["site"].Value != "";
            Call.Enabled = company["phone"]["Москва"].Value != "";

            #if UNITY_IPHONE

            Phone = Phone.Replace("(", null).Replace("(", null).Replace(" ", "-");

            #endif

            var options = result.Json["options"]["Условия страхования"];

            //foreach (var key in options.AsObject.Keys)
            //{
            //    Debug.Log(string.Format("{0}: {1}", options[key]["title"], result.Json["data"][key]));
            //}

            Options.Enabled = options.Count > 0;
            GetComponent<Options>().CompanyResult = result.Json;
        }
    }
}