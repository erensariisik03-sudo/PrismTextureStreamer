using System;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

public class LocalNotificationManager : MonoBehaviour
{
	public enum LocalNotificationType
	{
		None = 0,
		RewardReady = 1
	}

	private const string KEY_LOCAL_NOTIFICATION_FLAG = "local_notification_active";

	private const string KEY_LOCAL_NOTIFICATION_DAYS_FLAG = "local_notification_days_active";

	private int flag
	{
		get
		{
			return ObscuredPrefs.GetInt("local_notification_active", -1);
		}
		set
		{
			ObscuredPrefs.SetInt("local_notification_active", value);
		}
	}

	private int lastDay
	{
		get
		{
			return ObscuredPrefs.GetInt("local_notification_days_active", -1);
		}
		set
		{
			ObscuredPrefs.SetInt("local_notification_days_active", value);
		}
	}

	private void Awake()
	{
		if (flag == -1)
		{
			if (Debug.isDebugBuild)
			{
				flag = 1;
			}
			else
			{
				DecideIfNotificationsAreActive();
			}
		}
	}

	private void Start()
	{
		if (flag == 0)
		{
			if (HasPassedAWeek())
			{
			}
			base.enabled = false;
			return;
		}
		if (NativeToolkit.WasLaunchedFromNotification() || HasPassedAWeek())
		{
		}
		ClearNotification(LocalNotificationType.RewardReady);
		CreateLocalNotification(LocalNotificationType.RewardReady);
	}

	private void DecideIfNotificationsAreActive()
	{
		if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
		{
			flag = 1;
		}
		else
		{
			flag = 0;
		}
	}

	public void CreateLocalNotification(LocalNotificationType type)
	{
		if (type != LocalNotificationType.None && type == LocalNotificationType.RewardReady)
		{
			CreateRewardReadyNotification();
		}
	}

	private void CreateRewardReadyNotification()
	{
		if (Debug.isDebugBuild)
		{
			NativeToolkit.ScheduleLocalNotification(Localization.Get("localnotification_rewardready_title"), string.Empty, GetIDByEnum(LocalNotificationType.RewardReady), 1, "default_sound", false, "small_logo", "large_logo");
		}
		else
		{
			NativeToolkit.ScheduleLocalNotification(Localization.Get("localnotification_rewardready_title"), string.Empty, GetIDByEnum(LocalNotificationType.RewardReady), 10080, "default_sound", false, "small_logo", "large_logo");
		}
	}

	private void ClearNotification(int id)
	{
		NativeToolkit.ClearLocalNotification(id);
	}

	private void ClearNotification(LocalNotificationType type)
	{
		NativeToolkit.ClearLocalNotification(GetIDByEnum(type));
	}

	private void ClearAllNotifications()
	{
		NativeToolkit.ClearAllLocalNotifications();
	}

	private int GetIDByEnum(LocalNotificationType type)
	{
		return (int)type;
	}

	private bool HasPassedAWeek()
	{
		bool result = false;
		if (lastDay != -1 && lastDay <= DateTime.Now.DayOfYear - 7)
		{
			result = true;
		}
		return result;
	}

	private void OnApplicationQuit()
	{
		lastDay = DateTime.Now.DayOfYear;
	}
}
