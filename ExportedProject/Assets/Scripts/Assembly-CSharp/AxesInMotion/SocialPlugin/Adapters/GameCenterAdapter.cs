using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace AxesInMotion.SocialPlugin.Adapters
{
	public class GameCenterAdapter : ISocial, ISocialLeaderboard
	{
		private bool _initialized;

		private TimeScope _default = TimeScope.Week;

		public bool Requesting { get; set; }

		public GameCenterAdapter()
		{
			Requesting = false;
			_initialized = false;
		}

		public string Id()
		{
			return Social.localUser.userName;
		}

		public bool IsInitialized()
		{
			return _initialized;
		}

		public bool IsLoggedIn()
		{
			return Social.localUser.authenticated;
		}

		public void Logout()
		{
			throw new NotImplementedException();
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

		public void ReportScore(string leaderboard, long score)
		{
			Requesting = true;
			Social.ReportScore(score, leaderboard, (bool success) =>
			{
				Requesting = false;
				Debug.Log((!success) ? ("the score of " + leaderboard + " (" + score + ") no reportado, error por algun motivo") : ("the score of " + leaderboard + " (" + score + ") reportado "));
			});
		}

		public void ShowLeaderboard(string leaderboard)
		{
			Requesting = true;
			Debug.Log("Show Leaderboard " + leaderboard);
			Requesting = false;
		}

		public void ShowAllLeaderboard()
		{
			Requesting = true;
			Debug.Log("Show all Leaderboards");
			Requesting = false;
		}

		public void ReportAchievement(string achievement, float progress)
		{
			throw new NotImplementedException();
		}

		public void IncrementAchievement(string achievement, int steps)
		{
			throw new NotImplementedException();
		}

		public void ShowAchievement()
		{
			throw new NotImplementedException();
		}
	}
}
