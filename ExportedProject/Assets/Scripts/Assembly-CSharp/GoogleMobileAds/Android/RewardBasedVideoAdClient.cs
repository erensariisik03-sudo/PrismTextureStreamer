using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	public class RewardBasedVideoAdClient : AndroidJavaProxy, IRewardBasedVideoAdClient
	{
		private AndroidJavaObject androidRewardBasedVideo;

		public event EventHandler<EventArgs> OnAdLoaded = delegate
		{
		};

		public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad = delegate
		{
		};

		public event EventHandler<EventArgs> OnAdOpening = delegate
		{
		};

		public event EventHandler<EventArgs> OnAdStarted = delegate
		{
		};

		public event EventHandler<EventArgs> OnAdClosed = delegate
		{
		};

		public event EventHandler<Reward> OnAdRewarded = delegate
		{
		};

		public event EventHandler<EventArgs> OnAdLeavingApplication = delegate
		{
		};

		public RewardBasedVideoAdClient()
			: base("com.google.unity.ads.UnityRewardBasedVideoAdListener")
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			androidRewardBasedVideo = new AndroidJavaObject("com.google.unity.ads.RewardBasedVideo", androidJavaObject, this);
		}

		public void CreateRewardBasedVideoAd()
		{
			androidRewardBasedVideo.Call("create");
		}

		public void LoadAd(AdRequest request, string adUnitId)
		{
			androidRewardBasedVideo.Call("loadAd", Utils.GetAdRequestJavaObject(request), adUnitId);
		}

		public bool IsLoaded()
		{
			return androidRewardBasedVideo.Call<bool>("isLoaded", new object[0]);
		}

		public void ShowRewardBasedVideoAd()
		{
			androidRewardBasedVideo.Call("show");
		}

		public void DestroyRewardBasedVideoAd()
		{
			androidRewardBasedVideo.Call("destroy");
		}

		private void onAdLoaded()
		{
			if (OnAdLoaded != null)
			{
				OnAdLoaded(this, EventArgs.Empty);
			}
		}

		private void onAdFailedToLoad(string errorReason)
		{
			if (OnAdFailedToLoad != null)
			{
				AdFailedToLoadEventArgs e = new AdFailedToLoadEventArgs();
				e.Message = errorReason;
				AdFailedToLoadEventArgs e2 = e;
				OnAdFailedToLoad(this, e2);
			}
		}

		private void onAdOpened()
		{
			if (OnAdOpening != null)
			{
				OnAdOpening(this, EventArgs.Empty);
			}
		}

		private void onAdStarted()
		{
			if (OnAdStarted != null)
			{
				OnAdStarted(this, EventArgs.Empty);
			}
		}

		private void onAdClosed()
		{
			if (OnAdClosed != null)
			{
				OnAdClosed(this, EventArgs.Empty);
			}
		}

		private void onAdRewarded(string type, float amount)
		{
			if (OnAdRewarded != null)
			{
				Reward reward = new Reward();
				reward.Type = type;
				reward.Amount = amount;
				Reward e = reward;
				OnAdRewarded(this, e);
			}
		}

		private void onAdLeftApplication()
		{
			if (OnAdLeavingApplication != null)
			{
				OnAdLeavingApplication(this, EventArgs.Empty);
			}
		}
	}
}
