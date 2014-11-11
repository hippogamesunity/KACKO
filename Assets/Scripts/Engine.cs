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

        public void Awake()
        {
            Screen.sleepTimeout = 60; 
        }

        public void Start()
        {
            //PlayerPrefs.DeleteAll(); // TODO:
            Profile.Load();
            ViewBase.Current = GetComponent<Intro>();
            GetComponent<GameShop>().Refresh();
        }

        private DateTime _down;

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _down = DateTime.UtcNow;
            }
            else if (Input.GetMouseButton(0))
            {
                if (DateTime.UtcNow > _down.AddSeconds(5))
                {
                    GetComponent<DebugConsole>().enabled = !GetComponent<DebugConsole>().enabled;
                    _down = DateTime.UtcNow;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Back();
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

        public void Back()
        {
            if (FindObjectsOfType<TweenPosition>().All(i => !i.enabled))
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
                else if (ViewBase.Current is CompanyResult)
                {
                    GetComponent<Results>().Open(TweenDirection.Left);
                }
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

        private void ScheduledUpdate()
        {
            try
            {
                Profile.Sync();
                Profile.Load();
                StopLoading("данные успешно обновлены");
                TaskScheduler.CreateTask(Begin, 1);
            }
            catch (Exception e)
            {
                ShowException(e);
            }
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
            StartCalculate(osago: true);
        }

        public void CalcKasko()
        {
            StartCalculate(osago: false);
        }

        public void StartLoading(string message)
        {
            LogFormat(message);
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
            var request = new JSONClass
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
            }.ToString();

            Debug.Log("Request json: " + request);

            return request;
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

        public void OpenStore()
        {
            Debug.Log(PlanformDependedSettings.StoreLink);
            Application.OpenURL(PlanformDependedSettings.StoreLink);
        }

        private void StartCalculate(bool osago)
        {
            StartLoading(string.Format("выполняется расчет {0}...", osago ? "ОСАГО" : "КАСКО"));
            TaskScheduler.CreateTask(() => DelayedCalculate(osago), 1);
        }

        private void DelayedCalculate(bool osago)
        {
            var exception = TryCalculate(osago);

            if (exception == null)
            {
                return;
            }

            if (exception is ApiKeyException)
            {
                Profile.Sync();
                exception = TryCalculate(osago);

                if (exception != null)
                {
                    ShowException(exception);
                }
            }
            else
            {
                ShowException(exception);
            }
        }

        private Exception TryCalculate(bool osago)
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
                return e;
            }

            return null;
        }

        private void ShowException(Exception exception)
        {
            Debug.Log(exception);

            const string errorPattern = "ошибка подключения к API: {0}";
            var message = exception.Message;

            message = message.Replace("Error: NameResolutionFailure", "не удалось установить соединение с сервером");
            message = message.Replace("The request timed out", "превышен интервал ожидания для запроса");

            StopLoading(string.Format(errorPattern, message));
        }

        private void OpenResults()
        {
            StopLoading(null);
            GetComponent<Results>().Open(TweenDirection.Right);
        }
    }
}