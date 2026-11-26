using UnityEngine;

public class WorkshopDoorAudio : MonoBehaviour
{
	public AudioClip audioClip;

	private bool inited;

	private WorkshopDoorLogic workshopLogic;

	private void PlayOpen()
	{
		Init();
		if (workshopLogic.GetStatusBoolOpen())
		{
			PlayAudio();
		}
	}

	private void PlayClose()
	{
		Init();
		if (!workshopLogic.GetStatusBoolOpen())
		{
			PlayAudio();
		}
	}

	private void Init()
	{
		if (!inited)
		{
			base.gameObject.AddComponent<AudioSource>();
			base.GetComponent<AudioSource>().playOnAwake = false;
			base.GetComponent<AudioSource>().clip = audioClip;
			if (!workshopLogic)
			{
				workshopLogic = base.transform.parent.GetComponent<WorkshopDoorLogic>();
			}
			inited = true;
		}
	}

	private void PlayAudio()
	{
		base.GetComponent<AudioSource>().Play();
	}
}
