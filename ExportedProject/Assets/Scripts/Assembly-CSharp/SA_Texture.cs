using UnityEngine;

public class SA_Texture : MonoBehaviour
{
	public Texture texture
	{
		get
		{
			return base.GetComponent<Renderer>().material.mainTexture;
		}
		set
		{
			base.GetComponent<Renderer>().material.mainTexture = value;
		}
	}

	private void Awake()
	{
		base.GetComponent<Renderer>().material = new Material(base.GetComponent<Renderer>().material);
	}
}
