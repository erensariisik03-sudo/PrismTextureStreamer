using System.Collections.Generic;

public class AmplitudeController
{
	public AmplitudeController()
	{
		Amplitude instance = Amplitude.Instance;
		instance.logging = true;
		instance.init("61eac4c04997ace46de86919949c8cda");
		RefreshUserPropertys();
	}

	public void RefreshUserPropertys()
	{
		Amplitude.Instance.setUserProperty("kilometers", MySingleton<Manager>.Instance.stats.GetDistanceMilestone());
		Amplitude.Instance.setUserProperty("challenges_completed", MySingleton<Manager>.Instance.stats.GetCompleteChallenges());
		Amplitude.Instance.setUserProperty("cars_unlocked", MySingleton<Manager>.Instance.stats.GetUnlockedCars());
		Amplitude.Instance.setUserProperty("days_played", MySingleton<Manager>.Instance.stats.GetNumPlayedDays());
		Amplitude.Instance.setUserProperty("car_packs_unlocked", MySingleton<Manager>.Instance.stats.GetNumPacksBought());
		Amplitude.Instance.setUserProperty("control_config", MySingleton<Manager>.Instance.controls.ToString());
	}

	public void SendEvent(string idEvent)
	{
		RefreshUserPropertys();
		Amplitude.Instance.logEvent(idEvent);
	}

	public void SendEvent(string idEvent, Dictionary<string, object> dictData)
	{
		RefreshUserPropertys();
		Amplitude.Instance.logEvent(idEvent, dictData);
	}

	public void SendRevenueEvent(string productID, double realPrice, string receipt, string receiptSignature, Dictionary<string, object> dictData)
	{
		RefreshUserPropertys();
		Amplitude.Instance.logRevenue(productID, 1, realPrice, receipt, receiptSignature, "purchase", dictData);
	}
}
