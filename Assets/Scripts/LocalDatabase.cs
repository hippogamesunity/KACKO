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
                        new JSONClass { { "site", "http://www.allianz.ru/" }, { "icon", "Allianz" }, { "phone", new JSONClass { { "Москва", "+7 (495) 232-33-33" } } } } },
                    { "АМКОполис",
                        new JSONClass { { "site", "http://www.sk-amko.ru/" }, { "icon", "АМКОполис" }, { "phone", new JSONClass { { "Москва", "+7 (495) 545-39-11" } } } } },
                    { "Алроса",
                        new JSONClass { { "site", "http://www.ic-alrosa.ru/" }, { "icon", "Алроса" }, { "phone", new JSONClass { { "Москва", "+7 (495) 967-78-62" } } } } },
                    { "Альфа-Страхование",
                        new JSONClass { { "site", "http://www.alfastrah.ru/" }, { "icon", "Альфа-Страхование" }, { "phone", new JSONClass { { "Москва", "+7 (495) 788-09-99" } } }, { "regions", new JSONArray { "Москва", "Санкт-Петербург", "Архангельск", "Казань", "Екатеринбург", "Нижний Новгород", "Омск", "Новосибирск", "Хабаровск", "Иркутск", "Красноярск", "Томск", "Челябинск", "Воронеж", "Саратов", "Курск", "Смоленск", "Саратов", "Владивосток" } } } },
                    { "Британский Страховой Дом",
                        new JSONClass { { "site", "http://bihouse.ru/" }, { "icon", "Британский Страховой Дом" }, { "phone", new JSONClass { { "Москва", "+7 (495) 755-53-35" } } } } },
                    { "ВСК Страховой Дом",
                        new JSONClass { { "site", "http://www.vsk.ru/" }, { "icon", "ВСК Страховой Дом" }, { "phone", new JSONClass { { "Москва", "+7 (495) 784-77-00" } } } } },
                    { "ВТБ",
                        new JSONClass { { "site", "http://www.vtbins.ru/" }, { "icon", "ВТБ" }, { "phone", new JSONClass { { "Москва", "+7 (495) 644-44-40" } } } } },
                    { "Гайде",
                        new JSONClass { { "site", "http://www.guideh.com/" }, { "icon", "Гайде" }, { "phone", new JSONClass { { "Москва", "8 (800) 444-02-75" } } } } },
                    { "Геополис",
                        new JSONClass { { "site", "http://www.geopolis.ru/" }, { "icon", "Геополис" }, { "phone", new JSONClass { { "Москва", "+7 (495) 22З-ЗЗ-6З" } } }, { "regions", new JSONArray { "Москва" } } } },
                    { "Гефест",
                        new JSONClass { { "site", "http://www.gefest.ru/" }, { "icon", "Гефест" }, { "phone", new JSONClass { { "Москва", "7 (495) 777-11-87" } } } } },
                    { "ЖАСО",
                        new JSONClass { { "site", "http://www.zhaso.ru/" }, { "icon", "ЖАСО" }, { "phone", new JSONClass { { "Москва", "+7 (495) 663-03-30" } } } } },
                    { "Инвест-Альянс",
                        new JSONClass { { "site", "http://ins-invest.ru/" }, { "icon", "Инвест-Альянс" }, { "phone", new JSONClass { { "Москва", "+7 (495) 783-23-20" } } } } },
                    { "Ингосстрах",
                        new JSONClass { { "site", "http://www.ingos.ru/ru/" }, { "icon", "Ингосстрах" }, { "phone", new JSONClass { { "Москва", "+7 (495) 956-55-55" }  } } } },
                    { "Компаньон",
                        new JSONClass { { "site", "http://companion-group.ru/" }, { "icon", "Компаньон" }, { "phone", new JSONClass { { "Москва", "+7 (495) 797-94-28" } } } } },
                    { "Купеческое",
                        new JSONClass { { "site", "http://www.kupecheskoe.ru/" }, { "icon", "Купеческое" }, { "phone", new JSONClass { { "Москва", "+7 (495) 685-94-67" } } } } },
                    { "Либерти Страхование (бывший КИТ Финанс)",
                        new JSONClass { { "site", "http://www.liberty24.ru/" }, { "icon", "Либерти Страхование (бывший КИТ Финанс)" }, { "phone", new JSONClass { { "Москва", "8 (800) 100-2-100" } } } } },
                    { "МАКС",
                        new JSONClass { { "site", "http://www.makc.ru/" }, { "icon", "МАКС" }, { "phone", new JSONClass { { "Москва", "+7 (495) 730-11-01" } } }, { "regions", new JSONArray { "Москва" } } } },
                    { "НАСКО",
                        new JSONClass { { "site", "http://nasko.ru/" }, { "icon", "НАСКО" }, { "phone", new JSONClass { { "Москва", "8 (800) 500-16-16" } } } } },
                    { "Объединённая Страховая Компания",
                        new JSONClass { { "site", "http://www.osk-ins.ru/" }, { "icon", "Объединённая Страховая Компания" }, { "phone", new JSONClass { { "Москва", "+7 (495) 981-29-77" } } } } },
                    { "Оранта Страхование",
                        new JSONClass { { "site", "http://www.oranta-sk.ru/" }, { "icon", "Оранта Страхование" }, { "phone", new JSONClass { { "Москва", "8 (800) 200-85-65" } } } } },
                    { "РЕСО-Гарантия",
                        new JSONClass { { "site", "http://www.reso.ru/" }, { "icon", "РЕСО-Гарантия" }, { "phone", new JSONClass { { "Москва", "+7 (495) 730-30-00" } } } } },
                    { "Ренессанс Страхование",
                        new JSONClass { { "site", "http://www.renins.com/" }, { "icon", "Ренессанс Страхование" }, { "phone", new JSONClass { { "Москва", "8 (800) 333-8-800" } } } } },
                    { "Росгосстрах",
                        new JSONClass { { "site", "http://www.rgs.ru/" }, { "icon", "Росгосстрах" }, { "phone", new JSONClass { { "Москва", "8 (800) 200-0-900" } } } } },
                    { "СГ Московская Страховая Компания",
                        new JSONClass { { "site", "http://sgmsk.ru/" }, { "icon", "СГ Московская Страховая Компания" }, { "phone", new JSONClass { { "Москва", "+7 (495) 956 84 84" } } } } },
                    { "СОГАЗ",
                        new JSONClass { { "site", "http://www.sogaz.ru/" }, { "icon", "СОГАЗ" }, { "phone", new JSONClass { { "Москва", "8 (800) 333-08-88" } } } } },
                    { "Северная Казна",
                        new JSONClass { { "site", "http://www.kazna.com/" }, { "icon", "Северная Казна" }, { "phone", new JSONClass { { "Москва", "8 (800) 700-13-30" } } } } },
                    { "Согласие",
                        new JSONClass { { "site", "http://www.soglasie.ru/" }, { "icon", "Согласие" }, { "phone", new JSONClass { { "Москва", "8 (800) 200-01-01" } } } } },
                    { "Сургутнефтегаз",
                        new JSONClass { { "site", "https://www.sngi.ru/" }, { "icon", "Сургутнефтегаз" }, { "phone", new JSONClass { { "Москва", "8 (800) 444-40-01" } } }, { "regions", new JSONArray { "Москва", "Санкт-Петербург", "Екатеринбург", "Набережные Челны", "Кемерово" } } } },
                    { "УралСиб",
                        new JSONClass { { "site", "http://www.uralsibins.ru/" }, { "icon", "УралСиб" }, { "phone", new JSONClass { { "Москва", "8 (800) 250-92-02" } } } } },
                    { "Цюрих",
                        new JSONClass { { "site", "http://www.zurich.ru/" }, { "icon", "Цюрих" }, { "phone", new JSONClass { { "Москва", "8 (800) 700-77-07 " } } } } },
                    { "Энергогарант",
                        new JSONClass { { "site", "http://www.energogarant.ru/" }, { "icon", "Энергогарант" }, { "phone", new JSONClass { { "Москва", "+7 (495) 737-03-30" } } } } },
                    { "Эрго Русь",
                        new JSONClass { { "site", "http://www.ergorussia.ru/" }, { "icon", "Эрго Русь" }, { "phone", new JSONClass { { "Москва", "8 (800) 200 22 24" } } } } }
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