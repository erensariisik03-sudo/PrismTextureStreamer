using UnityEngine;

// Reklamsiz stub: odullu video izlenmis sayilir, odul hemen verilir.
public class Heyzap_Controller : MonoBehaviour
{
	public bool currentVideoAvailableStatus = true;

	public void Init() { }
	public bool GetHeyzapInterstitialReady() { return false; }
	public void ShowHeyzapInterstitial() { }
	public bool IsVideoRewardAvailable() { return true; }

	public void ShowVideoReward(Task task)
	{
		task.SetProgress(1f);
		MySingleton<Manager>.Instance.guiCurrentLogic.OnVideoRewardsFinished(task);
	}

	public void Loadincentivizedvideo() { }
}
