﻿using System;
using System.Linq;
using Assets.Scripts.Common;
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
        public const string Franchise = "Franchise";
        public const string Kbm = "Kbm";
        public const string Sex = "Sex";
        public const string Age = "Age";
        public const string Exp = "Exp";
        public const string Donate = "Donate";

        public const string Timestamp = "Timestamp";
        public const string ApiKey = "ApiKey";
        public const string Companies = "Companies";
        public const string Cars = "Cars";

        public const string LastRequest = "LastRequest";
        public const string LastResultTimestamp = "LastResultTimestamp";
        public const string LastResult = "LastResult";
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
        public static int Franchise;
        public static float Kbm;

        public static string Sex;
        public static int Age;
        public static int Exp;
        public static int Donate;

        private const string Login = "mYhx2YytWYulmY392ZtFXZAN2Zh1Wauw2Yt9";
        private const string PasswordHash = "TZ3YWN3IDNhNGZlRjY1ETY3YmNwEzNxY2Y0IzM3Q2N=Y";

        public static void Load()
        {
            LoadFormData();
            LoadUserData();
            Verify();
        }

        public static bool NeedSync
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

        public static void Sync()
        {
            var json = JSON.Parse(CalcApi.GetApiKey(B64R.Decode(Login), B64R.Decode(PasswordHash)));
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
        }

        public static void Save()
        {
            PlayerPrefs.SetString(Keys.Make, Make);
            PlayerPrefs.SetString(Keys.Model, Model);
            PlayerPrefs.SetInt(Keys.Year, Year);
            PlayerPrefs.SetInt(Keys.Price, Price);
            PlayerPrefs.SetInt(Keys.Power, Power);
            PlayerPrefs.SetString(Keys.Region, Region);
            PlayerPrefs.SetInt(Keys.Franchise, Franchise);
            PlayerPrefs.SetFloat(Keys.Kbm, Kbm);
            PlayerPrefs.SetString(Keys.Sex, Sex);
            PlayerPrefs.SetInt(Keys.Age, Age);
            PlayerPrefs.SetInt(Keys.Exp, Exp);
			PlayerPrefs.SetInt(Keys.Donate, Donate);
            PlayerPrefs.Save();
        }

        public static void SaveResult(string request, JSONNode result)
        {
            PlayerPrefs.SetString(Keys.LastRequest, request);
            PlayerPrefs.SetString(Keys.LastResultTimestamp, Convert.ToString(DateTime.UtcNow));
            PlayerPrefs.SetString(Keys.LastResult, result.SaveToCompressedBase64());
            PlayerPrefs.Save();
        }

        public static string GetResult(string request)
        {
            if (PlayerPrefs.GetString(Keys.LastRequest) == request && PlayerPrefs.HasKey(Keys.LastResult)
                && (DateTime.UtcNow - DateTime.Parse(PlayerPrefs.GetString(Keys.LastResultTimestamp))).TotalHours < 24)
            {
                try
                {
                    return JSONNode.LoadFromCompressedBase64(PlayerPrefs.GetString(Keys.LastResult));
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }

            return null;
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
                : 618000;

            Power = PlayerPrefs.HasKey(Keys.Power)
                ? PlayerPrefs.GetInt(Keys.Power)
                : 125;

            Region = PlayerPrefs.HasKey(Keys.Region)
                ? PlayerPrefs.GetString(Keys.Region)
                : Views.Region.Default;

            Franchise = PlayerPrefs.HasKey(Keys.Franchise)
                ? PlayerPrefs.GetInt(Keys.Franchise)
                : 0;

            Kbm = PlayerPrefs.HasKey(Keys.Kbm)
                ? PlayerPrefs.GetFloat(Keys.Kbm)
                : 1;

            Sex = PlayerPrefs.HasKey(Keys.Sex)
                ? PlayerPrefs.GetString(Keys.Sex)
                : "m";

            Age = PlayerPrefs.HasKey(Keys.Age)
                ? PlayerPrefs.GetInt(Keys.Age)
                : 30;

            Exp = PlayerPrefs.HasKey(Keys.Exp)
                ? PlayerPrefs.GetInt(Keys.Exp)
                : 5;

            Donate = PlayerPrefs.HasKey(Keys.Donate)
                ? PlayerPrefs.GetInt(Keys.Donate)
                : 0;
        }

        private static void Verify()
        {
            var make = GetMakeId(Make);
            var model = GetModelId(Model);

            if (Cars != null && (make == null || model == null))
            {
                if (make == null)
                {
                    Debug.Log("Unknown make: " + Make);
                }

                if (model == null)
                {
                    Debug.Log("Unknown model: " + Model);
                }

                Make = null;
                Model = null;
                Save();
            }

            if (!LocalDatabase.Data["regions"].Childs.Select(i => i.Value).Contains(Region))
            {
                Debug.Log("Unknown region: " + Region);

                Region = null;
                Save();
            }
        }

        private static string GetMakeId(string make)
        {
            try
            {
                return Cars.Childs.Single(i => i["name"].Value == make)["id"].Value;
            }
            catch
            {
                return null;
            }
        }

        private static string GetModelId(string model)
        {
            try
            {
                return Cars.Childs.Single(i => i["name"].Value == Make)["models"].Childs.Single(i => i["name"].Value == model).Value;
            }
            catch
            {
                return null;
            }
        }
    }
}