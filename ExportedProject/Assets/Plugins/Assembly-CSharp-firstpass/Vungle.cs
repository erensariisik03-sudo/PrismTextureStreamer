using System;
using System.Collections.Generic;
using System.Text;

public class Vungle
{
	private const string PLUGIN_VERSION = "2.2.4";

	private const string IOS_SDK_VERSION = "3.2.0";

	private const string ANDROID_SDK_VERSION = "3.3.3";

	public static string VersionInfo
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder("unity-");
			return stringBuilder.Append("2.2.4").Append("/android-").Append("3.3.3")
				.ToString();
		}
	}

	public static event Action onAdStartedEvent;

	[Obsolete("Please use onAdFinishedEvent event instead this and onAdViewedEvent event.")]
	public static event Action onAdEndedEvent;

	public static event Action<bool> adPlayableEvent;

	[Obsolete("Please use adPlayableEvent event instead this and onCachedAdAvailableEvent event.")]
	public static event Action onCachedAdAvailableEvent;

	[Obsolete("Please use onAdFinishedEvent event instead this and onAdEndedEvent event.")]
	public static event Action<double, double> onAdViewedEvent;

	public static event Action<string> onLogEvent;

	public static event Action<AdFinishedEventArgs> onAdFinishedEvent;

	static Vungle()
	{
		VungleManager.OnAdStartEvent += adStarted;
		VungleManager.OnAdEndEvent += adEnded;
		VungleManager.OnCachedAdAvailableEvent += cachedAdAvailable;
		VungleManager.OnAdPlayableEvent += adPlayable;
		VungleManager.OnVideoViewEvent += videoViewed;
		VungleManager.OnSDKLogEvent += onLog;
		VungleManager.OnAdFinishedEvent += adFinished;
	}

	private static void adStarted()
	{
		if (onAdStartedEvent != null)
		{
			onAdStartedEvent();
		}
	}

	private static void adEnded()
	{
		if (onAdEndedEvent != null)
		{
			onAdEndedEvent();
		}
	}

	private static void videoViewed(double timeWatched, double totalDuration)
	{
		if (onAdViewedEvent != null)
		{
			onAdViewedEvent(timeWatched, totalDuration);
		}
	}

	private static void cachedAdAvailable()
	{
		if (onCachedAdAvailableEvent != null)
		{
			onCachedAdAvailableEvent();
		}
	}

	private static void adPlayable(bool playable)
	{
		if (adPlayableEvent != null)
		{
			adPlayableEvent(playable);
		}
	}

	private static void onLog(string log)
	{
		if (onLogEvent != null)
		{
			onLogEvent(log);
		}
	}

	private static void adFinished(AdFinishedEventArgs args)
	{
		if (onAdFinishedEvent != null)
		{
			onAdFinishedEvent(args);
		}
	}

	public static void init(string androidAppId, string iosAppId)
	{
		VungleAndroid.init(androidAppId, "2.2.4");
	}

	public static void setSoundEnabled(bool isEnabled)
	{
		VungleAndroid.setSoundEnabled(isEnabled);
	}

	public static bool isAdvertAvailable()
	{
		return VungleAndroid.isVideoAvailable();
	}

	public static void playAd(bool incentivized = false, string user = "", int orientation = 6)
	{
		VungleAndroid.playAd(incentivized, user);
	}

	public static void playAdWithOptions(Dictionary<string, object> options)
	{
		if (options == null)
		{
			throw new ArgumentException("You can not call this method with null parameter");
		}
		VungleAndroid.playAdEx(options);
	}

	public static void clearCache()
	{
	}

	public static void clearSleep()
	{
	}

	public static void setEndPoint(string endPoint)
	{
	}

	public static void setLogEnable(bool enable)
	{
	}

	public static string getEndPoint()
	{
		return string.Empty;
	}

	public static void onResume()
	{
		VungleAndroid.onResume();
	}

	public static void onPause()
	{
		VungleAndroid.onPause();
	}
}
