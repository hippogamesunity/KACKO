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
        private const int Timeout = 60000;
        
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

        public static string Calc(string car, string apiKey)
        {
            return PostResponse(car, apiKey, "/kasko/calc?api=1");
        }

        public static string CalcOsago(string car, string apiKey)
        {
            return PostResponse(car, apiKey, "/kasko/options?code=OSAGO");
        }

        private static string PostResponse(string car, string apiKey, string path)
        {
            var url = string.Format("{0}{1}", ApiUrl, path);
            var request = (HttpWebRequest) WebRequest.Create(url);
            var bytes = Encoding.UTF8.GetBytes(car);

            request.UserAgent = UserAgent;
            request.Timeout = Timeout;
            request.Headers.Add("X-Authorization", apiKey);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            return ReadResponse(request);
        }

        private static string GetResponse(string apiKey, string path)
        {
            var url = string.Format("{0}{1}", ApiUrl, path);

            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Timeout = Timeout;
            request.UserAgent = UserAgent;

            if (apiKey != null)
            {
                request.Headers.Add("X-Authorization", apiKey);
            }

            return ReadResponse(request);
        }

        private static string ReadResponse(WebRequest request)
        {
            var response = (HttpWebResponse) request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("response status code: " + response.StatusCode);
            }

            using (var stream = response.GetResponseStream())
            {
                if (stream == null)
                {
                    throw new Exception("пустой ответ от сервера");
                }

                var reader = new StreamReader(stream, Encoding.UTF8);
                var responseString = reader.ReadToEnd();

                if (responseString.Contains("<!DOCTYPE html>"))
                {
                    throw new ApiKeyException("некорректный ключ API");
                }

                return responseString;
            }
        }
    }
}