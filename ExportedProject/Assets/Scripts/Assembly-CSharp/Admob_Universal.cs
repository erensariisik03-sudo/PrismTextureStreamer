using System;
using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine;

public class Admob_Universal : MonoBehaviour
{
	[HideInInspector]
	public bool intersitialLoaded;

	public static string ADMOB_BANNER_ID = "ca-app-pub-1450045239311598/1039312267";

	public static string ADMOB_INTERSTITIAL_ID = "ca-app-pub-1450045239311598/2516045464";

	public bool debug = true;

	private static bool _instanceFound;

	private bool initialized;

	public InterstitialAd interstitial;

	public BannerView bannerView;

	public bool admobFromPauseFlag;

	public bool handleOnAdBannerFailedToLoadFlag;

	public bool handleOnAdBannerLoadedFlag;

	public bool handeOnAdInterstitialClosedFlag;

	private bool bannerWasSuccesfullyLoaded;

	public void Init()
	{
		if (!MySingleton<Manager>.Instance.ads.disabledAds)
		{
			RequestInterstitial();
			RequestBanner();
			initialized = true;
		}
	}

	public void RequestBanner()
	{
		if (!MySingleton<Manager>.Instance.ads.disabledAds || !MySingleton<Manager>.Instance.remoteConfiguration.globalBanner)
		{
			bannerView = new BannerView(ADMOB_BANNER_ID, AdSize.SmartBanner, AdPosition.Bottom);
			AdRequest request = new AdRequest.Builder().Build();
			bannerView.LoadAd(request);
			bannerView.OnAdFailedToLoad += HandleOnAdBannerFailedToLoad;
			bannerView.OnAdLoaded += HandleOnAdBannerLoaded;
		}
	}

	public void RequestInterstitial()
	{
		if (!MySingleton<Manager>.Instance.ads.disabledAds)
		{
			interstitial = new InterstitialAd(ADMOB_INTERSTITIAL_ID);
			AdRequest request = new AdRequest.Builder().Build();
			interstitial.LoadAd(request);
			interstitial.OnAdFailedToLoad += HandleOnAdInterstitialFailedToLoad;
			interstitial.OnAdLoaded += HandleOnAdInterstitialLoaded;
			interstitial.OnAdOpening += HandleOnAdInterstitialShow;
			interstitial.OnAdClosed += HandeOnAdInterstitialClosed;
		}
	}

	public bool IsAdmobAdLoaded()
	{
		if (interstitial != null)
		{
			return interstitial.IsLoaded();
		}
		return false;
	}

	public void ShowIntersitialAd()
	{
		if (MySingleton<Manager>.Instance.ads.disabledAds)
		{
			return;
		}
		MySingleton<Manager>.Instance.admobFromPause = MySingleton<Manager>.Instance.adsFromPause;
		if (MySingleton<Manager>.Instance.admobInterstitialIsReady)
		{
			_ShowAdmobInterstitial();
		}
		else if (!SettingsManager.GetStarterPackBoughtOrEnded() && MySingleton<Manager>.Instance.adsFromPause)
		{
			MySingleton<Manager>.Instance.starterPackPanelIsActive = true;
			if ((bool)MySingleton<Manager>.Instance.guiGameLogic.pnStarterPack)
			{
				NGUITools.SetActive(MySingleton<Manager>.Instance.guiGameLogic.pnStarterPack.gameObject, true);
			}
			MySingleton<Manager>.Instance.guiGameLogic.backButton.function = GUIGameButton.GameButtonFunction.CloseStartingPackPopup;
			MySingleton<Manager>.Instance.starterPopUpAlreadySeen = true;
		}
		RequestInterstitial();
	}

	private void _ShowAdmobInterstitial()
	{
		if (interstitial != null && interstitial.IsLoaded())
		{
			if (MySingleton<Manager>.Instance.admobFromPause && (bool)MySingleton<Manager>.Instance.guiGameLogic && (bool)MySingleton<Manager>.Instance.guiGameLogic.pnPause)
			{
				NGUITools.SetActive(MySingleton<Manager>.Instance.guiGameLogic.pnPause.gameObject, false);
				StartCoroutine(SecurityPnPauseEnabled());
			}
			if (MySingleton<Manager>.Instance.admobFromMenu && (bool)MySingleton<Manager>.Instance.guiMenuLogic && (bool)MySingleton<Manager>.Instance.guiMenuLogic.pnSelectCar)
			{
				NGUITools.SetActive(MySingleton<Manager>.Instance.guiMenuLogic.pnSelectCar.gameObject, false);
				StartCoroutine(SecurityPnGarajeEnabled());
			}
			interstitial.Show();
		}
	}

	private IEnumerator SecurityPnPauseEnabled()
	{
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(2f));
		if (MySingleton<Manager>.Instance.paused)
		{
			NGUITools.SetActive(MySingleton<Manager>.Instance.guiGameLogic.pnPause.gameObject, true);
		}
	}

	private IEnumerator SecurityPnGarajeEnabled()
	{
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(1f));
		NGUITools.SetActive(MySingleton<Manager>.Instance.guiMenuLogic.pnSelectCar.gameObject, true);
	}

	public void ShowBanner()
	{
		if (!MySingleton<Manager>.Instance.ads.disabledAds && MySingleton<Manager>.Instance.remoteConfiguration.globalBanner)
		{
			StartCoroutine(ShowBannerCoroutine());
		}
	}

	private IEnumerator ShowBannerCoroutine()
	{
		yield return CoroutineUtil.WaitForRealSeconds(0f);
		if (!MySingleton<Manager>.Instance.ads.disabledAds)
		{
			if (bannerView != null)
			{
				bannerView.Show();
			}
		}
		else if (bannerView != null)
		{
			bannerView.Destroy();
		}
	}

	public void HideBanner()
	{
		if (bannerView != null)
		{
			bannerView.Hide();
		}
	}

	public void DisableAds()
	{
		if (bannerView != null)
		{
			HideBanner();
			bannerView.Destroy();
		}
		if (interstitial != null)
		{
			interstitial.Destroy();
		}
	}

	public void HandleOnAdBannerFailedToLoad(object sender, EventArgs args)
	{
		handleOnAdBannerFailedToLoadFlag = true;
	}

	public void HandleOnAdBannerLoaded(object sender, EventArgs args)
	{
		handleOnAdBannerLoadedFlag = true;
	}

	public void HandleOnAdInterstitialFailedToLoad(object sender, EventArgs args)
	{
	}

	public void HandleOnAdInterstitialLoaded(object sender, EventArgs args)
	{
	}

	public void HandleOnAdInterstitialShow(object sender, EventArgs args)
	{
	}

	public void HandeOnAdInterstitialClosed(object sender, EventArgs args)
	{
		handeOnAdInterstitialClosedFlag = true;
	}

	private void Update()
	{
		if (handeOnAdInterstitialClosedFlag)
		{
			handeOnAdInterstitialClosedFlag = false;
			AdmobClosedGuiLogic();
		}
		if (handleOnAdBannerFailedToLoadFlag && !bannerWasSuccesfullyLoaded)
		{
			handleOnAdBannerFailedToLoadFlag = false;
			CancelInvoke("RequestBanner");
			Invoke("RequestBanner", 5f);
		}
		if (handleOnAdBannerLoadedFlag)
		{
			CancelInvoke("RequestBanner");
			handleOnAdBannerLoadedFlag = false;
			bannerWasSuccesfullyLoaded = true;
		}
	}

	private void AdmobClosedGuiLogic()
	{
		if (MySingleton<Manager>.Instance.admobFromPause)
		{
			if ((bool)MySingleton<Manager>.Instance.guiGameLogic && (bool)MySingleton<Manager>.Instance.guiGameLogic.pnPause)
			{
				NGUITools.SetActive(MySingleton<Manager>.Instance.guiGameLogic.pnPause.gameObject, true);
			}
			MySingleton<Manager>.Instance.admobFromPause = false;
		}
		if (MySingleton<Manager>.Instance.admobFromMenu)
		{
			if ((bool)MySingleton<Manager>.Instance.guiMenuLogic && (bool)MySingleton<Manager>.Instance.guiMenuLogic.pnSelectCar)
			{
				NGUITools.SetActive(MySingleton<Manager>.Instance.guiMenuLogic.pnSelectCar.gameObject, true);
			}
			MySingleton<Manager>.Instance.admobFromMenu = false;
		}
		MySingleton<Manager>.Instance.AdsEnded();
	}

	private void RetryLoadBanner()
	{
		if (!bannerWasSuccesfullyLoaded)
		{
			CancelInvoke("RequestBanner");
			Invoke("RequestBanner", 5f);
		}
	}
}
