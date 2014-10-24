using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Views;
using SimpleJSON;
using UnityEditor.VersionControl;
using UnityEngine;
using Message = Assets.Scripts.Views.Message;

namespace Assets.Scripts
{
    public class Engine : Script
    {
        public UILabel Car;
        public UIInput Price;
        public UIInput Power;
        public UILabel Sex;
        public UIInput Age;
        public UIInput Exp;
        public GameButton OsagoButton;
        public GameButton KaskoButton;
        public Transform[] ProgressBar;

        private bool _loading;

        public void Start()
        {
            Profile.Load();
            UpdateForm();
            ViewBase.Current = GetComponent<Message>();
            ValidateForm();
        }

        public void Update()
        {
            OsagoButton.Enabled = KaskoButton.Enabled = Profile.Make >= 0 && Profile.Model >= 0 && Profile.Year >= 0 && Profile.Price >= 0
                && Profile.Sex >= 0 && Profile.Age >= 0 && Profile.Exp >= 0;

            if (_loading)
            {
                foreach (var sprite in ProgressBar)
                {
                    sprite.transform.localPosition += new Vector3(2, 0);

                    if (sprite.transform.localPosition.x >= 1280)
                    {
                        sprite.transform.localPosition = new Vector3(-1280, sprite.transform.localPosition.y);
                    }
                }
            }
        }

        public void SelectCar()
        {
            FindObjectOfType<Makes>().Open(TweenDirection.Right);
        }

        public void SelectMake(int id)
        {
            Profile.Make = id;
            GetComponent<Models>().Open(TweenDirection.Right);
        }

        public void SelectModel(int model)
        {
            Profile.Model = model;
            GetComponent<Years>().Open(TweenDirection.Right);
        }

        public void SelectYear(int year)
        {
            Profile.Year = year;
            GetComponent<Message>().Text.SetText("Готово!\r\nТеперь проверьте стоимость автомобиля, данные водителя и нажмите на кнопку \"Рассчитать\"");
            GetComponent<Message>().Open(TweenDirection.Right);
            UpdateForm();
        }

        public void ChangeSex()
        {
            Profile.Sex = Profile.Sex == 1 ? 2 : 1;
            Sex.text = Profile.Sex == 1 ? "М" : "Ж";
        }

        public void CalcOsago()
        {
            _loading = true;
            TaskScheduler.CreateTask(() => { _loading = false; GetComponent<Results>().Open(TweenDirection.Right); }, 2);
        }

        public void CalcKasko()
        {
            _loading = true;
            TaskScheduler.CreateTask(() => { _loading = false; GetComponent<Results>().Open(TweenDirection.Right); }, 2);
        }

        private void UpdateForm()
        {
            var car = Profile.Models[Convert.ToString(Profile.Make)].Childs.Single(i => int.Parse(i["id"]) == Profile.Model);
            var model = car["text"].Value.Replace("\"", null);
            var price = int.Parse(car["price"]);
            var power = int.Parse(car["power"]);

            Car.SetText("{0} ({1})", model, Profile.Year);

            if (price > 0)
            {
                Price.value = Convert.ToString(price);
            }

            if (power > 0)
            {
                Power.value = Convert.ToString(power);
            }

            Age.value = Convert.ToString(Profile.Age);
            Exp.value = Convert.ToString(Profile.Exp);
        }

        private void ValidateForm()
        {
            Price.onSubmit = new List<EventDelegate>
            {
                new EventDelegate(() =>
                {
                    var price = Math.Max(100000, int.Parse(Price.value));

                    price = Math.Min(99999999, price);

                    Profile.Price = price;
                    Price.value = Convert.ToString(price);
                })
            };

            Price.onSubmit = new List<EventDelegate>
            {
                new EventDelegate(() =>
                {
                    var price = Math.Max(999, int.Parse(Price.value));

                    price = Math.Min(60, price);

                    Profile.Price = price;
                    Price.value = Convert.ToString(price);
                })
            };

            Age.onSubmit = new List<EventDelegate>
            {
                new EventDelegate(() =>
                {
                    var age = Math.Max(16, int.Parse(Age.value));

                    age = Math.Min(99, age);

                    Profile.Age = age;
                    Age.value = Convert.ToString(age);
                })
            };

            Exp.onSubmit = new List<EventDelegate>
            {
                new EventDelegate(() =>
                {
                    var exp = Math.Max(0, int.Parse(Exp.value));

                    exp = Math.Min(99, exp);

                    Profile.Exp = exp;
                    Exp.value = Convert.ToString(exp);
                })
            };
        }
    }

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