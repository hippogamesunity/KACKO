using System.Collections.Generic;
using Assets.Scripts.Views;

namespace Assets.Scripts
{
    public static class LocalDatabase
    {
        public static readonly List<CompanyInfo> KnownCompanies = new List<CompanyInfo>
        {
            new CompanyInfo("Default", null),
            new CompanyInfo("Allianz", "http://www.allianz.ru/"),
            new CompanyInfo("АМКОполис", "http://www.sk-amko.ru/"),
            new CompanyInfo("Алроса", "http://www.ic-alrosa.ru/"),
            new CompanyInfo("Альфа-Страхование", "http://www.alfastrah.ru/"),
            new CompanyInfo("Британский Страховой Дом", "http://bihouse.ru/"),
            new CompanyInfo("ВСК Страховой Дом", "http://www.vsk.ru/"),
            new CompanyInfo("ВТБ", "http://www.vtbins.ru/"),
            new CompanyInfo("Гайде", "http://www.guideh.com/"),
            new CompanyInfo("Геополис", "http://www.geopolis.ru/"),
            new CompanyInfo("Гефест", "http://www.gefest.ru/"),
            new CompanyInfo("ЖАСО", "http://www.zhaso.ru/"),
            new CompanyInfo("Инвест-Альянс", "http://ins-invest.ru/"),
            new CompanyInfo("Ингосстрах", "http://www.ingos.ru/ru/"),
            new CompanyInfo("Компаньон", "http://companion-group.ru/"),
            new CompanyInfo("Купеческое", "http://www.kupecheskoe.ru/"),
            new CompanyInfo("Либерти Страхование (бывший КИТ Финанс)", "http://www.liberty24.ru/"),
            new CompanyInfo("МАКС", "http://www.makc.ru/"),
            new CompanyInfo("НАСКО", "http://nasko.ru/"),
            new CompanyInfo("Объединённая Страховая Компания", "http://www.osk-ins.ru/"),
            new CompanyInfo("Оранта Страхование", "http://www.oranta-sk.ru/"),
            new CompanyInfo("РЕСО-Гарантия", "http://www.reso.ru/"),
            new CompanyInfo("Ренессанс Страхование", "http://www.renins.com/"),
            new CompanyInfo("Росгосстрах", "http://www.rgs.ru/"),
            new CompanyInfo("СГ Московская Страховая Компания", "http://sgmsk.ru/"),
            new CompanyInfo("СОГАЗ", "http://www.sogaz.ru/"),
            new CompanyInfo("Северная Казна", "http://www.kazna.com/"),
            new CompanyInfo("Согласие", "http://www.soglasie.ru/"),
            new CompanyInfo("Сургутнефтегаз", "https://www.sngi.ru/"),
            new CompanyInfo("УралСиб", "http://www.uralsibins.ru"),
            new CompanyInfo("Цюрих", "http://www.zurich.ru/"),
            new CompanyInfo("Энергогарант", "http://www.energogarant.ru/"),
            new CompanyInfo("Эрго Русь", "http://www.ergorussia.ru/")
        };

        public static readonly List<string> BadCompanies = new List<string>
        {
            "Важно.Новое страхование"
        };

        public static readonly Dictionary<string, List<string>> CompanyAdditionalRegions = new Dictionary<string, List<string>>
        {
            { "Геополис", new List<string> { "Москва" } },
            { "МАКС", new List<string> { "Москва" } },
            { "Сургутнефтегаз", new List<string> { "Москва", "Санкт-Петербург", "Екатеринбург", "Набережные Челны", "Кемерово" } },
            { "Альфа-Страхование", new List<string> { "Москва", "Санкт-Петербург", "Архангельск", "Казань", "Екатеринбург", "Нижний Новгород", "Омск", "Новосибирск", "Хабаровск", "Иркутск", "Красноярск", "Томск", "Челябинск", "Воронеж", "Саратов", "Курск", "Смоленск", "Саратов", "Владивосток" } },
        };

        public static readonly List<string[]> RegionList = new List<string[]>
        {
            new[] { Regions.AnyRegion },
            new[] { "Москва", "Московская область" },
            new[] { "Питер", "Санкт-Петербург", "Ленинградская область", "СПб" },
            new[] { "Астрахань" },
            new[] { "Барнаул" },
            new[] { "Белгород" },
            new[] { "Брянск" },
            new[] { "Владивосток" },
            new[] { "Волгоград" },
            new[] { "Воронеж" },
            new[] { "Екатеринбург" },
            new[] { "Иваново" },
            new[] { "Ижевск" },
            new[] { "Иркутск" },
            new[] { "Казань" },
            new[] { "Калининград" },
            new[] { "Кемерово", "Кемеровский" },
            new[] { "Киров" },
            new[] { "Краснодар" },
            new[] { "Красноярск" },
            new[] { "Курск" },
            new[] { "Липецк" },
            new[] { "Магнитогорск" },
            new[] { "Махачкала" },
            new[] { "Н. Челны", "Набережные Челны" },
            new[] { "Н. Новгород", "Нижний Новгород" },
            new[] { "Нижний Тагил" },
            new[] { "Новокузнецк" },
            new[] { "Новосибирск" },
            new[] { "Омск" },
            new[] { "Оренбург" },
            new[] { "Пенза" },
            new[] { "Пермь" },
            new[] { "Ростов-на-Дону" },
            new[] { "Рязань" },
            new[] { "Самара" },
            new[] { "Саратов" },
            new[] { "Сочи" },
            new[] { "Ставрополь" },
            new[] { "Тверь" },
            new[] { "Тольятти" },
            new[] { "Томск" },
            new[] { "Тула" },
            new[] { "Тюмень" },
            new[] { "Улан-Удэ" },
            new[] { "Ульяновск" },
            new[] { "Уфа" },
            new[] { "Хабаровск" },
            new[] { "Чебоксары" },
            new[] { "Челябинск" },
            new[] { "Ярославль" }
        };
    }
}