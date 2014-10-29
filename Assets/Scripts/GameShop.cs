namespace Assets.Scripts
{
    public class GameShop : Script
    {
        public UISprite Donate;

        public void Refresh()
        {
            Donate.fillAmount = Profile.Donate / 5f;
        }
    }
}