using System;
using System.Reflection;
using GoogleMobileAds.Common;

namespace GoogleMobileAds.Api
{
	public class BannerView
	{
		private IBannerClient client;

		public event EventHandler<EventArgs> OnAdLoaded;

		public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

		public event EventHandler<EventArgs> OnAdOpening;

		public event EventHandler<EventArgs> OnAdClosed;

		public event EventHandler<EventArgs> OnAdLeavingApplication;

		public BannerView(string adUnitId, AdSize adSize, AdPosition position)
		{
			Type type = Type.GetType("GoogleMobileAds.GoogleMobileAdsClientFactory,Assembly-CSharp");
			MethodInfo method = type.GetMethod("BuildBannerClient", BindingFlags.Static | BindingFlags.Public);
			client = (IBannerClient)method.Invoke(null, null);
			client.CreateBannerView(adUnitId, adSize, position);
			ConfigureBannerEvents();
		}

		public BannerView(string adUnitId, AdSize adSize, int x, int y)
		{
			Type type = Type.GetType("GoogleMobileAds.GoogleMobileAdsClientFactory,Assembly-CSharp");
			MethodInfo method = type.GetMethod("BuildBannerClient", BindingFlags.Static | BindingFlags.Public);
			client = (IBannerClient)method.Invoke(null, null);
			client.CreateBannerView(adUnitId, adSize, x, y);
			ConfigureBannerEvents();
		}

		public void LoadAd(AdRequest request)
		{
			client.LoadAd(request);
		}

		public void Hide()
		{
			client.HideBannerView();
		}

		public void Show()
		{
			client.ShowBannerView();
		}

		public void Destroy()
		{
			client.DestroyBannerView();
		}

		private void ConfigureBannerEvents()
		{
			client.OnAdLoaded += (object sender, EventArgs args) =>
			{
				if (OnAdLoaded != null)
				{
					OnAdLoaded(this, args);
				}
			};
			client.OnAdFailedToLoad += (object sender, AdFailedToLoadEventArgs args) =>
			{
				if (OnAdFailedToLoad != null)
				{
					OnAdFailedToLoad(this, args);
				}
			};
			client.OnAdOpening += (object sender, EventArgs args) =>
			{
				if (OnAdOpening != null)
				{
					OnAdOpening(this, args);
				}
			};
			client.OnAdClosed += (object sender, EventArgs args) =>
			{
				if (OnAdClosed != null)
				{
					OnAdClosed(this, args);
				}
			};
			client.OnAdLeavingApplication += (object sender, EventArgs args) =>
			{
				if (OnAdLeavingApplication != null)
				{
					OnAdLeavingApplication(this, args);
				}
			};
		}
	}
}
