using Assets.Scripts.Common;

namespace Assets.Scripts.Views
{
    public class Intro : ViewBase
    {
        public UILabel Text;
        public GameButton StartButton;

        protected override void Initialize()
        {
            GetComponent<BackButton>().Enable(false);
        }

        protected override void Cleanup()
        {
            GetComponent<BackButton>().Enable(true);
        }
    }
}