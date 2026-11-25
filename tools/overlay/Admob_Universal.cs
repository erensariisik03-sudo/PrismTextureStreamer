using System;
using UnityEngine;

// Reklamsiz stub: AdMob SDK'ya hicbir bagimlilik yok.
public class Admob_Universal : MonoBehaviour
{
	public bool intersitialLoaded;
	public bool debug;
	public bool admobFromPauseFlag;
	public bool handleOnAdBannerFailedToLoadFlag;
	public bool handleOnAdBannerLoadedFlag;
	public bool handeOnAdInterstitialClosedFlag;

	public void Init() { }
	public void RequestBanner() { }
	public void RequestInterstitial() { }
	public bool IsAdmobAdLoaded() { return false; }
	public void ShowIntersitialAd() { }
	public void ShowBanner() { }
	public void HideBanner() { }
	public void DisableAds() { }
}
