using System;
using System.IO;
using SimpleJSON;

namespace Assets.Scripts
{
    public static class Profile
    {
        public static JSONNode Companies;
        public static JSONNode Cars;
        public static JSONNode Results;

        public static int Make;
        public static int Model;
        public static int Year;
        public static int Price;

        public static int Sex;
        public static int Age;
        public static int Exp;

        public static void Load()
        {
            Companies = JSON.Parse(File.ReadAllText(@"d:\companies.json"));
            Cars = JSON.Parse(File.ReadAllText(@"d:\cars.json"));
            Results = JSON.Parse(File.ReadAllText(@"d:\kasko.json"));

            Make = 29;
            Model = 318;
            Year = DateTime.Now.Year;

            Sex = 1;
            Age = 30;
            Exp = 5;
        }
    }
}