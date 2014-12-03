using System.Linq;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Additional : ViewBase
    {
        public UILabel Franchise;
        public UILabel Kbm;
        
        protected override void Initialize()
        {
            UpdateForm();
        }

        protected override void Cleanup()
        {
            GetComponent<Engine>().Status.SetText(null);
        }   


        public void UpdateForm()
        {
            Debug.Log("(" + Profile.Kbm + ")");
            var kbm = LocalDatabase.Data["kbm"].Childs.Single(i => i.Value.Contains("(" + Profile.Kbm + ")")).Value;

            Kbm.SetText(kbm);
            Franchise.SetText(Profile.Franchise);
        }
    }
}