using Amazon.Util.Storage.Internal;

namespace Amazon.Util.Internal.PlatformServices
{
	public class ApplicationSettings : IApplicationSettings
	{
		public void SetValue(string key, string value, ApplicationSettingsMode mode)
		{
			PlayerPreferenceKVStore playerPreferenceKVStore = new PlayerPreferenceKVStore();
			playerPreferenceKVStore.Put(key, value);
		}

		public string GetValue(string key, ApplicationSettingsMode mode)
		{
			PlayerPreferenceKVStore playerPreferenceKVStore = new PlayerPreferenceKVStore();
			return playerPreferenceKVStore.Get(key);
		}

		public void RemoveValue(string key, ApplicationSettingsMode mode)
		{
			PlayerPreferenceKVStore playerPreferenceKVStore = new PlayerPreferenceKVStore();
			playerPreferenceKVStore.Clear(key);
		}
	}
}
