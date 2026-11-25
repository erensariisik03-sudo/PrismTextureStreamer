using AxesInMotion.SocialPlugin;
using AxesInMotion.SocialPlugin.Adapters;
using AxesInMotion.UnityHelpers;
using UnityEngine;

public class FacebookManager : MonoBehaviour
{
	public delegate void SocialBeforeFeedCallbackDelegate();

	public string inviteTitleLocalizationKey = "facebook_invite_title";

	public string inviteMessageLocalizationKey = "facebook_invite_message";

	public int inviteMaxRecipients = 20;

	private ISocial _social;

	private SocialCustomCallbackDelegate _onSuccess;

	private SocialCustomCallbackDelegate _onCancel;

	private SocialCustomCallbackDelegate _onFailed;

	private SocialCustomCallbackDelegate _onTakeScreenshotSuccess;

	private SocialCustomCallbackDelegate _onTakeScreenshotFailed;

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

	public string ID
	{
		get
		{
			return _social.Id();
		}
	}

	private void Start()
	{
		_social = SocialFactory.Create(SocialType.Facebook);
		Initialize(InitializeCallback);
		MySingleton<Manager>.Instance.facebookManager = this;
	}

	private void Initialize(SocialInitializedCallbackDelegate initializeCallback)
	{
		if (!_social.IsInitialized())
		{
			_social.Init(initializeCallback);
			return;
		}
		ISocialEventArgs args = SocialEventArgsFactory.Create(SocialEventType.Initialized);
		InitializeCallback(args);
	}

	private void InitializeCallback(ISocialEventArgs args)
	{
	}

	private void CustomLoginCallback(ISocialEventArgs args)
	{
		if (!args.Error)
		{
			if (args.Type == SocialEventType.LoginSuccess)
			{
				_onSuccess();
			}
			else if (args.Type == SocialEventType.LoginCanceled)
			{
				_onCancel();
			}
			else
			{
				Debug.LogError("args.Type no implementado");
			}
		}
		else
		{
			_onFailed();
		}
	}

	public void Login(SocialCustomCallbackDelegate successCallback, SocialCustomCallbackDelegate cancelCallback, SocialCustomCallbackDelegate failedCallback, SocialScope[] scopes = null)
	{
		_onSuccess = successCallback;
		_onFailed = failedCallback;
		_onCancel = cancelCallback;
		if (_social.Requesting)
		{
			ISocialEventArgs e = SocialEventArgsFactory.Create(SocialEventType.LoginFailed);
			e.Error = true;
			e.Message = "requesting";
			CustomLoginCallback(e);
			return;
		}
		SocialScope[] array = scopes;
		if (array == null)
		{
			array = new SocialScope[3]
			{
				SocialScope.Publish,
				SocialScope.Email,
				SocialScope.Friends
			};
		}
		_social.Login(CustomLoginCallback, array);
	}

	public void Logout()
	{
		_social.Logout();
	}

	public void FeedImage(string message, Texture2D texture, SocialCustomCallbackDelegate successCallback, SocialCustomCallbackDelegate failedCallback)
	{
		_onTakeScreenshotSuccess = successCallback;
		_onTakeScreenshotFailed = failedCallback;
		if (!_social.IsInitialized())
		{
			_onTakeScreenshotFailed();
		}
		else if (_social.Requesting)
		{
			_onTakeScreenshotFailed();
		}
		else if (_social.IsLoggedIn())
		{
			byte[] bytesImage = ImageHelper.GetBytesImage(texture);
			WWWForm wwwForm = new WWWForm();
			((FacebookAdapter)_social).FeedImage(bytesImage, message, wwwForm, TakeScreenshotCallback, string.Empty);
		}
	}

	private void TakeScreenshotCallback(ISocialEventArgs args)
	{
		if (!args.Error)
		{
			Debug.Log("Take OK");
			if (_onTakeScreenshotSuccess != null)
			{
				_onTakeScreenshotSuccess();
			}
		}
		else
		{
			Debug.Log("Take Error");
			if (_onTakeScreenshotFailed != null)
			{
				_onTakeScreenshotFailed();
			}
		}
		_onTakeScreenshotSuccess = null;
		_onTakeScreenshotFailed = null;
	}

	public void InviteFriends()
	{
		if (_social.IsInitialized() && !_social.Requesting && _social.IsLoggedIn())
		{
			((ISocialInvite)_social).Invite(Localization.Get(inviteTitleLocalizationKey), Localization.Get(inviteMessageLocalizationKey), inviteMaxRecipients, OnInviteFriendsCallback);
		}
	}

	private void OnInviteFriendsCallback(ISocialEventArgs args)
	{
		if (!args.Error)
		{
			Debug.Log("Facebook :: Inviting Friends");
		}
	}

	public void RequestGameFriends(SocialFriendsRequestCallbackDelegate callback)
	{
		((ISocialFriends)_social).RequestGameFriends(callback);
	}
}
