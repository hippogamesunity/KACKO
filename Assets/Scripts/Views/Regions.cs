using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Regions : ViewBasePaging
    {
        public const string AnyRegion = "Любой";

        protected override void Initialize()
        {
            CreatePages(Mathf.CeilToInt(RegionList.Count / (Size.x * Size.y)));

            for (var i = 0; i < RegionList.Count; i++)
            {
                var page = (int) Mathf.Floor(i / (Size.x * Size.y));
                var j = i % (Size.x * Size.y);
                var instance = PrefabsHelper.InstantiateLink(Pages[page].transform);
                var region = RegionList[i];

                instance.name = region;
                instance.GetComponent<UILabel>().text = region;
                instance.GetComponent<GameButton>().Up += () => GetComponent<Engine>().SelectRegion(region);
                instance.transform.localPosition =
                    new Vector2(Step.x * Mathf.Floor(j / Size.y) - Position.x, Position.y - Step.y * (j % Size.y));
            }
        }

        public static readonly List<string> RegionList = new List<string>
        {
            AnyRegion,
            "Москва",
            "Санкт-Петербург",
            "Новосибирск",
            "Екатеринбург",
            "Нижний Новгород",
            "Казань",
            "Самара",
            "Омск",
            "Челябинск",
            "Ростов-на-Дону",
            "Уфа",
            "Волгоград",
            "Красноярск",
            "Пермь",
            "Воронеж",
            "Саратов",
            "Краснодар",
            "Тольятти",
            "Барнаул",
            "Тюмень",
            "Ижевск",
            "Ульяновск",
            "Иркутск",
            "Владивосток",
            "Ярославль",
            "Хабаровск",
            "Махачкала",
            "Оренбург",
            "Новокузнецк",
            "Томск",
            "Кемерово",
            "Рязань",
            "Астрахань",
            "Пенза",
            "Набережные Челны",
            "Липецк",
            "Тула",
            "Киров",
            "Чебоксары",
            "Калининград",
            "Курск",
            "Улан-Удэ",
            "Ставрополь",
            "Магнитогорск",
            "Брянск",
            "Иваново",
            "Тверь",
            "Белгород",
            "Сочи",
            "Нижний Тагил"
        };
    }
}