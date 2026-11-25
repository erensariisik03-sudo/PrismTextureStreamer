using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

namespace AxesInMotion.SocialPlugin.Adapters
{
	public class GooglePlayAdapter : ISocial, ISocialLeaderboard
	{
		private bool _initialized;

		public bool Requesting { get; set; }

		public GooglePlayAdapter()
		{
			Requesting = false;
			_initialized = false;
		}

		public string Id()
		{
			return Social.localUser.userName;
		}

		public bool IsLoggedIn()
		{
			return Social.localUser.authenticated;
		}

		public void Logout()
		{
			((PlayGamesPlatform)Social.Active).SignOut();
		}

		public void ReportScore(string leaderboard, long score)
		{
			Requesting = true;
			Social.ReportScore(score, leaderboard, (bool success) =>
			{
				Requesting = false;
				if (!success)
				{
				}
			});
		}

		public void ShowLeaderboard(string leaderboard)
		{
			Requesting = true;
			((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderboard);
			Requesting = false;
		}

		public void ShowAllLeaderboard()
		{
			Requesting = true;
			((PlayGamesPlatform)Social.Active).ShowLeaderboardUI();
			Requesting = false;
		}

		public void ReportAchievement(string achievement, float progress)
		{
			Social.ReportProgress(achievement, progress, (bool success) =>
			{
				if (!success)
				{
				}
			});
		}

		public void IncrementAchievement(string achievement, int steps)
		{
			Requesting = true;
			((PlayGamesPlatform)Social.Active).IncrementAchievement(achievement, steps, (bool success) =>
			{
				Requesting = false;
			});
		}

		public void ShowAchievement()
		{
			Requesting = true;
			Social.ShowAchievementsUI();
			Requesting = false;
		}

		public bool IsInitialized()
		{
			return _initialized;
		}

		public void Init(SocialInitializedCallbackDelegate initializedCallback)
		{
			ISocialEventArgs args = SocialEventArgsFactory.Create(SocialEventType.Initialized);
			if (IsInitialized())
			{
				Requesting = false;
				initializedCallback(args);
				return;
			}
			PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder().Build();
			PlayGamesPlatform.InitializeInstance(configuration);
			PlayGamesPlatform.DebugLogEnabled = Debug.isDebugBuild;
			Requesting = true;
			PlayGamesPlatform.Activate();
			Requesting = false;
			_initialized = true;
			if (initializedCallback != null)
			{
				initializedCallback(args);
			}
		}

		public void Login(SocialLoginCallbackDelegate callback = null, SocialScope[] scopes = null)
		{
			Requesting = true;
			Social.localUser.Authenticate((bool success) =>
			{
				Requesting = false;
				if (success)
				{
					ISocialEventArgs args = SocialEventArgsFactory.Create(SocialEventType.LoginSuccess);
					if (callback != null)
					{
						callback(args);
					}
				}
				else
				{
					ISocialEventArgs args2 = SocialEventArgsFactory.Create(SocialEventType.LoginFailed);
					if (callback != null)
					{
						callback(args2);
					}
				}
			});
		}
	}
}
