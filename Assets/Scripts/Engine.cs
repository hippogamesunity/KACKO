using System;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Views;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts
{
    public class Engine : Script
    {
        public UILabel Status;
        public Transform[] ProgressBar;
        public bool Loading;

        public void Start()
        {
            Profile.Load();
            ViewBase.Current = GetComponent<Intro>();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && FindObjectsOfType<TweenPosition>().All(i => !i.enabled))
            {
                if (ViewBase.Current is Intro)
                {
                    Application.Quit();
                }
                else if (ViewBase.Current is Form)
                {
                    GetComponent<Intro>().Open(TweenDirection.Left);
                }
                else if (ViewBase.Current is Makes)
                {
                    GetComponent<Form>().Open(TweenDirection.Left);
                }
                else if (ViewBase.Current is Models)
                {
                    GetComponent<Makes>().Open(TweenDirection.Left);
                }
                else if (ViewBase.Current is Years)
                {
                    GetComponent<Models>().Open(TweenDirection.Left);
                }
                else if (ViewBase.Current is Regions)
                {
                    GetComponent<Form>().Open(TweenDirection.Left);
                }
                else if (ViewBase.Current is Results)
                {
                    GetComponent<Form>().Open(TweenDirection.Left);
                }
                else if (ViewBase.Current is Company)
                {
                    GetComponent<Results>().Open(TweenDirection.Left);
                }
            }

            if (Loading)
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

        public void Begin()
        {
            GetComponent<Form>().Open(TweenDirection.Right);
            Status.SetText(null);
        }

        public void SelectCar()
        {
            FindObjectOfType<Makes>().Open(TweenDirection.Right);
        }

        public void SelectMake(string make)
        {
            Profile.Make = make;
            GetComponent<Models>().Open(TweenDirection.Right);
        }

        public void SelectModel(string model)
        {
            Profile.Model = model;
            GetComponent<Years>().Open(TweenDirection.Right);

            var models = Profile.Cars.Childs.Single(i => i["name"].Value == Profile.Make)["models"];
            var car = models.Childs.Single(i => i["name"].Value == Profile.Model);
            var price = int.Parse(car["price"]);
            var power = int.Parse(car["power"]);

            if (price > 0)
            {
                Profile.Price = price;
            }

            if (power > 0)
            {
                Profile.Power = power;
            }
        }

        public void SelectYear(int year)
        {
            Profile.Year = year;
            Profile.Save();
            GetComponent<Form>().Open(TweenDirection.Right);
            GetComponent<Form>().UpdateForm();
        }

        public void SelectRegion()
        {
            GetComponent<Regions>().Open(TweenDirection.Right);
        }

        public void SelectRegion(string region)
        {
            Profile.Region = region;
            Profile.Save();
            GetComponent<Form>().Open(TweenDirection.Right);
            GetComponent<Form>().UpdateForm();
        }

        public void ChangeSex()
        {
            Profile.Sex = Profile.Sex == "m" ? "w" : "m";
            Profile.Save();
            GetComponent<Form>().Sex.text = Profile.Sex == "m" ? "М" : "Ж";
        }

        public void CalcOsago()
        {
            Calc(osago: true);
        }

        public void CalcKasko()
        {
            Calc(osago: false);
        }

        public void StartLoading(string message)
        {
            Debug.Log(message);

            Status.SetText(message);
            Loading = true;
        }

        public void StopLoading(string message)
        {
            Status.SetText(message);
            Loading = false;
        }

        private static string GetRequestJson()
        {
            var car = new JSONClass
            {
                { "make", new JSONData(Profile.Make) },
                { "model", new JSONData(Profile.Model) },
                { "year", new JSONData(Profile.Year) },
                { "power", new JSONData(Profile.Power) },
                { "price", new JSONData(Profile.Price) },
                { "drivers", new JSONArray
                    {
                        new JSONClass
                        {
                            { "age", new JSONData(Profile.Age) },
                            { "experience", new JSONData(Profile.Exp) },
                            { "sex", new JSONData(Profile.Sex) },
                            { "marriage", new JSONData(false) }
                        }
                    }
                },
            };

            return car.ToString();
        }

        public static string GetMakeId(string make)
        {
            try
            {
                return Profile.Cars.Childs.Single(i => i["name"].Value == Profile.Make)["id"].Value;
            }
            catch
            {
                return null;
            }
        }

        public static string GetModelId(string make)
        {
            try
            {
                return Profile.Cars.Childs.Single(i => i["name"].Value == Profile.Make)["models"].Childs
                    .Single(i => i["name"].Value == Profile.Model).Value;
            }
            catch
            {
                return null;
            }
        }

        private void Calc(bool osago)
        {
            StartLoading(string.Format("выполняется расчет {0}...", osago ? "ОСАГО" : "КАСКО"));
            TaskScheduler.CreateTask(() => DelayedCalc(osago), 2);
        }

        private void DelayedCalc(bool osago)
        {
            try
            {
                var request = GetRequestJson();
                var cache = Profile.GetResult(request);
                JSONNode calc;

                if (cache != null)
                {
                    calc = JSON.Parse(cache);
                }
                else
                {
                    var result = CalcApi.Calc(request, Profile.ApiKey);

                    Profile.SaveResult(request, result);

                    calc = JSON.Parse(result);
                }

                GetComponent<Results>().Initialize(Profile.Companies, calc, osago);

                TaskScheduler.CreateTask(OpenResults, cache == null ? 1 : 0);
            }
            catch (Exception e)
            {
                StopLoading(string.Format("Ошибка подключения к API: {0}", e.Message));
            }
        }

        private void OpenResults()
        {
            StopLoading(null);
            GetComponent<Results>().Open(TweenDirection.Right);
        }
    }
}