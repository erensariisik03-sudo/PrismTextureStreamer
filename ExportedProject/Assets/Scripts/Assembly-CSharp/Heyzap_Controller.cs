using System.Collections;
using Heyzap;
using UnityEngine;

public class Heyzap_Controller : MonoBehaviour
{
	private const string HEYZAP_PUBLISHER_ID = "3afe13c93d96a2a882c749550304ce3b";

	private const float VIDEO_AVAILABILITY_CHECK_TIME = 2f;

	private Task videoTask;

	public bool currentVideoAvailableStatus;

	public void Init()
	{
		HeyzapAds.Start("3afe13c93d96a2a882c749550304ce3b", 0);
		if (Debug.isDebugBuild)
		{
			HeyzapAds.ShowMediationTestSuite();
		}
		EventSubscriptionsForRewards();
		EventSubscriptionsForInterstitials();
		Loadincentivizedvideo();
	}

	public bool GetHeyzapInterstitialReady()
	{
		return HZInterstitialAd.IsAvailable();
	}

	public void ShowHeyzapInterstitial()
	{
		if (!MySingleton<Manager>.Instance.ads.disabledAds && HZInterstitialAd.IsAvailable())
		{
			HZInterstitialAd.Show();
		}
	}

	private void EventSubscriptionsForRewards()
	{
		HZIncentivizedAd.AdDisplayListener displayListener = (string adState, string adTag) =>
		{
			if (adState.Equals("show"))
			{
				MySingleton<Manager>.Instance.AdsStart();
			}
			if (adState.Equals("hide"))
			{
				MySingleton<Manager>.Instance.AdsEnded();
				Loadincentivizedvideo();
			}
			if (adState.Equals("click"))
			{
			}
			if (adState.Equals("failed"))
			{
				MySingleton<Manager>.Instance.AdsEnded();
			}
			if (adState.Equals("available"))
			{
			}
			if (adState.Equals("fetch_failed"))
			{
				Loadincentivizedvideo();
			}
			if (adState.Equals("incentivized_result_complete"))
			{
				MySingleton<Manager>.Instance.AdsEnded();
				OnVideoRewardComplete(true);
			}
			if (adState.Equals("incentivized_result_incomplete"))
			{
				MySingleton<Manager>.Instance.AdsEnded();
				OnVideoRewardComplete(false);
			}
			Debug.Log("Heyzap Rewarded :" + adState.ToString());
		};
		HZIncentivizedAd.SetDisplayListener(displayListener);
		StartCoroutine(UpdateVideoAvailableStatus());
	}

	private void EventSubscriptionsForInterstitials()
	{
		HZInterstitialAd.AdDisplayListener displayListener = (string adState, string adTag) =>
		{
			if (adState.Equals("show"))
			{
				MySingleton<Manager>.Instance.AdsStart();
			}
			if (adState.Equals("hide"))
			{
				MySingleton<Manager>.Instance.AdsEnded();
			}
			if (adState.Equals("click"))
			{
			}
			if (adState.Equals("failed"))
			{
			}
			if (adState.Equals("available"))
			{
			}
			if (adState.Equals("fetch_failed"))
			{
			}
			Debug.Log("Heyzap Interstitial :" + adState.ToString());
		};
		HZInterstitialAd.SetDisplayListener(displayListener);
	}

	private IEnumerator UpdateVideoAvailableStatus()
	{
		bool isVideoRewardsAvailable = IsVideoRewardAvailable();
		if (isVideoRewardsAvailable != currentVideoAvailableStatus)
		{
			currentVideoAvailableStatus = isVideoRewardsAvailable;
			if ((bool)MySingleton<Manager>.Instance.guiCurrentLogic)
			{
				MySingleton<Manager>.Instance.guiCurrentLogic.OnVideoRewardsAvailabilityChanged(currentVideoAvailableStatus);
			}
		}
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(2f));
		StartCoroutine(UpdateVideoAvailableStatus());
	}

	private void OnVideoRewardComplete(bool success)
	{
		if (success)
		{
			videoTask.SetProgress(1f);
			MySingleton<Manager>.Instance.guiCurrentLogic.OnVideoRewardsFinished(videoTask);
		}
	}

	public bool IsVideoRewardAvailable()
	{
		if (HZIncentivizedAd.IsAvailable())
		{
			return true;
		}
		return false;
	}

	public void ShowVideoReward(Task task)
	{
		videoTask = task;
		HZIncentivizedAd.Show();
	}

	public void Loadincentivizedvideo()
	{
		StartCoroutine(DoLoadincentivizedvideo());
	}

	private IEnumerator DoLoadincentivizedvideo()
	{
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(0.02f));
		HZIncentivizedAd.Fetch();
	}
}
