using UnityEngine;

public class AmazonAdsSource : MonoBehaviour
{
	public AmazonAds amazonAds;

	public void Init()
	{
		AmazonAds.current.testMode = Debug.isDebugBuild;
		AmazonAds.current.Init();
		AmazonAds.current.RequestInterstitial();
	}

	public bool CanShowAd()
	{
		return AmazonAds.current.interstitialLoaded;
	}

	public bool ShowInterstitial()
	{
		bool interstitialLoaded = AmazonAds.current.interstitialLoaded;
		if (interstitialLoaded)
		{
			AmazonAds.current.ShowInterstitial();
		}
		AmazonAds.current.RequestInterstitial();
		return interstitialLoaded;
	}

	private void OnAmazonInterstitialLoaded()
	{
		if (Debug.isDebugBuild)
		{
			Debug.Log("AmazonAds: Interstitial Loaded");
		}
		AmazonAds.current.interstitialLoaded = true;
	}

	private void OnAmazonInterstitialFailed()
	{
	}

	private void OnAmazonInterstitialDismissed()
	{
		if (Debug.isDebugBuild)
		{
			Debug.Log("AmazonAds: Interstitial Dismissed");
		}
		MySingleton<Manager>.Instance.AdsEnded();
	}

	public void OnAdEvent(string strMessage)
	{
		if (strMessage == "amazon-interstitial-yes")
		{
			OnAmazonInterstitialLoaded();
		}
		if (strMessage == "amazon-interstitial-no")
		{
			OnAmazonInterstitialFailed();
		}
		if (strMessage == "amazon-interstitial-dismiss")
		{
			OnAmazonInterstitialDismissed();
		}
	}
}
