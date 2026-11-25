using UnityEngine;

public class Semaphore : MonoBehaviour
{
	private const float TIME_FOR_UPDATE_TEXTURE_SEMAPHORE = 0.4f;

	public Transform Way;

	public Transform renderObject;

	public Texture redLight;

	public Texture greenLight;

	private void OnEnable()
	{
		InvokeRepeating("UpdateTexture", 0.4f, 0.4f);
	}

	private void OnDisable()
	{
		CancelInvoke("UpdateTexture");
	}

	private void UpdateTexture()
	{
		if (Way.GetComponent<TrafficWaypoint>().stop)
		{
			renderObject.renderer.material.mainTexture = redLight;
		}
		else
		{
			renderObject.renderer.material.mainTexture = greenLight;
		}
	}
}
