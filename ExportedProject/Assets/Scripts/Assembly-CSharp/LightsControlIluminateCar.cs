using UnityEngine;

public class LightsControlIluminateCar : MonoBehaviour
{
	private int layerMask = LayerMask.NameToLayer("Car");

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider myTrigger)
	{
		if (myTrigger.gameObject.layer == layerMask)
		{
			Transform parent = myTrigger.transform.parent;
			if (parent != null)
			{
				parent = parent.transform.parent;
				if (parent.gameObject.layer != layerMask)
				{
				}
			}
		}
		Debug.Log("Yeeeee");
	}

	private void OnTriggerExit(Collider myTrigger)
	{
		if (myTrigger.gameObject.layer != layerMask)
		{
			return;
		}
		Transform parent = myTrigger.transform.parent;
		if (parent != null)
		{
			parent = parent.transform.parent;
			if (parent.gameObject.layer != layerMask)
			{
			}
		}
	}
}
