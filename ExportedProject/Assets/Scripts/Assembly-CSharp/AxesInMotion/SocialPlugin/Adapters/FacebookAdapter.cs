using System.Collections;
using System.Collections.Generic;
using Facebook;
using Facebook.MiniJSON;
using UnityEngine;

namespace AxesInMotion.SocialPlugin.Adapters
{
	public class FacebookAdapter : ISocial, ISocialFeed, ISocialFriends, ISocialInvite, ISocialProfile
	{
		private Dictionary<SocialScope, string> _scopes;

		private SocialInitializedCallbackDelegate _socialInitializedCallback;

		private SocialInviteCallbackDelegate _socialInviteCallback;

		private SocialLoginCallbackDelegate _socialLoginCallback;

		private SocialFeedCallbackDelegate _socialFeedCallbackDelegate;

		private SocialProfileCallbackDelegate _socialRequestFirstNameCallback;

		private SocialFriendsRequestCallbackDelegate _socialFriendsRequestCallback;

		public bool Requesting { get; set; }

		public FacebookAdapter()
		{
			Requesting = false;
			FillScopesDictionaty();
		}

		public void Init(SocialInitializedCallbackDelegate initializedCallback)
		{
			if (FB.IsInitialized)
			{
				ISocialEventArgs args = SocialEventArgsFactory.Create(SocialEventType.Initialized);
				_socialInitializedCallback(args);
			}
			else
			{
				_socialInitializedCallback = initializedCallback;
				FB.Init(OnInitCompleteCallback, OnHideUnityCallback);
			}
		}

		public bool IsLoggedIn()
		{
			return FB.IsLoggedIn;
		}

		public void Login(SocialLoginCallbackDelegate callback = null, SocialScope[] scopes = null)
		{
			_socialLoginCallback = callback;
			FB.Login(PerformScopesString(scopes), OnLoginCallback);
		}

		public void Logout()
		{
			FB.Logout();
		}

		public string Id()
		{
			return FB.UserId;
		}

		public void Invite(string title, string message, int maxRecipients, SocialInviteCallbackDelegate callback)
		{
			_socialInviteCallback = callback;
			FB.AppRequest(message, null, null, null, maxRecipients, string.Empty, title, OnAppRequestCallback);
		}

		public void FeedImage(byte[] imageBytes, string message, WWWForm wwwForm, SocialFeedCallbackDelegate socialFeedCallbackDelegate, string mimetype = "")
		{
			if (mimetype == string.Empty)
			{
				wwwForm.AddBinaryData("image", imageBytes, "image.png");
			}
			else
			{
				wwwForm.AddBinaryData("image", imageBytes, "image.png", mimetype);
			}
			wwwForm.AddField("message", message);
			_socialFeedCallbackDelegate = socialFeedCallbackDelegate;
			FB.API("me/photos", HttpMethod.POST, OnAPICallback, wwwForm);
		}

		private string PerformScopesString(SocialScope[] scopes)
		{
			string text = string.Empty;
			foreach (SocialScope key in scopes)
			{
				text = text + _scopes[key] + ",";
			}
			return text.Remove(text.Length - 1, 1);
		}

		private void FillScopesDictionaty()
		{
			_scopes = new Dictionary<SocialScope, string>();
			_scopes.Add(SocialScope.Email, "email");
			_scopes.Add(SocialScope.Publish, "publish_actions");
			_scopes.Add(SocialScope.Friends, "user_friends");
		}

		private void OnInitCompleteCallback()
		{
			Requesting = false;
			ISocialEventArgs args = SocialEventArgsFactory.Create(SocialEventType.Initialized);
			_socialInitializedCallback(args);
		}

		private void OnHideUnityCallback(bool isGameShown)
		{
			Requesting = false;
			if (isGameShown)
			{
				Debug.LogWarning("OnHideUnityCallback :: isGameShown :: " + isGameShown);
			}
			else
			{
				Debug.LogWarning("OnHideUnityCallback :: isGameShown :: " + isGameShown);
			}
		}

		private void OnLoginCallback(FBResult result)
		{
			Requesting = false;
			ISocialEventArgs e;
			if (IsLoggedIn())
			{
				Debug.Log("FacebookAdapter >> OnLoginCallback >> ESTOY LOGUEADO");
				e = SocialEventArgsFactory.Create(SocialEventType.LoginSuccess);
			}
			else if (result.Error == null)
			{
				Debug.Log("FacebookAdapter >> OnLoginCallback >> Cancelado");
				e = SocialEventArgsFactory.Create(SocialEventType.LoginCanceled);
			}
			else
			{
				Debug.Log("FacebookAdapter >> OnLoginCallback >> FALLO DE LOGIN");
				e = SocialEventArgsFactory.Create(SocialEventType.LoginFailed);
				e.Error = true;
				e.Message = result.Error;
			}
			_socialLoginCallback(e);
		}

		private void OnAppRequestCallback(FBResult result)
		{
			Debug.Log("FacebookAdapter >> OnAppRequestCallback");
			ISocialEventArgs e = SocialEventArgsFactory.Create(SocialEventType.InviteSuccess);
			Requesting = false;
			if (result != null)
			{
				Dictionary<string, object> dictionary = Json.Deserialize(result.Text) as Dictionary<string, object>;
				object value = 0;
				if (dictionary.TryGetValue("cancelled", out value))
				{
					Debug.Log("FacebookAdapter >> OnAppRequestCallback >> cancelled");
					e.Error = true;
					e.Message = "The application request was cancelled.";
				}
				else if (dictionary.TryGetValue("request", out value))
				{
					Debug.Log("FacebookAdapter >> OnAppRequestCallback >> request");
					object value2;
					dictionary.TryGetValue("to", out value2);
					((InviteSocialEventArgs)e).Recipients = value2 as ICollection;
				}
				_socialInviteCallback(e);
			}
			else
			{
				Debug.Log("FacebookAdapter >> OnAppRequestCallback >> The application request result returns null.");
				e.Error = true;
				e.Message = "The application request result returns null.";
			}
		}

		private void OnAPICallback(FBResult result)
		{
			Requesting = false;
			if (_socialFeedCallbackDelegate == null)
			{
				return;
			}
			ISocialEventArgs e;
			if (result != null)
			{
				if (result.Error == null)
				{
					e = SocialEventArgsFactory.Create(SocialEventType.FeedSuccess);
				}
				else
				{
					e = SocialEventArgsFactory.Create(SocialEventType.FeedFailed);
					e.Error = true;
					e.Message = result.Error;
				}
			}
			else
			{
				e = SocialEventArgsFactory.Create(SocialEventType.FeedFailed);
				e.Error = true;
				e.Message = "The application request result returns null.";
			}
			_socialFeedCallbackDelegate(e);
			_socialFeedCallbackDelegate = null;
		}

		public bool IsInitialized()
		{
			return FB.IsInitialized;
		}

		public void RequestFirstName(SocialProfileCallbackDelegate callback)
		{
			_socialRequestFirstNameCallback = callback;
			FB.API("/me?fields=first_name", HttpMethod.GET, OnRequestNameCallback);
		}

		public void OnRequestNameCallback(FBResult result)
		{
			if (result.Error != null)
			{
				Debug.LogError("Error al solicitar el nombre de usuario. Error: " + result.Error);
				return;
			}
			IDictionary dictionary = Json.Deserialize(result.Text) as IDictionary;
			string text = dictionary["first_name"].ToString();
			Debug.Log(text);
			if (_socialRequestFirstNameCallback != null)
			{
				_socialRequestFirstNameCallback(text);
			}
		}

		public void RequestGameFriends(SocialFriendsRequestCallbackDelegate callback)
		{
			_socialFriendsRequestCallback = callback;
			if (IsLoggedIn())
			{
				FB.API("/me/friends", HttpMethod.GET, OnRequestGameFriendsCallback);
			}
			else
			{
				_socialFriendsRequestCallback(new FBFriend[0]);
			}
		}

		public void OnRequestGameFriendsCallback(FBResult result)
		{
			if (result.Error != null)
			{
				Debug.LogError("Error al solicitar lista de amigos del juego. Error: " + result.Error);
				return;
			}
			IDictionary dictionary = Json.Deserialize(result.Text) as IDictionary;
			List<object> list = dictionary["data"] as List<object>;
			FBFriend[] array = new FBFriend[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				IDictionary dictionary2 = list[i] as IDictionary;
				array[i] = new FBFriend(dictionary2["id"] as string, dictionary2["name"] as string);
			}
			if (_socialFriendsRequestCallback != null)
			{
				_socialFriendsRequestCallback(array);
			}
		}
	}
}
