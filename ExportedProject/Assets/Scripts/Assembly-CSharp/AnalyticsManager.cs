using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
	public enum CategoryEvt
	{
		STORE = 0,
		GARAGE = 1,
		ACHIEVEMENT = 2,
		CAR = 3,
		COLLECTABLE = 4,
		CHALLENGE = 5,
		REWARDTRACKER = 6,
		NOTIFYEXP = 7,
		COMEBACK = 8,
		NONE = 9
	}

	public enum ActionEvt
	{
		STORE_VISIT = 0,
		GARAGE_VISIT = 1,
		ACHIEVEMENT = 2,
		CAR_DISTANCE = 3,
		CAR_COLLECTABLES = 4,
		CAR_BUY = 5,
		CAR_BUYPACK = 6,
		CAR_CHALLENGE = 7,
		CAR_REWARD = 8,
		COLLECTABLE = 9,
		CHALLENGE = 10,
		REWARD_TRACKER_VISIT = 11,
		RTCOLLECTABLES = 12,
		RTCHALLENGES = 13,
		NOTIFYEXP = 14,
		COMEBACK_EXP = 15,
		COMEBACK_NOEXP = 16,
		NONE = 17
	}

	public enum LabelEvt
	{
		CAR = 0,
		COLLECTABLE = 1,
		CHALLENGE_START = 2,
		CHALLENGE_FINISH = 3,
		RTCOLLECTABLES_ZONE = 4,
		RTCOLLECTABLES_TYPE = 5,
		RTCOLLECTABLES_COMPLETE = 6,
		RTCHALLENGE = 7,
		RTCHALLENGES_COMPLETE = 8,
		NONE = 9
	}

	public enum ValueEvt
	{
		CHALLENGE_MARK = 0,
		NONE = 1
	}

	public enum CrosspromoEvent
	{
		IMPRESION = 0,
		CLICK = 1
	}

	private const string EVENT_CATEGORY_CONNECTIONTEST = "Connection Test V2";

	private const string EVENT_ACTION_CONNECTIONTEST_NETWORKMODE = "NETWORK MODE";

	private const string EVENT_ACTION_CONNECTIONTEST_DEVICEPERFORMANCE = "DEVICE PERFORMANCE";

	private const string EVENT_ACTION_CONNECTIONTEST_QUALITYCONNECTION = "QUALITY CONNECTION";

	private const string EVENT_ACTION_CONNECTIONTEST_RESULT = "RESULT";

	private const string EVENT_LABEL_NETWORK_CONNECTIONTEST_WIFI = "WIFI";

	private const string EVENT_LABEL_NETWORK_CONNECTIONTEST_MOBILEDATA = "3G/4G";

	private const string EVENT_LABEL_NETWORK_CONNECTIONTEST_INADEQUATE = "INADEQUATE";

	private const string EVENT_LABEL_NETWORK_CONNECTIONTEST_FPS = "FPS";

	private const string EVENT_LABEL_NETWORK_CONNECTIONTEST_PING = "PING";

	private const string EVENT_LABEL_NETWORK_CONNECTIONTEST_PERFECTRESULT = "PERFECT";

	private const string EVENT_LABEL_NETWORK_CONNECTIONTEST_GOODRESULT = "GOOD";

	private const string EVENT_LABEL_NETWORK_CONNECTIONTEST_BADRESULT = "BAD";

	private const string EVENT_CATEGORY_GAME = "Game";

	private const string EVENT_CATEGORY_MODE = "Game Mode";

	private const string EVENT_CATEGORY_FACEBOOK = "Facebook";

	private const string EVENT_ACTION_START = "Start";

	private const string EVENT_ACTION_FINISHED = "Finished";

	private const string EVENT_ACTION_FACEBOOK_LOGIN = "Login";

	private const string EVENT_LABEL_FACEBOOK_LOGIN_START = "Login Start";

	private const string EVENT_LABEL_FACEBOOK_LOGIN_SUCCESS = "Login Success";

	private const string EVENT_LABEL_FACEBOOK_LOGIN_CANCEL = "Login Cancel";

	private const string EVENT_LABEL_FACEBOOK_LOGIN_FAILED = "Login Failed";

	private const string EVENT_CATEGORY_PRIVACY_POLICY = "PRIVACY POLICY";

	private const string EVENT_ACTION_SHOW_POLICY_BANNER = "SHOW POLICY BANNER";

	private const int INDEX_DIM_CONTROL = 1;

	private const int INDEX_DIM_PLATFORM = 2;

	public const string STORE_CAT = "Store";

	public const string GARAGE_CAT = "Garage";

	public const string ACHIEVEMENT_CAT = "Achievement";

	public const string CAR_CAT = "Car";

	public const string COLLECTABLE_CAT = "Collectable";

	public const string CHALLENGE_TYPE_CAT = "";

	public const string REWARDTRACKER_CAT = "Reward Tracker";

	public const string NOTIFYEXP_CAT = "Notify Exp";

	public const string COMEBACK_CAT = "Comeback";

	public const string ERROR_CAT = "CATEGORY_ERROR";

	public const string TRIAL_CAT = "Trial";

	public const string RADAR_CAT = "Radar";

	public const string CRASHABLE_CAT = "Destruction";

	public const string CROSSPROMOTION = "Crosspromotion";

	public const string STORE_VISIT_ACT = "Store Visit";

	public const string GARAGE_VISIT_ACT = "Garage Visit";

	public const string ACHIEVEMENT_ACT = "Achievement";

	public const string CAR_DISTANCE_ACT = "Car uDistance";

	public const string CAR_COLLECTABLE_ACT = "Car uCollectables";

	public const string CAR_BUY_ACT = "Car uBuy";

	public const string CAR_BUYPACK_ACT = "Car uBuyPack";

	public const string CAR_CHALLENGE_ACT = "Car uChallenge";

	public const string CAR_REWARD_ACT = "Car uReward";

	public const string COLLECTABLE_ACT = "Collectable";

	public const string CHALLENGE_ACT = "";

	public const string REWARDTRACKER_VISIT_ACT = "Reward Tracker Visit";

	public const string REWARDTRACKER_COLLECTABLES_ACT = "rtCollectables";

	public const string REWARDTRACKER_CHALLENGES_ACT = "rtChallenges";

	public const string NOTIFYEXP_ACT = "Notify Exp";

	public const string COMEBACK_NOEXP_ACT = "Comeback No Exp";

	public const string COMEBACK_EXP_ACT = "Comeback Exp";

	public const string ERROR_ACT = "ACTION_ERROR";

	public const string CROSSPROMO_VIEW_ACT = "Crosspromo viewed";

	public const string CROSSPROMO_ACT = "Crosspromo interaction";

	public const string DOWNLOADED_GAME_ACT = "Downloaded crosspromo game";

	public const string CAR_LABEL = "Car";

	public const string COLLECTABLE_LABEL = "Collectable";

	public const string CHALLENGE_LABEL = "";

	public const string CHALLENGE_START_LABEL = "Start";

	public const string CHALLENGE_FINISH_LABEL = "Finish";

	public const string RTCOLLECTABLES_ZONE_LABEL = "rtCollectables Zone";

	public const string RTCOLLECTABLES_TYPE_LABEL = "rtCollectables Type";

	public const string RTCOLLECTABLES_COMPLETE_LABEL = "rtCollectables Complete";

	public const string RTCHALLENGES_LABEL = "rtChallenges";

	public const string RTCHALLENGES_COMPLETE_LABEL = "rtChallenges Complete";

	public const string ERROR_LABEL = "LABEL_ERROR";

	public const string CHALLENGE_MARK_VALUE = "";

	public const string ERROR_VALUE = "VALUE_ERROR";

	private string platform;

	private float gameModeStartTime;

	public void NotifyConnectionTestRealised(byte networkMode, int frames, int ping, byte result)
	{
	}

	public void Init()
	{
		PopulateData();
		NotifyGameStarted();
	}

	private void OnApplicationQuit()
	{
		NotifyGameFinished();
	}

	private void NotifyGameStarted()
	{
	}

	private void NotifyGameFinished()
	{
	}

	public void NotifyGameModeStarted()
	{
	}

	public void NotifyGameModeFinished()
	{
	}

	public void NotifyFBLoginStarted()
	{
	}

	public void NotifyFBLoginSuccess()
	{
	}

	public void NotifyFBLoginCancel()
	{
	}

	public void NotifyFBLoginFailed()
	{
	}

	public void NotifyShowPolicyBanner(int timeZone, string country = "UNKNOWN")
	{
	}

	public void NotifyMoreGamesPressed()
	{
	}

	public void LogScreen(string screenName)
	{
	}

	public void ConfigureFields(string productName, string bundleId, string bundleVersion, string trackingCode)
	{
	}

	private void PopulateData()
	{
		platform = MySingleton<Manager>.Instance.platform.ToString();
	}

	public void NotifyZoneChange(string zoneNext)
	{
	}

	public void NotifyStoreEvent(ActionEvt _act)
	{
	}

	public void NotifyGarageEvent(ActionEvt _act)
	{
	}

	public void NotifyAchievementEvent(ActionEvt _act, string _actValue)
	{
	}

	public void NotifyUnlockCarEvent(ActionEvt _act, LabelEvt _lbl, string _lblValue)
	{
	}

	public void NotifyCollectableEvent(ActionEvt _act, string _actValue, LabelEvt _lbl, string _lblValue)
	{
	}

	public void NotifyChallengeEvent(ChallangeKey _challengeType, int _challengeIndex, LabelEvt _lbl, int _value = -1)
	{
	}

	public void NotifyRemoteConfigFailed()
	{
	}

	public void NotifyRewardTrackerEvents(ActionEvt _act, LabelEvt _lbl, string _lblValue = "")
	{
	}

	public void NotifyNotificationEvent(ActionEvt _act, string _actValue)
	{
	}

	public void NotifyComebackEvent(ActionEvt _act, string _lblValue = "")
	{
	}

	public void NotifyCrosspromotionEvent(CrosspromoEvent _act, string _lblValue, int _value = -1)
	{
	}
}
