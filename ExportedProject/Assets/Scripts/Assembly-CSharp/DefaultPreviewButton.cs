using UnityEngine;

public class DefaultPreviewButton : EventDispatcher
{
	public Texture normalTexture;

	public Texture pressedTexture;

	public Texture disabledTexture;

	public AudioClip sound;

	public AudioClip disabledsound;

	private bool IsDisabled;

	public string text
	{
		get
		{
			TextMesh componentInChildren = base.gameObject.GetComponentInChildren<TextMesh>();
			return componentInChildren.text;
		}
		set
		{
			TextMesh[] componentsInChildren = base.gameObject.GetComponentsInChildren<TextMesh>();
			TextMesh[] array = componentsInChildren;
			foreach (TextMesh textMesh in array)
			{
				textMesh.text = value;
			}
		}
	}

	private void Awake()
	{
		if (base.GetComponent<AudioSource>() == null)
		{
			base.gameObject.AddComponent<AudioSource>();
			base.GetComponent<AudioSource>().clip = sound;
			base.GetComponent<AudioSource>().Stop();
		}
		base.GetComponent<Renderer>().material = new Material(base.GetComponent<Renderer>().material);
	}

	public void DisabledButton()
	{
		if (disabledTexture != null)
		{
			base.GetComponent<Renderer>().material.mainTexture = disabledTexture;
		}
		IsDisabled = true;
	}

	public void EnabledButton()
	{
		if (disabledTexture != null)
		{
			base.GetComponent<Renderer>().material.mainTexture = normalTexture;
		}
		IsDisabled = false;
	}

	private void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hitInfo, float.PositiveInfinity) && hitInfo.transform.gameObject == base.gameObject)
		{
			OnClick();
		}
	}

	protected virtual void OnClick()
	{
		if (IsDisabled)
		{
			base.GetComponent<AudioSource>().PlayOneShot(disabledsound);
			return;
		}
		base.GetComponent<AudioSource>().PlayOneShot(sound);
		base.GetComponent<Renderer>().material.mainTexture = pressedTexture;
		dispatch("click");
		CancelInvoke("OnTimeoutPress");
		Invoke("OnTimeoutPress", 0.1f);
	}

	private void OnTimeoutPress()
	{
		base.GetComponent<Renderer>().material.mainTexture = normalTexture;
	}
}
