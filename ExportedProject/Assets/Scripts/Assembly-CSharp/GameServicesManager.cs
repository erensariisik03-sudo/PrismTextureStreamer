using AxesInMotion.SocialPlugin;
using GooglePlayGames;
using UnityEngine;

public class GameServicesManager : MonoBehaviour
{
	public bool tryLoginOnce = true;

	private ISocial _social;

	private string _leaderboardId;

	private bool _hasTryLogin;

	private bool _paused;

	public bool isLoggedIn
	{
		get
		{
			return _social.IsLoggedIn();
		}
	}

	public bool isInitialized
	{
		get
		{
			return _social.IsInitialized();
		}
	}

	[HideInInspector]
	public event GameServicesCallback loginSucceedBeforeShowLeaderboardEvent;

	[HideInInspector]
	public event GameServicesCallback loginSucceedBeforeShowAllLeaderboardEvent;

	private void Start()
	{
		MySingleton<Manager>.Instance.gameServicesManager = this;
		_social = SocialFactory.Create(SocialType.GooglePlay);
		if (!_social.IsInitialized())
		{
			_social.Init(InitializeCallback);
		}
		else
		{
			InitializeCallback(null);
		}
	}

	private void InitializeCallback(ISocialEventArgs args)
	{
		if (args.Error)
		{
			Debug.LogError("InitializeCallback :: por alguna razón ocurrió un error al inicializarse");
		}
	}

	public void CheckAndLogin(SocialLoginCallbackDelegate callback)
	{
		if (!_social.IsLoggedIn())
		{
			if ((tryLoginOnce && !_hasTryLogin) || !tryLoginOnce)
			{
				_social.Login(callback);
				_hasTryLogin = true;
			}
		}
		else
		{
			callback(SocialEventArgsFactory.Create(SocialEventType.LoginSuccess));
		}
	}

	public void Login(SocialLoginCallbackDelegate callback)
	{
		_social.Login(callback);
	}

	public void ShowAllLeaderboardOrLogin()
	{
		if (_social.IsLoggedIn())
		{
			((ISocialLeaderboard)_social).ShowAllLeaderboard();
		}
		else
		{
			Login(LoginFromShowAllLeaderboardCallback);
		}
	}

	public void ShowLeaderboardOrLogin(string leaderboard)
	{
		if (_social.IsLoggedIn())
		{
			((ISocialLeaderboard)_social).ShowLeaderboard(leaderboard);
			return;
		}
		_leaderboardId = leaderboard;
		Login(LoginFromShowLeaderboardCallback);
	}

	public void ShowAllLeaderboard()
	{
		((ISocialLeaderboard)_social).ShowAllLeaderboard();
	}

	public void ShowLeaderboard(string leaderboard)
	{
		((ISocialLeaderboard)_social).ShowLeaderboard(leaderboard);
	}

	public void ShareLeaderboardScore(string leaderboard, long score)
	{
		Debug.Log("ShareLeaderboardScore: " + leaderboard + score);
		((ISocialLeaderboard)_social).ReportScore(leaderboard, score);
	}

	private void LoginFromShowLeaderboardCallback(ISocialEventArgs args)
	{
		if (_social.IsLoggedIn())
		{
			if (loginSucceedBeforeShowLeaderboardEvent != null)
			{
				loginSucceedBeforeShowLeaderboardEvent();
				loginSucceedBeforeShowLeaderboardEvent = null;
			}
			ShowLeaderboard(_leaderboardId);
		}
	}

	private void LoginFromShowAllLeaderboardCallback(ISocialEventArgs args)
	{
		if (_social.IsLoggedIn())
		{
			if (loginSucceedBeforeShowLeaderboardEvent != null)
			{
				loginSucceedBeforeShowAllLeaderboardEvent();
				loginSucceedBeforeShowAllLeaderboardEvent = null;
			}
			ShowAllLeaderboard();
		}
	}

	public void SimpleTest()
	{
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
		Social.localUser.Authenticate((bool success) =>
		{
		});
	}
}
