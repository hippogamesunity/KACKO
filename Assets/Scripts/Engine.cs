using Assets.Scripts.Common;
using Assets.Scripts.Views;

namespace Assets.Scripts
{
    public class Engine : Script
    {
        public void Start()
        {
            Profile.Load();
            ViewBase.Current = GetComponent<Intro>();
        }

        public void Begin()
        {
            GetComponent<Form>().Open(TweenDirection.Right);
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
            GetComponent<Form>().Open(TweenDirection.Right);
            GetComponent<Form>().UpdateForm();
        }

        public void ChangeSex()
        {
            Profile.Sex = Profile.Sex == 1 ? 2 : 1;
            GetComponent<Form>().Sex.text = Profile.Sex == 1 ? "М" : "Ж";
        }

        public void CalcOsago()
        {
            GetComponent<Form>().Loading = true;
            TaskScheduler.CreateTask(() => { GetComponent<Form>().Loading = false; GetComponent<Results>().Open(TweenDirection.Right); }, 2);
        }

        public void CalcKasko()
        {
            GetComponent<Form>().Loading = true;
            TaskScheduler.CreateTask(() => { GetComponent<Form>().Loading = false; GetComponent<Results>().Open(TweenDirection.Right); }, 2);
        }
    }
}