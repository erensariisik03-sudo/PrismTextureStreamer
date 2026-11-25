using System.Collections.Generic;
using GameAnalyticsSDK;
using UnityEngine;

public class GameAnalyticsController : MonoBehaviour
{
	public static GameAnalyticsController instance;

	private void Awake()
	{
		instance = this;
	}

	public void SendEvent(string idEvent)
	{
		GameAnalytics.NewDesignEvent(idEvent);
	}

	public void SendEvent(string idEvent, Dictionary<string, object> dictData)
	{
		string text = idEvent;
		foreach (string key in dictData.Keys)
		{
			text = text + ":" + dictData[key];
		}
		GameAnalytics.NewDesignEvent(text);
	}

	public void SendRevenueEvent(double amount, string itemId, string receipt, string signature)
	{
		amount *= 100.0;
		int amount2 = (int)amount;
		string empty = string.Empty;
		empty = ((!(itemId == "com.aim.racing.starterpack")) ? "currency" : "pack");
		GameAnalytics.NewBusinessEventGooglePlay("USD", amount2, empty, itemId, "cartType", receipt, signature);
	}
}
