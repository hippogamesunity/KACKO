using System;
using System.IO;
using Assets.Scripts.Common;
using Assets.Scripts.Views;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Keys
    {
        public const string Make = "Make";
        public const string Model = "Model";
        public const string Year = "Year";
        public const string Price = "Price";
        public const string Power = "Power";
        public const string Region = "Region";
        public const string Sex = "Sex";
        public const string Age = "Age";
        public const string Exp = "Exp";

        public const string Timestamp = "Timestamp";
        public const string ApiKey = "ApiKey";
        public const string Companies = "Companies";
        public const string Cars = "Cars";

        public const string LastRequest = "LastResultUrl";
        public const string LastResultTimestamp = "LastResultTimestamp";
    }

    public static class Profile
    {
        public static string ApiKey;
        public static JSONNode Companies;
        public static JSONNode Cars;

        public static string Make;
        public static string Model;
        public static int Year;
        public static int Price;
        public static int Power;
        public static string Region;

        public static string Sex;
        public static int Age;
        public static int Exp;

        private const string Login = "mYhx2YytWYulmY392ZtFXZAN2Zh1Wauw2Yt9";
        private const string Password = "Xcldnc6RTMzIAN==";

        public static void Load()
        {
            if (NeedUpdate)
            {
                Update();
            }
            else
            {
                LoadFormData();
                LoadUserData();
                Verify();
            }
        }

        public static void Save()
        {
            PlayerPrefs.SetString(Keys.Make, Make);
            PlayerPrefs.SetString(Keys.Model, Model);
            PlayerPrefs.SetInt(Keys.Year, Year);
            PlayerPrefs.SetInt(Keys.Price, Price);
            PlayerPrefs.SetInt(Keys.Power, Power);
            PlayerPrefs.SetString(Keys.Region, Region);
            PlayerPrefs.SetString(Keys.Sex, Sex);
            PlayerPrefs.SetInt(Keys.Age, Age);
            PlayerPrefs.SetInt(Keys.Exp, Exp);
        }

        public static void SaveResult(string request, string result)
        {
            File.WriteAllText(CachePath, result);

            PlayerPrefs.SetString(Keys.LastRequest, request);
            PlayerPrefs.SetString(Keys.LastResultTimestamp, Convert.ToString(DateTime.UtcNow));
            PlayerPrefs.Save();
        }

        public static string GetResult(string request)
        {
            Debug.Log(CachePath);
            if (PlayerPrefs.GetString(Keys.LastRequest) == request
                && (DateTime.UtcNow - DateTime.Parse(PlayerPrefs.GetString(Keys.LastResultTimestamp))).TotalHours < 24
                && File.Exists(CachePath))
            {
                return File.ReadAllText(CachePath);
            }

            return null;
        }

        private static string CachePath
        {
            get { return Path.Combine(Application.persistentDataPath, "cache"); }
        }

        private static bool NeedUpdate
        {
            get
            {
                if (PlayerPrefs.HasKey(Keys.Timestamp))
                {
                    return (DateTime.UtcNow - DateTime.Parse(PlayerPrefs.GetString(Keys.Timestamp))).TotalHours > 24;
                }

                return true;
            }
        }

        private static void Update()
        {
            StartLoading("обновление данных...");
            TaskScheduler.CreateTask(() =>
            {
                ScheduledUpdate();
                LoadUserData();
                Verify();
            }, 1);
        }

        private static void ScheduledUpdate()
        {
            try
            {
                var json = JSON.Parse(CalcApi.GetApiKey(B64R.Decode(Login), B64R.Decode(Password)));
                var error = json["error"];

                if (error != null)
                {
                    throw new Exception(error["message"]);
                }

                ApiKey = json["api_key"];
                Cars = JSON.Parse(CalcApi.GetCars(ApiKey));
                Companies = JSON.Parse(CalcApi.GetCompanies(ApiKey));

                PlayerPrefs.SetString(Keys.ApiKey, ApiKey);
                PlayerPrefs.SetString(Keys.Companies, Companies.ToString());
                PlayerPrefs.SetString(Keys.Cars, Cars.ToString());
                PlayerPrefs.SetString(Keys.Timestamp, Convert.ToString(DateTime.UtcNow));
                PlayerPrefs.Save();

                StopLoading("данные успешно обновлены");
            }
            catch (Exception e)
            {
                StopLoading(string.Format("Ошибка подключения к API: {0}", e.Message), error: true);
            }
        }

        private static void LoadFormData()
        {
            ApiKey = PlayerPrefs.GetString(Keys.ApiKey);
            Companies = JSON.Parse(PlayerPrefs.GetString(Keys.Companies));
            Cars = JSON.Parse(PlayerPrefs.GetString(Keys.Cars));
        }

        private static void LoadUserData()
        {
            Make = PlayerPrefs.HasKey(Keys.Make)
                ? PlayerPrefs.GetString(Keys.Make)
                : "Ford";

            Model = PlayerPrefs.HasKey(Keys.Model)
                ? PlayerPrefs.GetString(Keys.Model)
                : "Focus";

            Year = PlayerPrefs.HasKey(Keys.Year)
                ? PlayerPrefs.GetInt(Keys.Year)
                : DateTime.Now.Year;

            Price = PlayerPrefs.HasKey(Keys.Price)
                ? PlayerPrefs.GetInt(Keys.Price)
                : 1000000;

            Power = PlayerPrefs.HasKey(Keys.Power)
                ? PlayerPrefs.GetInt(Keys.Power)
                : 1000000;

            Region = PlayerPrefs.HasKey(Keys.Region)
                ? PlayerPrefs.GetString(Keys.Region)
                : Regions.AnyRegion;

            Sex = PlayerPrefs.HasKey(Keys.Sex)
                ? PlayerPrefs.GetString(Keys.Sex)
                : "m";

            Age = PlayerPrefs.HasKey(Keys.Age)
                ? PlayerPrefs.GetInt(Keys.Age)
                : 30;

            Exp = PlayerPrefs.HasKey(Keys.Exp)
                ? PlayerPrefs.GetInt(Keys.Exp)
                : 5;
        }

        private static void Verify()
        {
            if (Engine.GetMakeId(Make) == null || Engine.GetModelId(Model) == null)
            {
                Debug.Log(Region);
                Debug.Log(Make);
                Make = null;
                Model = null;
                Save();
            }

            if (!Regions.RegionList.Contains(Region))
            {
                Region = null;
                Save();
            }
        }

        private static void StartLoading(string message)
        {
            UnityEngine.Object.FindObjectOfType<Intro>().StartButton.Enabled = false;
            UnityEngine.Object.FindObjectOfType<Engine>().StartLoading(message);
        }

        private static void StopLoading(string message, bool error = false)
        {
            UnityEngine.Object.FindObjectOfType<Intro>().StartButton.Enabled = !error;
            UnityEngine.Object.FindObjectOfType<Engine>().StopLoading(message);
        }
    }
}