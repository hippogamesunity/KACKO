using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Update()
        {
            OsagoButton.Enabled = KaskoButton.Enabled = Profile.Make >= 0 && Profile.Model >= 0 && Profile.Year >= 0 && Profile.Price >= 0
                && Profile.Sex >= 0 && Profile.Age >= 0 && Profile.Exp >= 0;
        }

        public void UpdateForm()
        {
            var car = Profile.Cars.Childs.Single(i => int.Parse(i["id"].Value) == Profile.Make)["models"].Childs.Single(i => int.Parse(i["id"]) == Profile.Model);
            var make = Profile.Cars.Childs.Single(i => int.Parse(i["id"]) == Profile.Make)["name"].Value;
            var model = car["name"].Value;
            
            Car.SetText("{0} {1} ({2})", make, model, Profile.Year);
            Region.SetText(Profile.Region);
            Price.value = Convert.ToString(Profile.Price);
            Power.value = Convert.ToString(Profile.Power);
            Sex.SetText(Profile.Sex == 1 ? "М" : "Ж");
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
                    var power = Math.Max(999, int.Parse(Power.value));

                    power = Math.Min(60, power);

                    Profile.Power = power;
                    Profile.Save();
                    Price.value = Convert.ToString(power);
                })
            };

            Age.onSubmit = new List<EventDelegate>
            {
                new EventDelegate(() =>
                {
                    var age = Math.Max(16, int.Parse(Age.value));

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

                    Profile.Exp = exp;
                    Profile.Save();
                    Exp.value = Convert.ToString(exp);
                })
            };
        }
    }
}