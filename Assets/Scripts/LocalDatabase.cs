using Assets.Scripts.Views;
using SimpleJSON;

namespace Assets.Scripts
{
    public static class LocalDatabase
    {
        public static JSONClass Data = new JSONClass
        {
            {
                "companies", new JSONClass
                {
                    { "Allianz",
                        new JSONClass { { "site", "http://www.allianz.ru/" }, { "icon", "Allianz" } } },
                    { "АМКОполис",
                        new JSONClass { { "site", "http://www.sk-amko.ru/" }, { "icon", "АМКОполис" } } },
                    { "Алроса",
                        new JSONClass { { "site", "http://www.ic-alrosa.ru/" }, { "icon", "Алроса" } } },
                    { "Альфа-Страхование",
                        new JSONClass { { "site", "http://www.alfastrah.ru/" }, { "icon", "Альфа-Страхование" }, { "regions", new JSONArray { "Москва", "Санкт-Петербург", "Архангельск", "Казань", "Екатеринбург", "Нижний Новгород", "Омск", "Новосибирск", "Хабаровск", "Иркутск", "Красноярск", "Томск", "Челябинск", "Воронеж", "Саратов", "Курск", "Смоленск", "Саратов", "Владивосток" } } } },
                    { "Британский Страховой Дом",
                        new JSONClass { { "site", "http://bihouse.ru/" }, { "icon", "Британский Страховой Дом" } } },
                    { "ВСК Страховой Дом",
                        new JSONClass { { "site", "http://www.vsk.ru/" }, { "icon", "ВСК Страховой Дом" } } },
                    { "ВТБ",
                        new JSONClass { { "site", "http://www.vtbins.ru/" }, { "icon", "ВТБ" } } },
                    { "Гайде",
                        new JSONClass { { "site", "http://www.guideh.com/" }, { "icon", "Гайде" } } },
                    { "Геополис",
                        new JSONClass { { "site", "http://www.geopolis.ru/" }, { "icon", "Геополис" }, { "regions", new JSONArray { "Москва" } } } },
                    { "Гефест",
                        new JSONClass { { "site", "http://www.gefest.ru/" }, { "icon", "Гефест" } } },
                    { "ЖАСО",
                        new JSONClass { { "site", "http://www.zhaso.ru/" }, { "icon", "ЖАСО" },  } },
                    { "Инвест-Альянс",
                        new JSONClass { { "site", "http://ins-invest.ru/" }, { "icon", "Инвест-Альянс" }, } },
                    { "Ингосстрах",
                        new JSONClass { { "site", "http://www.ingos.ru/ru/" }, { "icon", "Ингосстрах" },  } },
                    { "Компаньон",
                        new JSONClass { { "site", "http://companion-group.ru/" }, { "icon", "Компаньон" },  } },
                    { "Купеческое",
                        new JSONClass { { "site", "http://www.kupecheskoe.ru/" }, { "icon", "Купеческое" },  } },
                    { "Либерти Страхование (бывший КИТ Финанс)",
                        new JSONClass { { "site", "http://www.liberty24.ru/" }, { "icon", "Либерти Страхование (бывший КИТ Финанс)" },  } },
                    { "МАКС",
                        new JSONClass { { "site", "http://www.makc.ru/" }, { "icon", "МАКС" }, { "regions", new JSONArray { "Москва" } } } },
                    { "НАСКО",
                        new JSONClass { { "site", "http://nasko.ru/" }, { "icon", "НАСКО" } } },
                    { "Объединённая Страховая Компания",
                        new JSONClass { { "site", "http://www.osk-ins.ru/" }, { "icon", "Объединённая Страховая Компания" } } },
                    { "Оранта Страхование",
                        new JSONClass { { "site", "http://www.oranta-sk.ru/" }, { "icon", "Оранта Страхование" } } },
                    { "РЕСО-Гарантия",
                        new JSONClass { { "site", "http://www.reso.ru/" }, { "icon", "РЕСО-Гарантия" } } },
                    { "Ренессанс Страхование",
                        new JSONClass { { "site", "http://www.renins.com/" }, { "icon", "Ренессанс Страхование" } } },
                    { "Росгосстрах",
                        new JSONClass { { "site", "http://www.rgs.ru/" }, { "icon", "Росгосстрах" } } },
                    { "СГ Московская Страховая Компания",
                        new JSONClass { { "site", "http://sgmsk.ru/" }, { "icon", "СГ Московская Страховая Компания" } } },
                    { "СОГАЗ",
                        new JSONClass { { "site", "http://www.sogaz.ru/" }, { "icon", "СОГАЗ" } } },
                    { "Северная Казна",
                        new JSONClass { { "site", "http://www.kazna.com/" }, { "icon", "Северная Казна" } } },
                    { "Согласие",
                        new JSONClass { { "site", "http://www.soglasie.ru/" }, { "icon", "Согласие" } } },
                    { "Сургутнефтегаз",
                        new JSONClass { { "site", "https://www.sngi.ru/" }, { "icon", "Сургутнефтегаз" }, { "regions", new JSONArray { "Москва", "Санкт-Петербург", "Екатеринбург", "Набережные Челны", "Кемерово" } } } },
                    { "УралСиб",
                        new JSONClass { { "site", "http://www.uralsibins.ru/" }, { "icon", "УралСиб" } } },
                    { "Цюрих",
                        new JSONClass { { "site", "http://www.zurich.ru/" }, { "icon", "Цюрих" } } },
                    { "Энергогарант",
                        new JSONClass { { "site", "http://www.energogarant.ru/" }, { "icon", "Энергогарант" } } },
                    { "Эрго Русь",
                        new JSONClass { { "site", "http://www.ergorussia.ru/" }, { "icon", "Эрго Русь" } } }
                }
            },
            {
                "skip", new JSONArray
                {
                    "Важно.Новое страхование"
                }
            },
            {
                "regions", new JSONClass
                {
                    new JSONArray { Regions.AnyRegion },
                    new JSONArray { "Москва", "Московская область" },
                    new JSONArray { "Питер", "Санкт-Петербург", "Ленинградская область", "СПб" },
                    new JSONArray { "Астрахань" },
                    new JSONArray { "Барнаул" },
                    new JSONArray { "Белгород" },
                    new JSONArray { "Брянск" },
                    new JSONArray { "Владивосток" },
                    new JSONArray { "Волгоград" },
                    new JSONArray { "Воронеж" },
                    new JSONArray { "Екатеринбург" },
                    new JSONArray { "Иваново" },
                    new JSONArray { "Ижевск" },
                    new JSONArray { "Иркутск" },
                    new JSONArray { "Казань" },
                    new JSONArray { "Калининград" },
                    new JSONArray { "Кемерово", "Кемеровский" },
                    new JSONArray { "Киров" },
                    new JSONArray { "Краснодар" },
                    new JSONArray { "Красноярск" },
                    new JSONArray { "Курск" },
                    new JSONArray { "Липецк" },
                    new JSONArray { "Магнитогорск" },
                    new JSONArray { "Махачкала" },
                    new JSONArray { "Н. Челны", "Набережные Челны" },
                    new JSONArray { "Н. Новгород", "Нижний Новгород" },
                    new JSONArray { "Нижний Тагил" },
                    new JSONArray { "Новокузнецк" },
                    new JSONArray { "Новосибирск" },
                    new JSONArray { "Омск" },
                    new JSONArray { "Оренбург" },
                    new JSONArray { "Пенза" },
                    new JSONArray { "Пермь" },
                    new JSONArray { "Ростов-на-Дону" },
                    new JSONArray { "Рязань" },
                    new JSONArray { "Самара" },
                    new JSONArray { "Саратов" },
                    new JSONArray { "Сочи" },
                    new JSONArray { "Ставрополь" },
                    new JSONArray { "Тверь" },
                    new JSONArray { "Тольятти" },
                    new JSONArray { "Томск" },
                    new JSONArray { "Тула" },
                    new JSONArray { "Тюмень" },
                    new JSONArray { "Улан-Удэ" },
                    new JSONArray { "Ульяновск" },
                    new JSONArray { "Уфа" },
                    new JSONArray { "Хабаровск" },
                    new JSONArray { "Чебоксары" },
                    new JSONArray { "Челябинск" },
                    new JSONArray { "Ярославль" }
                }
            }
        };
    }
}