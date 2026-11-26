using UnityEngine;

// Reklamsiz stub: tum public API korunur (sahne serilestirmesi + cagri noktalari icin).
public class Ads : MonoBehaviour
{
	[HideInInspector] public float lastTimeAds;
	[HideInInspector] public float timeFromLastPlay;
	public UIPanel PnAdsComing;
	public UILabel LbTime;
	[HideInInspector] public bool disabledAds = true;
	public Admob_Universal admobUniversal;
	public Heyzap_Controller heyzapController;
	public bool admobAdActive;

	private void Awake() { Object.DontDestroyOnLoad(gameObject); }

	public void Init() { }
	public void ShowAdmobOrHeyzap(bool fromChallangeCircuit, bool admob = true, bool enabledAdplacement = true) { }
	public void RestartCounter() { }
	public void CancelPnMsg() { }
	public void DisableAllAds() { }
	public void ShowBanner() { }
	public void HideBanner() { }
	public void CheckForTimeInterstitial() { }
	public void ResetOccurredEvents() { }
	public void ShowAdmobOrHeyzapAboutOccurredEvents() { }
	public void ShowAdsFromChallange() { }
	public void DisableAdsFromChallange() { }
	public void EnableAds() { }
}
