using UnityEngine;

namespace Assets.Scripts
{
  public class BackButton : Script
  {
    public GameObject Back;

    public void Awake()
    {
      Back.SetActive(false);
    }

    public void Enable(bool enable)
    {
      #if UNITY_IPHONE

      Back.SetActive(true);
      Back.collider2D.enabled = enable;
      TweenAlpha.Begin(Back, 0.25f, enable ? 180 / 255f : 0);

      #endif
    }
  }
}