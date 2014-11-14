using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using SimpleJSON;

namespace ConsoleApplication4
{
    class Program
    {
        static void Main()
        {
            const string DumpFolder = @"C:\Cars\Dump";
            const string CleanFolder = @"C:\Cars\Clean";

            foreach (var file in Directory.GetFiles(DumpFolder))
            {
                var json = File.ReadAllText(file);

                Clean(Path.GetFileNameWithoutExtension(file), ref json);

                var j = JSON.Parse(json);
                var b64 = j.SaveToCompressedBase64();

                File.WriteAllText(Path.Combine(CleanFolder, Path.GetFileName(file)), b64);
            }
            
            //var aaa = new JSONClass { { "name", "oleg" } };
            //var bb = aaa.Value;
            //var aa = KaskoApi.B64R.Encode("e675b743addeb15a676a0761cb43477f");
            
            //var key = KaskoApi.GetApiKey("blackrainbowgames@gmail.com", "e675b743addeb15a676a0761cb43477f");
            //var json = JSON.Parse(key);
            //var error = json["error"];

            //if (error == null)
            //{
            //    key = json["api_key"];
            //}
            //else
            //{
            //    throw new Exception(error["message"]);
            //}

            //var result = KaskoApi.GetKasko(key);
            //var companies = JSON.Parse(KaskoApi.GetCompanies(key));
            //var a = key;
        }

        private static void Clean(string make, ref string json)
        {
            var dict = new Dictionary<string, string>
            {
                { "Ресталинг", "(Restyling)" },
                { "Рестайлинг", "(Restyling)" },

                { "Седан-хардтоп", "Hardtop" },
                { "Седан", "Sedan" },
                { "Хардтоп", "Hardtop" },
                { "Хэтчбек", "Hatchback" },
                { "Лифтбек", "Liftback" },
                { "Фастбек", "Fastback" },
                { "Универсал", "Wagon" },
                { "Купе", "Coupe" },
                { "Кабриолет", "Cabriolet" },
                { "Родстер", "Roadster" },
                { "Спидстер", "Speedster" },
                { "Тарга", "Targa" },
                { "Лимузин", "Limousine" },
                { "Компактвэн", "Compact Van" },
                { "Фургон", "Van" },
                { "Внедорожник открытый", "SUV Open" },
                { "Внедорожник", "SUV" },
                { "Пикап Одинарная кабина", "Single Cab Pickup" },
                { "Пикап Полуторная кабина", "King Cab Pickup" },
                { "Пикап Двойная кабина", "Double Cab Pickup" },

                { "1 дв.", "3 door" },
                { "2 дв.", "3 door" },
                { "3 дв.", "3 door" },
                { "4 дв.", "3 door" },
                { "5 дв.", "5 door" },

                { "\"передний\"", "\"fwd\"" },
                { "\"задний\"", "\"rwd\"" },
                { "\"полный\"", "\"awd\"" },

                { "\"бензин\"", "\"petrol\"" },
                { "\"дизель\"", "\"diesel\"" },
                { "\"гибрид\"", "\"hybrid\"" },
                { "\"электро\"", "\"electro\"" },
                { "\"бензин / газ\"", "\"petrol / gas\"" },
                { "\"двухтактный\"", "\"duple\"" },

                { "н.в.", "" },
                { "кВт", "kW" },

                { " ", " " },
                { "–", "-" },
            };

            foreach (var key in dict.Keys)
            {
                json = Regex.Replace(json, key, dict[key], RegexOptions.IgnoreCase);
            }

            if (!new List<string> { "Hyundai", "Бронто", "ВАЗ", "ГАЗ", "ЗАЗ", "ЗИЛ", "ИЖ", "КамАЗ", "ЛУАЗ",
                "Москвич", "СеАЗ", "СМЗ", "ТагАЗ", "УАЗ", "Эксклюзив" }.Contains(make))
            {
                foreach (var c in new List<string> { "а", "б", "в", "г", "д", "е", "к", "л", "м", "н" })
                {
                    if (json.Contains(c)) throw new Exception(string.Format("{0}: {1}", c, json));
                }
            }
        }
    }

    public static class KaskoApi
    {
        private const string ApiUrl = "http://pkasko.ru";
        private const string UserAgent = "Mozilla/5.0 (compatible; PkaskoApiClient/1.0; +http://pkasko.ru)";

        public static string GetApiKey(string login, string password)
        {
            return GetResponse(null, string.Format("/auth/api?login={0}&passwordHash={1}", login, password));
        }

        public static string GetCars(string apiKey)
        {
            return GetResponse(apiKey, "/calcservice/cars");
        }

        public static string GetCompanies(string apiKey)
        {
            return GetResponse(apiKey, "/calcservice/companies");
        }

        public static string GetKasko(string apiKey)
        {
            var data = new JSONClass
            {
                { "make", new JSONData("Audi") },
                { "model", new JSONData("A4") },
                { "year", new JSONData("2014") },
                { "power", new JSONData("174") },
                { "price", new JSONData("1000000") },
                { "drivers", new JSONArray
                    {
                        new JSONClass
                        {
                            { "age", new JSONData(25) },
                            { "experience", new JSONData(5) },
                            { "sex", new JSONData("m") },
                            { "marriage", new JSONData(false) }
                        }
                    }
                },
            };

            var url = string.Format("{0}/kasko/calc?api=1", ApiUrl);
            var request = (HttpWebRequest) WebRequest.Create(url);
            var bytes = Encoding.UTF8.GetBytes(data.ToString());

            request.UserAgent = UserAgent;
            request.Timeout = 120 * 1000;
            request.Headers.Add("X-Authorization", apiKey);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;
            
            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(response.StatusCode.ToString());
            }

            using (var stream = response.GetResponseStream())
            {
                var reader = new StreamReader(stream, Encoding.UTF8);

                return reader.ReadToEnd();
            }
        }

        public static string GetOsago(string apiKey)
        {
            var data = new JSONClass
            {
                { "make", new JSONData("Audi") },
                { "model", new JSONData("A4") },
                { "year", new JSONData("2014") },
                { "power", new JSONData("174") },
                { "price", new JSONData("1000000") },
                { "drivers", new JSONArray
                    {
                        new JSONClass
                        {
                            { "age", new JSONData(25) },
                            { "experience", new JSONData(5) },
                            { "sex", new JSONData("m") },
                            { "marriage", new JSONData(false) }
                        }
                    }
                },
            };

            var url = string.Format("{0}/kasko/options?code=OSAGO", ApiUrl);
            var request = (HttpWebRequest)WebRequest.Create(url);
            var bytes = Encoding.UTF8.GetBytes(data.ToString());

            request.UserAgent = UserAgent;
            request.Timeout = 10 * 1000;
            request.Headers.Add("X-Authorization", apiKey);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(response.StatusCode.ToString());
            }

            using (var stream = response.GetResponseStream())
            {
                var reader = new StreamReader(stream, Encoding.UTF8);

                return reader.ReadToEnd();
            }
        }

        private static string GetResponse(string apiKey, string path)
        {
            var url = string.Format("{0}{1}", ApiUrl, path);
            var request = (HttpWebRequest) WebRequest.Create(url);

            request.UserAgent = UserAgent;
            request.Timeout = 10 * 1000;

            if (apiKey != null)
            {
                request.Headers.Add("X-Authorization", apiKey);
            }

            var response = (HttpWebResponse) request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(response.StatusCode.ToString());
            }

            using (var stream = response.GetResponseStream())
            {
                var reader = new StreamReader(stream, Encoding.UTF8);

                return reader.ReadToEnd();
            }
        }

        public static class Base64
        {
            public static string Encode(string plainText)
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                return Convert.ToBase64String(plainTextBytes);
            }

            public static string Decode(string base64EncodedData)
            {
                var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);

                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
        }

        public class B64R
        {
            public static string Encode(string value)
            {
                var base64 = Base64.Encode(value);
                var chars = base64.ToCharArray();

                Reverse(chars);

                return new string(chars);
            }

            public static string Decode(string value)
            {
                var chars = value.ToCharArray();

                Reverse(chars);

                return Base64.Decode(new string(chars));
            }

            private static void Reverse(IList<char> chars)
            {
                for (var i = 1; i < chars.Count; i += 2)
                {
                    var c = chars[i];

                    chars[i] = chars[i - 1];
                    chars[i - 1] = c;
                }
            }
        }
    }

    public static class LocalDatabase
    {
        public static JSONClass Data = new JSONClass
        {
            {
                "companies", new JSONClass
                {
                    { "Allianz", new JSONClass { { "site", "http://www.allianz.ru/" }, { "icon", "Allianz" } } },
                    { "АМКОполис", new JSONClass { { "site", "http://www.sk-amko.ru/" }, { "icon", "АМКОполис" } } },
                    { "Алроса", new JSONClass { { "site", "http://www.ic-alrosa.ru/" }, { "icon", "Алроса" } } },
                    { "Альфа-Страхование", new JSONClass { { "site", "http://www.alfastrah.ru/" }, { "icon", "Альфа-Страхование" }, { "regions", new JSONArray { "Москва", "Санкт-Петербург", "Архангельск", "Казань", "Екатеринбург", "Нижний Новгород", "Омск", "Новосибирск", "Хабаровск", "Иркутск", "Красноярск", "Томск", "Челябинск", "Воронеж", "Саратов", "Курск", "Смоленск", "Саратов", "Владивосток" } } } },
                    { "Британский Страховой Дом", new JSONClass { { "site", "http://bihouse.ru/" }, { "icon", "Британский Страховой Дом" } } },
                    { "ВСК Страховой Дом", new JSONClass { { "site", "http://www.vsk.ru/" }, { "icon", "ВСК Страховой Дом" } } },
                    { "ВТБ", new JSONClass { { "site", "http://www.vtbins.ru/" }, { "icon", "ВТБ" } } },
                    { "Гайде", new JSONClass { { "site", "http://www.guideh.com/" }, { "icon", "Гайде" } } },
                    { "Геополис", new JSONClass { { "site", "http://www.geopolis.ru/" }, { "icon", "Геополис" }, { "regions", new JSONArray { "Москва" } } } },
                    { "Гефест", new JSONClass { { "site", "http://www.gefest.ru/" }, { "icon", "Гефест" } } },
                    { "ЖАСО", new JSONClass { { "site", "http://www.zhaso.ru/" }, { "icon", "ЖАСО" },  } },
                    { "Инвест-Альянс", new JSONClass { { "site", "http://ins-invest.ru/" }, { "icon", "Инвест-Альянс" }, } },
                    { "Ингосстрах", new JSONClass { { "site", "http://www.ingos.ru/ru/" }, { "icon", "Ингосстрах" },  } },
                    { "Компаньон", new JSONClass { { "site", "http://companion-group.ru/" }, { "icon", "Компаньон" },  } },
                    { "Купеческое", new JSONClass { { "site", "http://www.kupecheskoe.ru/" }, { "icon", "Купеческое" },  } },
                    { "Либерти Страхование (бывший КИТ Финанс)", new JSONClass { { "site", "http://www.liberty24.ru/" }, { "icon", "Либерти Страхование (бывший КИТ Финанс)" },  } },
                    { "МАКС", new JSONClass { { "site", "http://www.makc.ru/" }, { "icon", "МАКС" }, { "regions", new JSONArray { "Москва" } } } },
                    { "НАСКО", new JSONClass { { "site", "http://nasko.ru/" }, { "icon", "НАСКО" } } },
                    { "Объединённая Страховая Компания", new JSONClass { { "site", "http://www.osk-ins.ru/" }, { "icon", "Объединённая Страховая Компания" } } },
                    { "Оранта Страхование", new JSONClass { { "site", "http://www.oranta-sk.ru/" }, { "icon", "Оранта Страхование" } } },
                    { "РЕСО-Гарантия", new JSONClass { { "site", "http://www.reso.ru/" }, { "icon", "РЕСО-Гарантия" } } },
                    { "Ренессанс Страхование", new JSONClass { { "site", "http://www.renins.com/" }, { "icon", "Ренессанс Страхование" } } },
                    { "Росгосстрах", new JSONClass { { "site", "http://www.rgs.ru/" }, { "icon", "Росгосстрах" } } },
                    { "СГ Московская Страховая Компания", new JSONClass { { "site", "http://sgmsk.ru/" }, { "icon", "СГ Московская Страховая Компания" } } },
                    { "СОГАЗ", new JSONClass { { "site", "http://www.sogaz.ru/" }, { "icon", "СОГАЗ" } } },
                    { "1Северная Казна", new JSONClass { { "site", "http://www.kazna.com/" }, { "icon", "Северная Казна" } } },
                    { "Согласие", new JSONClass { { "site", "http://www.soglasie.ru/" }, { "icon", "Согласие" } } },
                    { "Сургутнефтегаз", new JSONClass { { "site", "https://www.sngi.ru/" }, { "icon", "Сургутнефтегаз" }, { "regions", new JSONArray { "Москва", "Санкт-Петербург", "Екатеринбург", "Набережные Челны", "Кемерово" } } } },
                    { "УралСиб", new JSONClass { { "site", "http://www.uralsibins.ru/" }, { "icon", "УралСиб" } } },
                    { "Цюрих", new JSONClass { { "site", "http://www.zurich.ru/" }, { "icon", "Цюрих" } } },
                    { "Энергогарант", new JSONClass { { "site", "http://www.energogarant.ru/" }, { "icon", "Энергогарант" } } },
                    { "Эрго Русь", new JSONClass { { "site", "http://www.ergorussia.ru/" }, { "icon", "Эрго Русь" } } }
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
                    new JSONArray { "Any" },
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