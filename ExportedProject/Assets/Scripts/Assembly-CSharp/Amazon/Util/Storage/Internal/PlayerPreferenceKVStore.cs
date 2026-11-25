using System.Threading;
using Amazon.Runtime.Internal;
using CodeStage.AntiCheat.ObscuredTypes;

namespace Amazon.Util.Storage.Internal
{
	public class PlayerPreferenceKVStore : KVStore
	{
		public override void Clear(string key)
		{
			if (UnityInitializer.IsMainThread())
			{
				ClearHelper(key);
				return;
			}
			UnityRequestQueue.Instance.ExecuteOnMainThread(() =>
			{
				ClearHelper(key);
			});
		}

		public override void Put(string key, string value)
		{
			if (UnityInitializer.IsMainThread())
			{
				PutHelper(key, value);
				return;
			}
			AutoResetEvent asyncEvent = new AutoResetEvent(false);
			UnityRequestQueue.Instance.ExecuteOnMainThread(() =>
			{
				PutHelper(key, value);
				asyncEvent.Set();
			});
			asyncEvent.WaitOne();
		}

		public override string Get(string key)
		{
			if (UnityInitializer.IsMainThread())
			{
				return GetHelper(key);
			}
			string value = string.Empty;
			AutoResetEvent asyncEvent = new AutoResetEvent(false);
			UnityRequestQueue.Instance.ExecuteOnMainThread(() =>
			{
				value = GetHelper(key);
				asyncEvent.Set();
			});
			asyncEvent.WaitOne();
			return value;
		}

		private void PutHelper(string key, string value)
		{
			ObscuredPrefs.SetString(key, value);
			ObscuredPrefs.Save();
		}

		private void ClearHelper(string key)
		{
			ObscuredPrefs.DeleteKey(key);
			ObscuredPrefs.Save();
		}

		private string GetHelper(string key)
		{
			string result = string.Empty;
			if (ObscuredPrefs.HasKey(key))
			{
				result = ObscuredPrefs.GetString(key);
			}
			return result;
		}
	}
}
