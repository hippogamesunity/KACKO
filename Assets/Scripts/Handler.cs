using System;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Views;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts
{
    public partial class Engine
    {
        public void Back()
        {
            if (FindObjectsOfType<TweenPosition>().Any(i => i.enabled)) return;

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
            else if (ViewBase.Current is Model)
            {
                GetComponent<Makes>().Open(TweenDirection.Left);
            }
            else if (ViewBase.Current is Generation)
            {
                GetComponent<Model>().Open(TweenDirection.Left);
            }
            else if (ViewBase.Current is Engines)
            {
                if (GetComponent<Generation>().JsonModel == null)
                {
                    GetComponent<Model>().Open(TweenDirection.Left);
                }
                else
                {
                    GetComponent<Generation>().Open(TweenDirection.Left);
                }
            }
            else if (ViewBase.Current is Year)
            {
                if (GetComponent<Year>().Bounds == null)
                {
                    GetComponent<Model>().Open(TweenDirection.Left);
                }
                else
                {
                    GetComponent<Engines>().Open(TweenDirection.Left);
                }
            }
            else if (ViewBase.Current is Region)
            {
                GetComponent<Form>().Open(TweenDirection.Left);
            }
            else if (ViewBase.Current is Franchise)
            {
                GetComponent<Form>().Open(TweenDirection.Left);
            }
            else if (ViewBase.Current is Results)
            {
                GetComponent<Form>().Open(TweenDirection.Left);
            }
            else if (ViewBase.Current is CompanyResult)
            {
                GetComponent<Results>().Open(TweenDirection.Left);
            }
            else if (ViewBase.Current is Reliability)
            {
                GetComponent<CompanyResult>().Open(TweenDirection.Left);
            }
            else if (ViewBase.Current is Options)
            {
                GetComponent<CompanyResult>().Open(TweenDirection.Left);
            }
            else if (ViewBase.Current is Additional)
            {
                GetComponent<Form>().Open(TweenDirection.Left);
            }
        }

        public void Begin()
        {
            if (Profile.NeedSync)
            {
                StartLoading("обновление данных...");
                TaskScheduler.CreateTask(ScheduledUpdate, 1);
            }
            else
            {
                GetComponent<Form>().Open(TweenDirection.Right);
                Status.SetText(null);
            }
        }

        public void SelectCar()
        {
            FindObjectOfType<Makes>().Open(TweenDirection.Right);
        }

        public void SelectMake(string make)
        {
            Profile.Make = make;
            GetComponent<Model>().Open(TweenDirection.Right);
        }

        public void SelectModel(string model)
        {
            Profile.Model = model;

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

            var database = Resources.Load<TextAsset>("Cars/" + Profile.Make);

            if (database == null)
            {
                GetComponent<Year>().Bounds = null;
                GetComponent<Year>().Open(TweenDirection.Right);
            }
            else
            {
                var jsonMake = JSONNode.LoadFromCompressedBase64(database.text);
                var jsonModel = FindJsonModel(jsonMake, model);

                if (jsonModel == null)
                {
                    GetComponent<Year>().Bounds = null;
                    GetComponent<Year>().Open(TweenDirection.Right);
                }
                else
                {
                    var generations = jsonModel["generations"];

                    if (generations.Count == 1)
                    {
                        GetComponent<Generation>().JsonModel = null;
                        GetComponent<Engines>().JsonEngines = generations[0]["engines"];
                        GetComponent<Engines>().Open(TweenDirection.Right);
                    }
                    else
                    {
                        GetComponent<Generation>().JsonModel = jsonModel;
                        GetComponent<Generation>().Open(TweenDirection.Right);
                    }
                }
            }
        }

        public void SelectGeneration(string generation)
        {
            var jsonEngines = GetComponent<Generation>().JsonModel["generations"].Childs
                .Single(i => i["name"].Value.Equals(generation))["engines"];

            GetComponent<Engines>().JsonEngines = jsonEngines;
            GetComponent<Engines>().Open(TweenDirection.Right);
        }

        public void SelectEngine(string engine, int power, int price, string production)
        {
            if (power > 0)
            {
                Profile.Power = power;
            }

            if (price > 0)
            {
                Profile.Price = price;
            }

            GetComponent<Year>().Bounds = production;
            GetComponent<Year>().Open(TweenDirection.Right);
        }

        public void SelectYear(int year)
        {
            Profile.Year = year;
            Profile.Save();
            GetComponent<Form>().Open(TweenDirection.Right);
        }

        public void SelectRegion()
        {
            GetComponent<Region>().Open(TweenDirection.Right);
        }

        public void SelectRegion(string region)
        {
            Profile.Region = region;
            Profile.Save();
            GetComponent<Form>().Open(TweenDirection.Left);
        }

        public void SelectFranchise()
        {
            GetComponent<Franchise>().Open(TweenDirection.Right);
        }

        public void SelectFranchise(int franchise)
        {
            Profile.Franchise = franchise;
            Profile.Save();
            GetComponent<Additional>().Open(TweenDirection.Left);
        }

        public void SelectKbm()
        {
            GetComponent<Kbm>().Open(TweenDirection.Right);
        }

        public void SelectKbm(float kbm)
        {
            Profile.Kbm = kbm;
            Profile.Save();
            GetComponent<Additional>().Open(TweenDirection.Left);
        }

        public void ChangeSex()
        {
            Profile.Sex = Profile.Sex == "m" ? "w" : "m";
            Profile.Save();
            GetComponent<Form>().Sex.text = Profile.Sex == "m" ? "М" : "Ж";
        }

        public void CalcOsago()
        {
            StartCalculate(osago: true);
        }

        public void CalcKasko()
        {
            StartCalculate(osago: false);
        }

        public void OpenStore()
        {
            Debug.Log(PlanformDependedSettings.StoreLink);
            Application.OpenURL(PlanformDependedSettings.StoreLink);
        }

        public void WriteEmail()
        {
            Application.OpenURL(Uri.EscapeUriString(string.Format("mailto:{0}?subject={1}&body={2}",
                "blackrainbowgames@gmail.com", "Предложение о сотрудничестве", "")));
        }

        public void OpenReliability()
        {
            GetComponent<Reliability>().Open(TweenDirection.Right);
        }

        public void OpenOptions()
        {
            GetComponent<Options>().Open(TweenDirection.Right);
        }

        public void OpenAdditional()
        {
            GetComponent<Additional>().Open(TweenDirection.Right);
        }
    }
}