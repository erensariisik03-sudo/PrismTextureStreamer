using System;
using System.Text.RegularExpressions;
using GameAnalyticsSDK.Events;
using GameAnalyticsSDK.Setup;
using GameAnalyticsSDK.State;
using GameAnalyticsSDK.Wrapper;
using UnityEngine;

namespace GameAnalyticsSDK
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(GA_SpecialEvents))]
	public class GameAnalytics : MonoBehaviour
	{
		private static Settings _settings;

		private static GameAnalytics _instance;

		public static Settings SettingsGA
		{
			get
			{
				if (_settings == null)
				{
					InitAPI();
				}
				return _settings;
			}
			private set
			{
				_settings = value;
			}
		}

		public void Awake()
		{
			if (Application.isPlaying)
			{
				if (_instance != null)
				{
					Debug.LogWarning("Destroying duplicate GameAnalytics object - only one is allowed per scene!");
					UnityEngine.Object.Destroy(base.gameObject);
					return;
				}
				_instance = this;
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
				Application.logMessageReceived += GA_Debug.HandleLog;
				Initialize();
			}
		}

		private void OnDestroy()
		{
			if (Application.isPlaying && _instance == this)
			{
				_instance = null;
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.gameanalytics.sdk.GAPlatform");
			if (pauseStatus)
			{
				androidJavaClass2.CallStatic("onActivityPaused", androidJavaObject);
			}
			else
			{
				androidJavaClass2.CallStatic("onActivityResumed", androidJavaObject);
			}
		}

		private void OnApplicationQuit()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.gameanalytics.sdk.GAPlatform");
			androidJavaClass2.CallStatic("onActivityStopped", androidJavaObject);
		}

		private static void InitAPI()
		{
			try
			{
				_settings = (Settings)Resources.Load("GameAnalytics/Settings", typeof(Settings));
				GAState.Init();
			}
			catch (Exception ex)
			{
				Debug.Log("Error getting Settings in InitAPI: " + ex.Message);
			}
		}

		private static void Initialize()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (SettingsGA.InfoLogBuild)
			{
				GA_Setup.SetInfoLog(true);
			}
			if (SettingsGA.VerboseLogBuild)
			{
				GA_Setup.SetVerboseLog(true);
			}
			int platformIndex = GetPlatformIndex();
			GA_Wrapper.SetUnitySdkVersion("unity " + Settings.VERSION);
			GA_Wrapper.SetUnityEngineVersion("unity " + GetUnityVersion());
			if (platformIndex >= 0)
			{
				GA_Wrapper.SetBuild(SettingsGA.Build[platformIndex]);
			}
			if (SettingsGA.CustomDimensions01.Count > 0)
			{
				GA_Setup.SetAvailableCustomDimensions01(SettingsGA.CustomDimensions01);
			}
			if (SettingsGA.CustomDimensions02.Count > 0)
			{
				GA_Setup.SetAvailableCustomDimensions02(SettingsGA.CustomDimensions02);
			}
			if (SettingsGA.CustomDimensions03.Count > 0)
			{
				GA_Setup.SetAvailableCustomDimensions03(SettingsGA.CustomDimensions03);
			}
			if (SettingsGA.ResourceItemTypes.Count > 0)
			{
				GA_Setup.SetAvailableResourceItemTypes(SettingsGA.ResourceItemTypes);
			}
			if (SettingsGA.ResourceCurrencies.Count > 0)
			{
				GA_Setup.SetAvailableResourceCurrencies(SettingsGA.ResourceCurrencies);
			}
			if (SettingsGA.UseManualSessionHandling)
			{
				SetEnabledManualSessionHandling(true);
			}
			if (platformIndex >= 0)
			{
				if (!SettingsGA.UseCustomId)
				{
					GA_Wrapper.Initialize(SettingsGA.GetGameKey(platformIndex), SettingsGA.GetSecretKey(platformIndex));
				}
				else
				{
					Debug.Log("Custom id is enabled. Initialize is delayed until custom id has been set.");
				}
			}
			else
			{
				Debug.LogWarning("Unsupported platform: " + Application.platform);
			}
		}

		public static void NewBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType)
		{
			GA_Business.NewEvent(currency, amount, itemType, itemId, cartType);
		}

		public static void NewBusinessEventGooglePlay(string currency, int amount, string itemType, string itemId, string cartType, string receipt, string signature)
		{
			GA_Business.NewEventGooglePlay(currency, amount, itemType, itemId, cartType, receipt, signature);
		}

		public static void NewDesignEvent(string eventName)
		{
			GA_Design.NewEvent(eventName);
		}

		public static void NewDesignEvent(string eventName, float eventValue)
		{
			GA_Design.NewEvent(eventName, eventValue);
		}

		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01)
		{
			GA_Progression.NewEvent(progressionStatus, progression01);
		}

		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02)
		{
			GA_Progression.NewEvent(progressionStatus, progression01, progression02);
		}

		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03)
		{
			GA_Progression.NewEvent(progressionStatus, progression01, progression02, progression03);
		}

		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, int score)
		{
			GA_Progression.NewEvent(progressionStatus, progression01, score);
		}

		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, int score)
		{
			GA_Progression.NewEvent(progressionStatus, progression01, progression02, score);
		}

		public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int score)
		{
			GA_Progression.NewEvent(progressionStatus, progression01, progression02, progression03, score);
		}

		public static void NewResourceEvent(GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId)
		{
			GA_Resource.NewEvent(flowType, currency, amount, itemType, itemId);
		}

		public static void NewErrorEvent(GAErrorSeverity severity, string message)
		{
			GA_Error.NewEvent(severity, message);
		}

		public static void SetFacebookId(string facebookId)
		{
			GA_Setup.SetFacebookId(facebookId);
		}

		public static void SetGender(GAGender gender)
		{
			GA_Setup.SetGender(gender);
		}

		public static void SetBirthYear(int birthYear)
		{
			GA_Setup.SetBirthYear(birthYear);
		}

		public static void SetCustomId(string userId)
		{
			if (SettingsGA.UseCustomId)
			{
				Debug.Log("Initializing with custom id: " + userId);
				GA_Wrapper.SetCustomUserId(userId);
				int platformIndex = GetPlatformIndex();
				if (platformIndex >= 0)
				{
					GA_Wrapper.Initialize(SettingsGA.GetGameKey(platformIndex), SettingsGA.GetSecretKey(platformIndex));
				}
				else
				{
					Debug.LogWarning("Unsupported platform (or missing platform in settings): " + Application.platform);
				}
			}
			else
			{
				Debug.LogWarning("Custom id is not enabled");
			}
		}

		public static void SetEnabledManualSessionHandling(bool enabled)
		{
			GA_Wrapper.SetEnabledManualSessionHandling(enabled);
		}

		public static void StartSession()
		{
			GA_Wrapper.StartSession();
		}

		public static void EndSession()
		{
			GA_Wrapper.EndSession();
		}

		public static void SetCustomDimension01(string customDimension)
		{
			GA_Setup.SetCustomDimension01(customDimension);
		}

		public static void SetCustomDimension02(string customDimension)
		{
			GA_Setup.SetCustomDimension02(customDimension);
		}

		public static void SetCustomDimension03(string customDimension)
		{
			GA_Setup.SetCustomDimension03(customDimension);
		}

		private static string GetUnityVersion()
		{
			string text = string.Empty;
			string[] array = Application.unityVersion.Split('.');
			for (int i = 0; i < array.Length; i++)
			{
				int result;
				if (int.TryParse(array[i], out result))
				{
					text = ((i != 0) ? (text + "." + array[i]) : array[i]);
					continue;
				}
				string[] array2 = Regex.Split(array[i], "[^\\d]+");
				if (array2.Length > 0 && int.TryParse(array2[0], out result))
				{
					text = text + "." + array2[0];
				}
			}
			return text;
		}

		private static int GetPlatformIndex()
		{
			int num = -1;
			RuntimePlatform platform = Application.platform;
			if (platform == RuntimePlatform.IPhonePlayer)
			{
				return SettingsGA.Platforms.IndexOf(platform);
			}
			return SettingsGA.Platforms.IndexOf(platform);
		}
	}
}
