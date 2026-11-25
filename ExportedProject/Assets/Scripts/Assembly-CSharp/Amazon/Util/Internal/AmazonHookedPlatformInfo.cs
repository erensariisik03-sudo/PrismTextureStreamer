using System;
using Amazon.Runtime.Internal.Util;
using UnityEngine;

namespace Amazon.Util.Internal
{
	public class AmazonHookedPlatformInfo
	{
		private const string IPHONE_OS = "iPhone OS";

		private const string ANDROID_OS = "Android";

		private static Logger _logger = Logger.GetLogger(typeof(AmazonHookedPlatformInfo));

		private static AmazonHookedPlatformInfo instance = null;

		private string device_platform;

		private string device_model;

		private string device_make;

		private string device_platformVersion;

		private string device_locale;

		private string app_version_name;

		private string app_version_code;

		private string app_package_name;

		private string app_title;

		public string Platform
		{
			get
			{
				return device_platform;
			}
			internal set
			{
				if (value.Equals(RuntimePlatform.IPhonePlayer.ToString(), StringComparison.OrdinalIgnoreCase) || value.Contains("iPhoneOS") || value.Contains("iPhone"))
				{
					device_platform = "iPhone OS";
				}
				else if (value.Equals(RuntimePlatform.Android.ToString(), StringComparison.OrdinalIgnoreCase) || value.Contains("Android") || value.Contains("android"))
				{
					device_platform = "Android";
				}
				else
				{
					device_platform = value;
				}
			}
		}

		public string Model
		{
			get
			{
				return device_model;
			}
			internal set
			{
				device_model = value;
			}
		}

		public string Make
		{
			get
			{
				return device_make;
			}
			internal set
			{
				device_make = value;
			}
		}

		public string PlatformVersion
		{
			get
			{
				return device_platformVersion;
			}
			internal set
			{
				device_platformVersion = value;
			}
		}

		public string PersistentDataPath { get; set; }

		public string Locale
		{
			get
			{
				return device_locale;
			}
			set
			{
				device_locale = value;
			}
		}

		public static AmazonHookedPlatformInfo Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new AmazonHookedPlatformInfo();
					instance.Init();
				}
				return instance;
			}
		}

		public string PackageName
		{
			get
			{
				return app_package_name;
			}
			internal set
			{
				app_package_name = value;
			}
		}

		public string VersionName
		{
			get
			{
				return app_version_name;
			}
			internal set
			{
				app_version_name = value;
			}
		}

		public string VersionCode
		{
			get
			{
				return app_version_code;
			}
			internal set
			{
				app_version_code = value;
			}
		}

		public string Title
		{
			get
			{
				return app_title;
			}
			internal set
			{
				app_title = value;
			}
		}

		private AmazonHookedPlatformInfo()
		{
		}

		public void Init()
		{
			PersistentDataPath = Application.persistentDataPath;
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.os.Build");
			AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.os.Build$VERSION");
			PlatformVersion = androidJavaClass2.GetStatic<string>("RELEASE");
			Model = androidJavaClass.GetStatic<string>("MODEL");
			Make = androidJavaClass.GetStatic<string>("MANUFACTURER");
			AndroidJavaClass androidJavaClass3 = new AndroidJavaClass("java.util.Locale");
			AndroidJavaObject androidJavaObject = androidJavaClass3.CallStatic<AndroidJavaObject>("getDefault", new object[0]);
			Locale = androidJavaObject.Call<string>("toString", new object[0]);
			Platform = Application.platform.ToString();
			AndroidJavaClass androidJavaClass4 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject androidJavaObject2 = androidJavaClass4.GetStatic<AndroidJavaObject>("currentActivity");
			PackageName = androidJavaObject2.Call<string>("getPackageName", new object[0]);
			AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("getPackageManager", new object[0]);
			AndroidJavaObject androidJavaObject4 = androidJavaObject3.Call<AndroidJavaObject>("getPackageInfo", new object[2] { PackageName, 0 });
			AndroidJavaObject androidJavaObject5 = androidJavaObject3.Call<AndroidJavaObject>("getApplicationInfo", new object[2] { PackageName, 0 });
			VersionCode = Convert.ToString(androidJavaObject4.Get<int>("versionCode"));
			VersionName = androidJavaObject4.Get<string>("versionName");
			Title = androidJavaObject3.Call<string>("getApplicationLabel", new object[1] { androidJavaObject5 });
			_logger.DebugFormat("make = {0}", Make);
			_logger.DebugFormat("model = {0}", Model);
			_logger.DebugFormat("platform version = {0}", PlatformVersion);
			_logger.DebugFormat("locale = {0}", Locale);
			_logger.DebugFormat("Title = {0}", Title);
			_logger.DebugFormat("Version Code = {0}", VersionCode);
			_logger.DebugFormat("Version Name = {0}", VersionName);
			_logger.DebugFormat("Package Name = {0}", PackageName);
		}
	}
}
