using System;
using System.Collections.Generic;
using MiniJSONV;
using UnityEngine;

public class VungleManager : MonoBehaviour
{
	private AdFinishedEventArgs adFinishedEventArgs;

	public static event Action OnAdStartEvent;

	[Obsolete("Please use OnAdPlayable instead.")]
	public static event Action OnCachedAdAvailableEvent;

	public static event Action<bool> OnAdPlayableEvent;

	[Obsolete("Please use OnAdFinishedEvent instead.")]
	public static event Action OnAdEndEvent;

	[Obsolete("Please use OnAdFinishedEvent instead.")]
	public static event Action<double, double> OnVideoViewEvent;

	public static event Action<string> OnSDKLogEvent;

	public static event Action<AdFinishedEventArgs> OnAdFinishedEvent;

	static VungleManager()
	{
		try
		{
			GameObject gameObject = new GameObject("VungleManager");
			gameObject.AddComponent<VungleManager>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
		catch (UnityException)
		{
			Debug.LogWarning("It looks like you have the VungleManager on a GameObject in your scene. Please remove the script from your scene.");
		}
	}

	public static void noop()
	{
	}

	private void OnAdStart(string empty)
	{
		OnAdStartEvent();
	}

	private void OnCachedAdAvailable(string empty)
	{
		OnCachedAdAvailableEvent();
	}

	private void OnAdPlayable(string playable)
	{
		OnAdPlayableEvent("1".Equals(playable));
	}

	private void OnVideoView(string param)
	{
		string[] array = param.Split('-');
		if (array.Length == 3)
		{
			double num = double.Parse(array[1]) / 1000.0;
			double num2 = double.Parse(array[2]) / 1000.0;
			bool flag = adFinishedEventArgs != null;
			if (!flag)
			{
				adFinishedEventArgs = new AdFinishedEventArgs();
			}
			adFinishedEventArgs.IsCompletedView = bool.Parse(array[0]);
			adFinishedEventArgs.TimeWatched = num;
			adFinishedEventArgs.TotalDuration = num2;
			if (flag)
			{
				OnAdFinishedEvent(adFinishedEventArgs);
				adFinishedEventArgs = null;
			}
			OnVideoViewEvent(num, num2);
		}
	}

	private void OnCloseProductSheet(string empty)
	{
	}

	private void OnSDKLog(string log)
	{
		OnSDKLogEvent(log);
	}

	private void OnAdEnd(string param)
	{
		bool flag = adFinishedEventArgs != null;
		if (!flag)
		{
			adFinishedEventArgs = new AdFinishedEventArgs();
		}
		adFinishedEventArgs.WasCallToActionClicked = param.Equals("1");
		if (flag)
		{
			OnAdFinishedEvent(adFinishedEventArgs);
			adFinishedEventArgs = null;
		}
		OnAdEndEvent();
	}

	private bool extractBoolValue(string json, string key)
	{
		Dictionary<string, object> attrs = (Dictionary<string, object>)Json.Deserialize(json);
		return extractBoolValue(attrs, key);
	}

	private bool extractBoolValue(Dictionary<string, object> attrs, string key)
	{
		return bool.Parse(attrs[key].ToString());
	}
}
