using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Form : ViewBase
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
        public bool Loading;

        protected override void Initialize()
        {
            UpdateForm();
            ValidateForm();
        }

        public void Update()
        {
            OsagoButton.Enabled = KaskoButton.Enabled = Profile.Make >= 0 && Profile.Model >= 0 && Profile.Year >= 0 && Profile.Price >= 0
                && Profile.Sex >= 0 && Profile.Age >= 0 && Profile.Exp >= 0;

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

        public void UpdateForm()
        {
            var car = Profile.Models[Convert.ToString(Profile.Make)].Childs.Single(i => int.Parse(i["id"]) == Profile.Model);
            var make = Profile.Makes.Childs.Single(i => int.Parse(i["id"]) == Profile.Make)["text"].Value.Replace("\"", null);
            var model = car["text"].Value.Replace("\"", null);
            var price = int.Parse(car["price"]);
            var power = int.Parse(car["power"]);

            Car.SetText("{0} {1} ({2})", make, model, Profile.Year);

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
}