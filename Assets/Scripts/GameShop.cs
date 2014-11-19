using System.Collections.Generic;
using Assets.Scripts.Common;
using OnePF;

namespace Assets.Scripts
{
  public class GameShop : Script
  {
    public UISprite DonateIcon;
    private const string SkuDonate = "kaskoclub_donate";
    private OpenIABClient _openIabClient;

    public void Start()
    {
      var options = new Options
      {
        checkInventoryTimeoutMs = Options.INVENTORY_CHECK_TIMEOUT_MS * 2,
        discoveryTimeoutMs = Options.DISCOVER_TIMEOUT_MS * 2,
        checkInventory = false,
        verifyMode = OptionsVerifyMode.VERIFY_SKIP,
        prefferedStoreNames = new[] { OpenIAB_Android.STORE_GOOGLE },
        storeKeys = new Dictionary<string, string>
        {
          { PlanformDependedSettings.StoreName, PlanformDependedSettings.StorePublicKey }
        }
      };

      _openIabClient = new OpenIABClient(options);
      _openIabClient.PurchaseSucceeded += PurchaseSucceeded;
      _openIabClient.ConsumePurchaseSucceeded += Consumed;

      _openIabClient.MapSku(PlanformDependedSettings.StoreName, new Dictionary<string, string>
      {
        { SkuDonate, SkuDonate }
      });
    }

    public void Refresh()
    {
      DonateIcon.fillAmount = Profile.Donate / 5f;
    }

    public void Donate()
    {
      #if UNITY_EDITOR

      DonateSucceeded();

      #else

      _openIabClient.PurchaseProduct(SkuDonate);

      #endif
    }

    private void PurchaseSucceeded(Purchase purchase)
    {
      if (purchase.Sku == SkuDonate)
      {
        _openIabClient.ConsumeProduct(purchase);
      }
    }

    private void Consumed(Purchase purchase)
    {
      if (purchase.Sku == SkuDonate)
      {
        DonateSucceeded();
      }
    }

    private void DonateSucceeded()
    {
      Profile.Donate++;
      Profile.Save();
      Refresh();
    }
  }
}