using System;
using System.Collections.Generic;
using Assets.Scripts.Common;

namespace Assets.Scripts.Views
{
    public class Form : ViewBase
    {
        public UILabel Car;
        public UIInput Price;
        public UIInput Power;
        public UILabel Region;
        public UILabel Sex;
        public UIInput Age;
        public UIInput Exp;
        public GameButton OsagoButton;
        public GameButton KaskoButton;
        
        protected override void Initialize()
        {
            UpdateForm();
            ValidateForm();
        }

        protected override void Cleanup()
        {
            GetComponent<Engine>().Status.SetText(null);
        }   

        public void Update()
        {
            OsagoButton.Enabled = KaskoButton.Enabled =
                Profile.Make != null
                && Profile.Model != null
                && Profile.Year >= 0
                && Profile.Price >= 0
                && Profile.Power >= 0
                && Profile.Region != null
                && Profile.Sex != null
                && Profile.Age >= 0
                && Profile.Exp >= 0;
        }

        public void UpdateForm()
        {
            if (Profile.Make != null && Profile.Model != null)
            {
                Car.SetText("{0} {1} ({2})", Profile.Make, Profile.Model, Profile.Year);
            }
            else
            {
                Car.SetText(null);
            }

            Region.SetText(Profile.Region);
            Price.value = Convert.ToString(Profile.Price);
            Power.value = Convert.ToString(Profile.Power);
            Sex.SetText(Profile.Sex == "m" ? "М" : "Ж");
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
                    Profile.Save();
                    Price.value = Convert.ToString(price);
                })
            };

            Power.onSubmit = new List<EventDelegate>
            {
                new EventDelegate(() =>
                {
                    var power = Math.Max(60, int.Parse(Power.value));

                    power = Math.Min(999, power);

                    Profile.Power = power;
                    Profile.Save();
                    UnityEngine.Debug.Log(power);
                    Power.value = Convert.ToString(power);
                })
            };

            Age.onSubmit = new List<EventDelegate>
            {
                new EventDelegate(() =>
                {
                    var age = Math.Max(18, int.Parse(Age.value));

                    age = Math.Min(99, age);

                    Profile.Age = age;
                    Profile.Save();
                    Age.value = Convert.ToString(age);
                })
            };

            Exp.onSubmit = new List<EventDelegate>
            {
                new EventDelegate(() =>
                {
                    var exp = Math.Max(0, int.Parse(Exp.value));

                    exp = Math.Min(99, exp);

                    var fix = Profile.Age - exp - 16;

                    if (fix < 0)
                    {
                        exp += fix;
                    }

                    Profile.Exp = exp;
                    Profile.Save();
                    Exp.value = Convert.ToString(exp);
                })
            };
        }
    }
}