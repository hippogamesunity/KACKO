using System.IO;
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

        public void SelectMake(int id)
        {
            Profile.Make = id;
            GetComponent<Models>().Open(TweenDirection.Right);
        }

        public void SelectModel(int model)
        {
            Profile.Model = model;
            GetComponent<Years>().Open(TweenDirection.Right);

            var models = Profile.Cars.Childs.Single(i => int.Parse(i["id"].Value) == Profile.Make)["models"];
            var car = models.Childs.Single(i => int.Parse(i["id"]) == Profile.Model);
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
            Profile.Sex = Profile.Sex == 1 ? 2 : 1;
            Profile.Save();
            GetComponent<Form>().Sex.text = Profile.Sex == 1 ? "М" : "Ж";
        }

        public void CalcOsago()
        {
            StartLoading("выполняется расчет ОСАГО...");

            var calc = JSON.Parse(File.ReadAllText(@"d:\kasko.json"));

            TaskScheduler.CreateTask(() =>
            {
                StopLoading(null);
                GetComponent<Results>().Initialize(Profile.Companies, calc);
                GetComponent<Results>().Open(TweenDirection.Right);
            }, 2);
        }

        public void CalcKasko()
        {
            StartLoading("выполняется расчет КАСКО...");

            var calc = JSON.Parse(File.ReadAllText(@"d:\kasko.json"));

            TaskScheduler.CreateTask(() =>
            {
                StopLoading(null);
                GetComponent<Results>().Initialize(Profile.Companies, calc);
                GetComponent<Results>().Open(TweenDirection.Right);
            }, 2);
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
    }
}