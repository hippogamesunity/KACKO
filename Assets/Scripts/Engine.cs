using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Views;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts
{
    public partial class Engine : Script
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

        private void StartLoading(string message)
        {
            LogFormat(message);
            Status.SetText(message);
            Loading = true;
        }

        private void StopLoading(string message)
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
                { "region", new JSONData(Profile.Region) },
                { "franchise", new JSONData(Profile.Franchise) }
            }.ToString();

            Debug.Log("Request json: " + request);

            return request;
        }

        private static JSONNode FindJsonModel(JSONNode jsonMake, string model)
        {
            var jsonModel = jsonMake["models"].Childs.FirstOrDefault(i => i["name"].Value.Equals(model, StringComparison.InvariantCultureIgnoreCase)) ??
                            jsonMake["models"].Childs.FirstOrDefault(i => i["name"].Value.IndexOf(model, StringComparison.InvariantCultureIgnoreCase) > -1);

            if (jsonModel == null)
            {
                foreach (var mod in jsonMake["models"].Childs)
                {
                    var engines = new JSONArray();

                    foreach (var generation in mod["generations"].Childs)
                    {
                        foreach (var engine in generation["engines"].Childs)
                        {
                            var joined = string.Format("{0} {1}", mod["name"].Value, engine["name"].Value);

                            if (joined.IndexOf(model, StringComparison.InvariantCultureIgnoreCase) > -1)
                            {
                                engines.Add(engine);
                            }
                        }
                    }

                    if (engines.Count > 0)
                    {
                        return new JSONClass
                        {
                            { "name", model },
                            { "generations", new JSONArray
                                {
                                    new JSONClass
                                    {
                                        { "name", model },
                                        { "engines", engines }
                                    }
                                }
                            }
                        };
                    }
                }
            }

            if (jsonModel == null && model.Contains(" "))
            {
                model = model.Split(Convert.ToChar(" "))[0];

                foreach (var mod in jsonMake["models"].Childs)
                {
                    if (mod["name"].Value == model)
                    {
                        return mod;
                    }
                }
            }

            return jsonModel;
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

                    calc = JSON.Parse(result);

                    Profile.SaveResult(request, result);
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
            var replaces = new Dictionary<string, string>
            {
                { "NameResolutionFailure", "не удалось установить соединение с сервером" },
                { "The request timed out", "превышен интервал ожидания для запроса" },
                { "ReadDone2", "не удалось установить соединение с сервером" },
            };

            foreach (var replace in replaces)
            {
                if (message.Contains(replace.Key))
                {
                    message = replace.Value;
                }
            }

            StopLoading(string.Format(errorPattern, message));
        }

        private void OpenResults()
        {
            StopLoading(null);
            GetComponent<Results>().Open(TweenDirection.Right);
        }
    }
}