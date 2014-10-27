using System;
using Assets.Scripts.Common;
using Assets.Scripts.Views;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Profile
    {
        public static JSONNode Companies;
        public static JSONNode Cars;

        public static int Make;
        public static int Model;
        public static int Year;
        public static int Price;
        public static int Power;
        public static string Region;

        public static int Sex;
        public static int Age;
        public static int Exp;

        private const string TimestampKey = "Timestamp";
        private const string CompaniesKey = "Companies";
        private const string CarsKey = "Cars";
        private const string MakeKey = "Make";
        private const string ModelKey = "Model";
        private const string YearKey = "Year";
        private const string PriceKey = "Price";
        private const string PowerKey = "Power";
        private const string RegionKey = "Region";
        private const string SexKey = "Sex";
        private const string AgeKey = "Age";
        private const string ExpKey = "Exp";

        public static void Load()
        {
            if (NeedUpdate)
            {
                Update();
            }
            else
            {
                LoadFormData();
            }

            LoadUserData();
        }

        public static void Save()
        {
            PlayerPrefs.SetInt(MakeKey, Make);
            PlayerPrefs.SetInt(ModelKey, Model);
            PlayerPrefs.SetInt(YearKey, Year);
            PlayerPrefs.SetInt(PriceKey, Price);
            PlayerPrefs.SetInt(PowerKey, Power);
            PlayerPrefs.SetString(RegionKey, Region);
            PlayerPrefs.SetInt(SexKey, Sex);
            PlayerPrefs.SetInt(AgeKey, Age);
            PlayerPrefs.SetInt(ExpKey, Exp);
        }

        private static bool NeedUpdate
        {
            get
            {
                if (PlayerPrefs.HasKey(TimestampKey))
                {
                    return (DateTime.UtcNow - DateTime.Parse(PlayerPrefs.GetString(TimestampKey))).TotalHours > 0;
                }

                return true;
            }
        }

        private static void Update()
        {
            StartLoading("обновление данных...");
            TaskScheduler.CreateTask(ScheduledUpdate1, 1);
        }

        private static void ScheduledUpdate1()
        {
            try
            {
                var json = JSON.Parse(CalcApi.GetApiKey("blackrainbowgames@gmail.com", "qwertz1234"));
                var error = json["error"];

                if (error != null)
                {
                    throw new Exception(error["message"]);
                }

                var key = json["api_key"];

                Cars = JSON.Parse(CalcApi.GetCars(key));
                Companies = JSON.Parse(CalcApi.GetCompanies(key));

                PlayerPrefs.SetString(CompaniesKey, Companies.ToString());
                PlayerPrefs.SetString(CarsKey, Cars.ToString());
                PlayerPrefs.SetString(TimestampKey, Convert.ToString(DateTime.UtcNow));
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
            Companies = JSON.Parse(PlayerPrefs.GetString(CompaniesKey));
            Cars = JSON.Parse(PlayerPrefs.GetString(CarsKey));
        }

        private static void LoadUserData()
        {
            Make = PlayerPrefs.HasKey(MakeKey)
                ? PlayerPrefs.GetInt(MakeKey)
                : 29;

            Model = PlayerPrefs.HasKey(ModelKey)
                ? PlayerPrefs.GetInt(ModelKey)
                : 318;

            Year = PlayerPrefs.HasKey(YearKey)
                ? PlayerPrefs.GetInt(YearKey)
                : DateTime.Now.Year;

            Price = PlayerPrefs.HasKey(PriceKey)
                ? PlayerPrefs.GetInt(PriceKey)
                : 1000000;

            Power = PlayerPrefs.HasKey(PowerKey)
                ? PlayerPrefs.GetInt(PowerKey)
                : 1000000;

            Region = PlayerPrefs.HasKey(RegionKey)
                ? PlayerPrefs.GetString(RegionKey)
                : Regions.AnyRegion;

            Sex = PlayerPrefs.HasKey(SexKey)
                ? PlayerPrefs.GetInt(SexKey)
                : 1;

            Age = PlayerPrefs.HasKey(AgeKey)
                ? PlayerPrefs.GetInt(AgeKey)
                : 30;

            Exp = PlayerPrefs.HasKey(ExpKey)
                ? PlayerPrefs.GetInt(ExpKey)
                : 5;
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