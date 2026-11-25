using System;
using UnityEngine;

[Serializable]
[RequireComponent(typeof(WheelCollider))]
public class Wheel_0020Skidmarks : MonoBehaviour
{
	public GameObject skidCaller;

	public float startSlipValue;

	private Skidmarks skidmarks;

	private int lastSkidmark;

	private WheelCollider wheel_col;

	public Wheel_0020Skidmarks()
	{
		startSlipValue = 6.5f;
		lastSkidmark = -1;
	}

	public virtual void Start()
	{
		skidCaller = transform.root.gameObject;
		wheel_col = (WheelCollider)GetComponent(typeof(WheelCollider));
		if ((bool)(Skidmarks)UnityEngine.Object.FindObjectOfType(typeof(Skidmarks)))
		{
			skidmarks = (Skidmarks)UnityEngine.Object.FindObjectOfType(typeof(Skidmarks));
		}
		else
		{
			Debug.Log("No skidmarks object found. Skidmarks will not be drawn");
		}
	}

	public virtual void FixedUpdate()
	{
		WheelHit hit = default(WheelHit);
		wheel_col.GetGroundHit(out hit);
		float num = Mathf.Abs(hit.sidewaysSlip);
		if (!(num <= startSlipValue))
		{
			Vector3 pos = hit.point + 2f * skidCaller.rigidbody.velocity * Time.deltaTime;
			lastSkidmark = skidmarks.AddSkidMark(pos, hit.normal, num / 25f, lastSkidmark);
		}
		else
		{
			lastSkidmark = -1;
		}
	}

	public virtual void Main()
	{
	}
}
