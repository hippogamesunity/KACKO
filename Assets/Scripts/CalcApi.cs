using System;
using System.IO;
using System.Net;
using System.Text;

namespace Assets.Scripts
{
    public static class CalcApi
    {
        private const string ApiUrl = "http://pkasko.ru";
        private const string UserAgent = "Mozilla/5.0 (compatible; PkaskoApiClient/1.0; +http://pkasko.ru)";
        
        public static string GetApiKey(string login, string password)
        {
            return GetResponse(null, string.Format("/auth/api?login={0}&password={1}", login, password));
        }

        public static string GetCars(string apiKey)
        {
            return GetResponse(apiKey, "/calcservice/cars");
        }

        public static string GetCompanies(string apiKey)
        {
            return GetResponse(apiKey, "/calcservice/companies");
        }


        private static string GetResponse(string apiKey, string path)
        {
            var url = string.Format("{0}{1}", ApiUrl, path);

            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Timeout = 10 * 1000;
            request.UserAgent = UserAgent;

            if (apiKey != null)
            {
                request.Headers.Add("X-Authorization", apiKey);
            }

            var response = (HttpWebResponse) request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Response status code: " + response.StatusCode);
            }

            using (var stream = response.GetResponseStream())
            {
                if (stream == null)
                {
                    throw new Exception("Пустой ответ от сервера");
                }

                var reader = new StreamReader(stream, Encoding.UTF8);
                var responseString = reader.ReadToEnd();

                if (responseString.Contains("<!DOCTYPE html>"))
                {
                    throw new Exception("Некорректный ключ API");
                }

                return responseString;
            }
        }
    }
}