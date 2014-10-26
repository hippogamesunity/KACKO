using System;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Profile
    {
        public static JSONNode Makes;
        public static JSONNode Models;
        public static string Results;

        public static int Make;
        public static int Model;
        public static int Year;
        public static int Price;

        public static int Sex;
        public static int Age;
        public static int Exp;

        public static void Load()
        {
            Makes = JSON.Parse(Resources.Load<TextAsset>("makes").text);
            Models = JSON.Parse(Resources.Load<TextAsset>("models").text);
            Results = Resources.Load<TextAsset>("results").text;

            Make = 29;
            Model = 318;
            Year = DateTime.Now.Year;

            Sex = 1;
            Age = 30;
            Exp = 5;
        }
    }
}