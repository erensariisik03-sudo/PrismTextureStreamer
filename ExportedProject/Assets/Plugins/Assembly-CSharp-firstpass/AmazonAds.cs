using UnityEngine;

public class AmazonAds : MonoBehaviour
{
	public enum AdSize
	{
		SIZE_AUTO = 0,
		SIZE_300x50 = 1,
		SIZE_320x50 = 2,
		SIZE_300x250 = 3,
		SIZE_600x90 = 4,
		SIZE_728x90 = 5,
		SIZE_1024x50 = 6
	}

	public enum VerticalAlign
	{
		Top = 0,
		Bottom = 1
	}

	public static AmazonAds current;

	public string appKey;

	public bool testMode = true;

	public int refreshInterval = 60;

	[HideInInspector]
	public bool interstitialLoaded;

	public bool autoLoadOnStart = true;

	private bool isInit;

	private bool isBannerCreated;

	public AdSize bannerSize;

	public VerticalAlign bannerAlign = VerticalAlign.Bottom;

	private AndroidJavaObject plugin;

	private void Awake()
	{
		if (current == null)
		{
			current = this;
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.twimler.plugins.AmazonAds");
			plugin = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
			InvokeRepeating("Refresh", refreshInterval, refreshInterval);
		}
	}

	public void Init()
	{
		plugin.Call("init", base.gameObject.name, appKey, testMode);
		isInit = true;
		if (autoLoadOnStart)
		{
			CreateBanner();
		}
	}

	public void Refresh()
	{
		if (isInit)
		{
			plugin.Call("refresh");
		}
	}

	public void CreateBanner()
	{
		if (isInit)
		{
			plugin.Call("createBanner", GetPosition(), (int)bannerSize);
			isBannerCreated = true;
		}
	}

	public void DestroyBanner()
	{
		if (isInit)
		{
			plugin.Call("destroyBanner");
			isBannerCreated = false;
		}
	}

	public void HideBanner(bool hide)
	{
		if (isInit && isBannerCreated)
		{
			plugin.Call("hideBanner", hide);
		}
	}

	public void RequestInterstitial()
	{
		if (isInit)
		{
			plugin.Call("requestInterstitial");
		}
	}

	public void ShowInterstitial()
	{
		if (isInit && interstitialLoaded)
		{
			plugin.Call("showInterstitial");
			interstitialLoaded = false;
		}
	}

	private string GetPosition()
	{
		string result = "BM";
		if (bannerAlign == VerticalAlign.Top)
		{
			result = "TM";
		}
		if (bannerAlign == VerticalAlign.Bottom)
		{
			result = "BM";
		}
		return result;
	}
}
