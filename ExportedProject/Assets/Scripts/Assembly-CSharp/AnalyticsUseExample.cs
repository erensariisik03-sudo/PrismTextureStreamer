using UnityEngine;

public class AnalyticsUseExample : MonoBehaviour
{
	private void Awake()
	{
		Singleton<AndroidGoogleAnalytics>.instance.StartTracking();
	}

	private void Start()
	{
		Singleton<AndroidGoogleAnalytics>.instance.SendView("Home Screen");
		Singleton<AndroidGoogleAnalytics>.instance.SendEvent("Category", "Action", "label");
		Singleton<AndroidGoogleAnalytics>.instance.SendEvent("Category", "Action", "label", 100L, "screen", "main");
		Singleton<AndroidGoogleAnalytics>.instance.SendTiming("App Started", (long)Time.time);
		Singleton<AndroidGoogleAnalytics>.instance.SetKey("SCREEN", "MAIN");
		PurchaseTackingExample();
	}

	public void PurchaseTackingExample()
	{
		Singleton<AndroidGoogleAnalytics>.instance.CreateTransaction("0_123456", "In-app Store", 2.1f, 0.17f, 0f, "USD");
		Singleton<AndroidGoogleAnalytics>.instance.CreateItem("0_123456", "Level Pack: Space", "L_789", "Game expansions", 1.99f, 1, "USD");
	}
}
