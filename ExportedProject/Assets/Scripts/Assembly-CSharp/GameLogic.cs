using System.Collections;
using System.Collections.Generic;
using AxesInMotion.Network.Minigames;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
	private const float STEP_DECREMENT = 0.075f;

	private const float STEP_INCREMENT = 0.075f;

	private const float STEP_HANDBRAKE = 0.085f;

	public const float INGAME_MUSIC_MAX_VOLUME = 0.6f;

	public float farCamera = 300f;

	public float speedMS;

	public Transform mainArrowPointer;

	public Transform firstSpawnPoint;

	public Transform currentCar;

	public Camera currentCamera;

	public GameObject carsParent;

	public GameObject persistentCollectablesParent;

	public GameObject actualZoneCollectablesParent;

	public bool challangeInProgress;

	public CarSteeringWheel steer;

	public CarSound carSound;

	public CarControl carControl;

	public List<Billboard> flares;

	public GameObject positionLights;

	public bool lightsOn;

	[HideInInspector]
	public float gas;

	[HideInInspector]
	public float brakes;

	[HideInInspector]
	public float handbreak;

	public AudioSource musicAudioSource;

	public float dirLeft;

	[HideInInspector]
	public float dirRight;

	public GameObject trafficModeInstantiater;

	public InstanceTraffic instanceTraffic;

	public GameObject checkPointPrefab;

	[HideInInspector]
	public GUIGameLogic guiLogic;

	public LevelZone firstLevelZone;

	public LevelZone[] allZones;

	[HideInInspector]
	public InfoWorkshop[] allInfoWorkshop;

	public List<Transform> carComponents;

	public GameObject instantiatedCar;

	public GameObject rootLoading;

	public ChallangeModeManager challangeModeManager;

	public bool jump;

	private void InitializeNetworkGame()
	{
		MySingleton<Manager>.Instance.networkGame.InstantiatePlayerCar();
		StartCoroutine(DisableOfflineElements());
		StartCoroutine(InitGame());
		if (MySingleton<Manager>.Instance.networkGame.miniGameSelected != MiniGameLogic.MiniGameType.None)
		{
			StartCoroutine(InitializeMiniGame());
		}
	}

	private IEnumerator DisableOfflineElements()
	{
		yield return null;
		MySingleton<Manager>.Instance.networkGame.DisableOfflineElements();
	}

	private IEnumerator InitializeMiniGame()
	{
		yield return null;
		MySingleton<Manager>.Instance.networkGame.InitializeMiniGame();
	}

	private void Awake()
	{
		mainArrowPointer.gameObject.SetActive(false);
		rootLoading = Object.Instantiate(MySingleton<Manager>.Instance.UIRootLoading) as GameObject;
		carComponents = new List<Transform>();
		flares = new List<Billboard>();
		MySingleton<Manager>.Instance.gameLogic = this;
		InstantiateCar();
		musicAudioSource = base.gameObject.GetComponent<AudioSource>();
		musicAudioSource.volume = SettingsManager.GetMusicVolume();
		GetCarLightSystem();
	}

	private void Start()
	{
		if ((bool)MySingleton<Manager>.Instance.guiGameLogic && (bool)MySingleton<Manager>.Instance.guiGameLogic.exclusiveFreeElements && (bool)MySingleton<Manager>.Instance.guiGameLogic.exclusiveNetworkElements)
		{
			MySingleton<Manager>.Instance.guiGameLogic.exclusiveFreeElements.SetActive(true);
		}
		MySingleton<Manager>.Instance.stats.DistanceInitData();
		MySingleton<Manager>.Instance.taskManager.StartingSaveDataRewardsLoop();
		CarSettings component = currentCar.GetComponent<CarSettings>();
		component.abs = true;
		component.tc = true;
		component.esp = true;
		CarLightRay componentInChildren = currentCar.GetComponentInChildren<CarLightRay>();
		if ((bool)componentInChildren)
		{
			componentInChildren.enabled = true;
		}
		lightsOn = false;
	}

	private void OnEnable()
	{
		carComponents.Clear();
	}

	private void OnDisable()
	{
		if (!(MySingleton<Manager>.Instance == null))
		{
			if (MySingleton<Manager>.Instance.stats != null)
			{
				MySingleton<Manager>.Instance.stats.SaveDataIntoPlayerPrefs();
			}
			if (MySingleton<Manager>.Instance.taskManager != null)
			{
				MySingleton<Manager>.Instance.taskManager.CancelSaveDataRewardsLoop();
			}
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			MySingleton<Manager>.Instance.AdsEnded();
		}
	}

	public void InitLogic()
	{
		if (MySingleton<Manager>.Instance.gameMode == Manager.GameMode.Checkpoint)
		{
			checkPointPrefab = Object.Instantiate(MySingleton<Manager>.Instance.checkPointPrefab) as GameObject;
			ReloadCheckPoints();
		}
	}

	public void ReloadCheckPoints()
	{
		CheckpointLogic component = checkPointPrefab.GetComponent<CheckpointLogic>();
		Transform[] checkpointsSorted = component.checkpointsSorted;
		NGUITools.SetActive(checkpointsSorted[component.actualIndex].gameObject, false);
		NGUITools.SetActive(checkpointsSorted[MySingleton<Manager>.Instance.currentCheckPoint].gameObject, true);
		component.actualIndex = MySingleton<Manager>.Instance.currentCheckPoint;
	}

	public void SetUnits(SettingsManager.Units newUnits)
	{
		SettingsManager.SetUnits(newUnits);
	}

	public void Reset()
	{
		jump = true;
		currentCar.transform.position = new Vector3(currentCar.transform.position.x, currentCar.transform.position.y + 3f, currentCar.transform.position.z);
		currentCar.transform.localRotation = Quaternion.Euler(new Vector3(0f, currentCar.transform.localRotation.eulerAngles.y, 0f));
		CancelInvoke("ResetJumpFlag");
		Invoke("ResetJumpFlag", 5f);
	}

	private void ResetJumpFlag()
	{
		jump = false;
	}

	public bool ChangeAbs()
	{
		currentCar.GetComponent<CarSettings>().abs = !currentCar.GetComponent<CarSettings>().abs;
		return currentCar.GetComponent<CarSettings>().abs;
	}

	public bool ChangeEsp()
	{
		currentCar.GetComponent<CarSettings>().esp = !currentCar.GetComponent<CarSettings>().esp;
		return currentCar.GetComponent<CarSettings>().esp;
	}

	public bool ChangeTc()
	{
		currentCar.GetComponent<CarSettings>().tc = !currentCar.GetComponent<CarSettings>().tc;
		return currentCar.GetComponent<CarSettings>().tc;
	}

	public void NextCamera()
	{
		currentCar.GetComponent<CarCameras>().Next();
	}

	public void Repair()
	{
		currentCar.GetComponent<CarDamage>().DoBounce();
	}

	public void Gas(bool isPressed)
	{
		if (isPressed)
		{
			CancelInvoke("DecrementGas");
			InvokeRepeating("IncrementGas", 0f, 0.1f);
		}
		else
		{
			CancelInvoke("IncrementGas");
			InvokeRepeating("DecrementGas", 0f, 0.1f);
		}
	}

	public void Brake(bool isPressed)
	{
		if (isPressed)
		{
			CancelInvoke("DecrementBrake");
			InvokeRepeating("IncrementBrake", 0f, 0.1f);
		}
		else
		{
			CancelInvoke("IncrementBrake");
			InvokeRepeating("DecrementBrake", 0f, 0.1f);
		}
	}

	public void Handbrake(bool isPressed)
	{
		if (isPressed)
		{
			CancelInvoke("DecrementHandbrake");
			InvokeRepeating("IncrementHandbrake", 0f, 0.1f);
		}
		else
		{
			CancelInvoke("IncrementHandbrake");
			InvokeRepeating("DecrementHandbrake", 0f, 0.1f);
		}
	}

	public void DirLeft(bool isPressed)
	{
		if (isPressed)
		{
			CancelInvoke("DecrementDirLeft");
			InvokeRepeating("IncrementDirLeft", 0f, 0.1f);
		}
		else
		{
			CancelInvoke("IncrementDirLeft");
			InvokeRepeating("DecrementDirLeft", 0f, 0.1f);
		}
	}

	public void DirRight(bool isPressed)
	{
		if (isPressed)
		{
			CancelInvoke("DecrementDirRight");
			InvokeRepeating("IncrementDirRight", 0f, 0.1f);
		}
		else
		{
			CancelInvoke("IncrementDirRight");
			InvokeRepeating("DecrementDirRight", 0f, 0.1f);
		}
	}

	private void IncrementGas()
	{
		gas += 0.075f;
		if (gas >= 1f)
		{
			gas = 1f;
			CancelInvoke("IncrementGas");
		}
	}

	private void IncrementBrake()
	{
		brakes -= 0.075f;
		if (brakes <= -1f)
		{
			brakes = -1f;
			CancelInvoke("IncrementBrake");
		}
	}

	private void DecrementGas()
	{
		gas -= 0.075f;
		if (gas <= 0f)
		{
			gas = 0f;
			CancelInvoke("DecrementGas");
		}
	}

	private void DecrementBrake()
	{
		brakes += 0.075f;
		if (brakes >= 0f)
		{
			brakes = 0f;
			CancelInvoke("DecrementBrake");
		}
	}

	private void IncrementHandbrake()
	{
		handbreak += 0.085f;
		if (handbreak >= 1f)
		{
			handbreak = 1f;
			CancelInvoke("IncrementHandbrake");
		}
	}

	private void DecrementHandbrake()
	{
		handbreak -= 0.085f;
		if (handbreak <= 0f)
		{
			handbreak = 0f;
			CancelInvoke("DecrementHandbrake");
		}
	}

	private void IncrementDirLeft()
	{
		dirLeft -= 0.075f;
		if (dirLeft <= -1f)
		{
			dirLeft = -1f;
			CancelInvoke("IncrementDirLeft");
		}
	}

	private void DecrementDirLeft()
	{
		dirLeft += 0.075f;
		if (dirLeft >= 0f)
		{
			dirLeft = 0f;
			CancelInvoke("DecrementDirLeft");
		}
	}

	private void IncrementDirRight()
	{
		dirRight += 0.075f;
		if (dirRight >= 1f)
		{
			dirRight = 1f;
			CancelInvoke("IncrementDirRight");
		}
	}

	private void DecrementDirRight()
	{
		dirRight -= 0.075f;
		if (dirRight <= 0f)
		{
			dirRight = 0f;
			CancelInvoke("DecrementDirRight");
		}
	}

	public void ResetCarInput()
	{
		gas = 0f;
		brakes = 0f;
		dirRight = 0f;
		dirLeft = 0f;
		CancelInvoke();
	}

	public int GetSpeed()
	{
		speedMS = Vector3.Dot(carControl.rigidbody.velocity, carControl.transform.forward);
		if (MySingleton<Manager>.Instance.units == SettingsManager.Units.KMH)
		{
			return (int)Mathf.Abs(speedMS * 3.6f * 2f);
		}
		return (int)Mathf.Abs(speedMS * 2.237f * 2f);
	}

	public float GetSpeedMS(Vector3 velocity)
	{
		return Vector3.Dot(velocity, carControl.transform.forward);
	}

	public string GetGear()
	{
		int gearInput = carControl.gearInput;
		if (gearInput > 0)
		{
			int currentGear = carSound.currentGear;
			return Mathf.Clamp(currentGear, 1, carSound.gearCount).ToString();
		}
		return (gearInput >= 0) ? "N" : "R";
	}

	public float GetRevs01Range()
	{
		float engineRPM = carSound.engineRPM;
		float engineGearUpRPM = carSound.engineGearUpRPM;
		float num = carSound.engineIdleRPM - 500f;
		return (engineRPM - num) / (engineGearUpRPM - num);
	}

	public void Driving(float deltaMeters)
	{
		MySingleton<Manager>.Instance.stats.IncrementMetersDrove(deltaMeters);
		MySingleton<Manager>.Instance.taskManager.taskDistance1.AddProgress(deltaMeters);
	}

	public void GoToWorkshop(InfoWorkshop infoWorkshop)
	{
		guiLogic.ShowLoading();
		MySingleton<Manager>.Instance.currentWorkshopId = infoWorkshop.Id;
		if ((bool)MySingleton<Manager>.Instance.levelZone)
		{
			MySingleton<Manager>.Instance.currentLevelZoneName = MySingleton<Manager>.Instance.levelZone.nameSceneZone;
		}
		Application.LoadLevel(3);
	}

	public void InstantiateCar()
	{
		InfoCar infoCar = MySingleton<Manager>.Instance.carsInfoPrefabs[MySingleton<Manager>.Instance.GetIndexArrayPosFromId(MySingleton<Manager>.Instance.indexCurrentCar)];
		instantiatedCar = Object.Instantiate(Resources.Load(infoCar.idName) as GameObject) as GameObject;
		InfoCar infoCar2 = instantiatedCar.AddMissingComponent<InfoCar>();
		infoCar2.Initialize(infoCar);
		currentCar = instantiatedCar.transform;
		carControl = currentCar.GetComponent<CarControl>();
		carSound = currentCar.GetComponent<CarSound>();
		instantiatedCar.SetActive(false);
		carComponents = new List<Transform>();
		carControl.FindGameLogic();
		StartCoroutine(InitGame());
	}

	private void ActiveCarComponents(Transform car)
	{
		foreach (Transform item in car)
		{
			if (!item.gameObject.activeSelf && item.gameObject.tag != "InstantiateAI" && item.gameObject.tag != "FrontLight")
			{
				item.gameObject.SetActive(true);
				carComponents.Add(item);
			}
			else if (item.gameObject.tag == "InstantiateAI")
			{
				trafficModeInstantiater = item.gameObject;
			}
			ActiveCarComponents(item);
		}
		instantiatedCar.GetComponent<CarVisuals>().shadow.SetActive(true);
	}

	private IEnumerator InitGame()
	{
		ActiveCarComponents(instantiatedCar.transform);
		if (!MySingleton<Manager>.Instance.returningFromWorkshop)
		{
			if (firstLevelZone != null)
			{
				firstLevelZone.ForceLoadLevel();
			}
			if (MySingleton<Manager>.Instance.gameMode != Manager.GameMode.Multiplayer)
			{
				instantiatedCar.transform.position = firstSpawnPoint.transform.position;
				instantiatedCar.transform.rotation = firstSpawnPoint.transform.rotation;
			}
		}
		else
		{
			MySingleton<Manager>.Instance.returningFromWorkshop = false;
			if ((allZones != null) & (allZones.Length > 0))
			{
				LevelZone[] array = allZones;
				foreach (LevelZone zone in array)
				{
					if (!(zone.nameSceneZone == MySingleton<Manager>.Instance.currentLevelZoneName))
					{
						continue;
					}
					zone.ForceLoadLevel();
					yield return null;
					InfoWorkshop[] allInfoWorkshop = GameObject.Find(zone.nameSceneZone).GetComponent<InfoZone>().infoWorkshopArray;
					InfoWorkshop[] array2 = allInfoWorkshop;
					foreach (InfoWorkshop workshop in array2)
					{
						if (workshop.Id == MySingleton<Manager>.Instance.currentWorkshopId)
						{
							Transform spawnPoint = workshop.gameObject.GetComponent<WorkshopDoorLogic>().spawnPoint;
							instantiatedCar.transform.position = spawnPoint.position;
							instantiatedCar.transform.rotation = spawnPoint.rotation;
						}
					}
				}
			}
			else
			{
				InfoWorkshop[] allInfoWorkshop2 = GameObject.Find("2Game_Persistent_Additive").GetComponent<InfoZone>().infoWorkshopArray;
				InfoWorkshop[] array3 = allInfoWorkshop2;
				foreach (InfoWorkshop workshop2 in array3)
				{
					if (workshop2.Id == MySingleton<Manager>.Instance.currentWorkshopId)
					{
						Transform spawnPoint2 = workshop2.gameObject.GetComponent<WorkshopDoorLogic>().spawnPoint;
						instantiatedCar.transform.position = spawnPoint2.position;
						instantiatedCar.transform.rotation = spawnPoint2.rotation;
					}
				}
			}
		}
		GameObject trafficDetector = new GameObject("Traffic Detector");
		trafficDetector.transform.position = instantiatedCar.transform.position;
		trafficDetector.transform.SetParent(instantiatedCar.transform);
		trafficDetector.AddComponent<TestSwitcher>();
		rootLoading.SetActive(false);
		instantiatedCar.GetComponent<CarLogic>().LockXZMovements(true);
		instantiatedCar.GetComponent<CarLogic>().LockXZMovements(false);
		CarCameras camerasScript = instantiatedCar.GetComponent<CarCameras>();
		camerasScript.FixedCameras[0] = Camera.main;
		currentCamera = Camera.main;
		camerasScript.enabled = true;
		InitLogic();
		instantiatedCar.GetComponent<CarControl>().enabled = true;
		instantiatedCar.GetComponent<CarLogic>().enabled = true;
		instantiatedCar.GetComponent<CarExternalInputRandom>().enabled = true;
		instantiatedCar.GetComponent<CarSound>().enabled = true;
		instantiatedCar.GetComponent<CarCameras>().enabled = true;
		instantiatedCar.GetComponent<CarVisuals>().enabled = true;
		instantiatedCar.AddMissingComponent<Lifeguard>();
		instantiatedCar.SetActive(true);
	}

	public void PlayAudioCollectableCollected()
	{
		StartCoroutine(PlayAudioCollectable());
	}

	public void PlayHorn(bool isPressed)
	{
		if (isPressed)
		{
			if (!carSound.horn.isPlaying)
			{
				carSound.horn.Play();
			}
		}
		else
		{
			carSound.horn.Stop();
		}
	}

	private IEnumerator PlayAudioCollectable()
	{
		GameObject go = new GameObject();
		go.transform.parent = base.transform;
		AudioClip audioCollectable = Resources.Load("collectable_collected") as AudioClip;
		AudioSource audioSource = go.AddMissingComponent<AudioSource>();
		audioSource.playOnAwake = false;
		audioSource.clip = audioCollectable;
		audioSource.Play();
		yield return null;
		while (audioSource.isPlaying)
		{
			yield return new WaitForSeconds(0.3f);
		}
		Object.Destroy(go);
		yield return null;
		Resources.UnloadUnusedAssets();
		yield return null;
	}

	private void GetCarLightSystem()
	{
		if (!(instantiatedCar != null))
		{
			return;
		}
		CarVisuals component = instantiatedCar.GetComponent<CarVisuals>();
		if (component.PositionLights != null && component.Flares != null)
		{
			FrontFlares component2 = component.Flares.GetComponent<FrontFlares>();
			if (component2 != null && component2.flares.Length > 0)
			{
				flares.Clear();
				flares.AddRange(component2.flares);
				positionLights = component.PositionLights;
			}
		}
		else
		{
			Debug.LogWarning("Este coche no tiene luces de posicion!!");
		}
	}

	public void SwitchCarLights()
	{
		GetCarLightSystem();
		CarVisuals component = instantiatedCar.GetComponent<CarVisuals>();
		if (component.LightProjectors != null)
		{
			if (!lightsOn)
			{
				if ((bool)component.LightProjectors)
				{
					component.LightProjectors.SetActive(true);
				}
			}
			else if ((bool)component.LightProjectors)
			{
				component.LightProjectors.SetActive(false);
			}
		}
		if ((bool)positionLights && flares.Count == 2)
		{
			for (int i = 0; i < 2; i++)
			{
				if (!lightsOn)
				{
					positionLights.SetActive(true);
					if (flares[i] != null)
					{
						flares[i].targetScale = 1f;
					}
				}
				else
				{
					positionLights.SetActive(false);
					if (flares[i] != null)
					{
						flares[i].targetScale = 0f;
					}
				}
			}
		}
		if (lightsOn)
		{
			lightsOn = false;
		}
		else
		{
			lightsOn = true;
		}
	}

	public void ForceSwitchCarLights()
	{
		if (!SettingsManager.GetStarterPackBoughtOrEnded() && !lightsOn)
		{
			SwitchCarLights();
		}
	}

	public void VolumeChanged(float value)
	{
		float num = Mathf.Lerp(0f, 0.6f, value);
		SettingsManager.SetMusicVolume(value);
	}

	public void InstantiateCar(InfoCar info, VehicleCamera vehicleCamera)
	{
		DestroyInstantiatedCar();
		instantiatedCar = Object.Instantiate(Resources.Load(info.idName) as GameObject) as GameObject;
		ActiveCarComponents(instantiatedCar.transform);
		InfoCar infoCar = instantiatedCar.AddMissingComponent<InfoCar>();
		infoCar.Initialize(info);
		currentCar = instantiatedCar.transform;
		carControl = currentCar.GetComponent<CarControl>();
		carSound = currentCar.GetComponent<CarSound>();
		CarCameras component = instantiatedCar.GetComponent<CarCameras>();
		component.FixedCameras[0] = vehicleCamera.GetComponent<Camera>();
		vehicleCamera.ChangeTarget(currentCar);
		PrepareInstantiatedCar();
	}

	public void PrepareInstantiatedCar()
	{
		instantiatedCar.SetActive(false);
		carControl.FindGameLogic();
		foreach (Transform item in instantiatedCar.transform)
		{
			if (!item.gameObject.activeSelf && item.gameObject.tag != "InstantiateAI" && item.gameObject.tag != "FrontLight")
			{
				item.gameObject.SetActive(true);
			}
			else if (item.gameObject.tag == "InstantiateAI")
			{
				trafficModeInstantiater = item.gameObject;
			}
		}
		CarLogic component = instantiatedCar.GetComponent<CarLogic>();
		component.LockXZMovements(true);
		component.LockXZMovements(false);
		carControl.enabled = true;
		component.enabled = true;
		instantiatedCar.GetComponent<CarExternalInputRandom>().enabled = true;
		instantiatedCar.GetComponent<CarSound>().enabled = true;
		instantiatedCar.GetComponent<CarCameras>().enabled = true;
		CarVisuals component2 = instantiatedCar.GetComponent<CarVisuals>();
		component2.enabled = true;
		component2.shadow.SetActive(true);
		instantiatedCar.AddMissingComponent<Lifeguard>();
		instantiatedCar.SetActive(true);
	}

	public void InstantiateGameCar(VehicleCamera vehicleCamera)
	{
		DestroyInstantiatedCar();
		InfoCar info = MySingleton<Manager>.Instance.carsInfoPrefabs[MySingleton<Manager>.Instance.GetIndexArrayPosFromId(MySingleton<Manager>.Instance.indexCurrentCar)];
		InstantiateCar(info, vehicleCamera);
	}

	private void DestroyInstantiatedCar()
	{
		ResetCarInput();
		if (instantiatedCar != null)
		{
			instantiatedCar.SetActive(false);
		}
		Object.Destroy(instantiatedCar);
		Resources.UnloadUnusedAssets();
		instantiatedCar = null;
	}

	public void OnAIMPlayerConnected(NetworkPlayer networkPlayer)
	{
		if (MySingleton<Manager>.Instance.selectedGame == Manager.SelectedGame.MultiplayerDriving)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i <= MySingleton<Manager>.Instance.networkManager.playersCount; i++)
		{
			if (!MySingleton<Manager>.Instance.networkFriends.IsFriend(networkPlayer))
			{
				continue;
			}
			num++;
			if (num >= 1)
			{
				MySingleton<AchievementManager>.Instance.ApplyAchivement(AchievementManager.Achievement.MULTIPLAYER_PLAY_FACEBOOK_FRIEND);
				MySingleton<Manager>.Instance.guiGameLogic.ActiveAchievementPanel(true);
				if (num >= MySingleton<Manager>.Instance.connection.maxPlayersPerRoom)
				{
					MySingleton<AchievementManager>.Instance.ApplyAchivement(AchievementManager.Achievement.MULTIPLAYER_PLAY_FACEBOOK_FRIEND_FULL_ROOM);
					MySingleton<Manager>.Instance.guiGameLogic.ActiveAchievementPanel(true);
				}
			}
		}
	}
}
