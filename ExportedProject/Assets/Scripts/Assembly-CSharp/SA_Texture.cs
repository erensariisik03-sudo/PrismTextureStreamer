using UnityEngine;

public class SA_Texture : MonoBehaviour
{
	public Texture texture
	{
		get
		{
			return base.renderer.material.mainTexture;
		}
		set
		{
			base.renderer.material.mainTexture = value;
		}
	}

	private void Awake()
	{
		base.renderer.material = new Material(base.renderer.material);
	}
}
