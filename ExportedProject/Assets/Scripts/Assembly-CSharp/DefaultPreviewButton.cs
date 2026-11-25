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
		if (base.audio == null)
		{
			base.gameObject.AddComponent<AudioSource>();
			base.audio.clip = sound;
			base.audio.Stop();
		}
		base.renderer.material = new Material(base.renderer.material);
	}

	public void DisabledButton()
	{
		if (disabledTexture != null)
		{
			base.renderer.material.mainTexture = disabledTexture;
		}
		IsDisabled = true;
	}

	public void EnabledButton()
	{
		if (disabledTexture != null)
		{
			base.renderer.material.mainTexture = normalTexture;
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
			base.audio.PlayOneShot(disabledsound);
			return;
		}
		base.audio.PlayOneShot(sound);
		base.renderer.material.mainTexture = pressedTexture;
		dispatch("click");
		CancelInvoke("OnTimeoutPress");
		Invoke("OnTimeoutPress", 0.1f);
	}

	private void OnTimeoutPress()
	{
		base.renderer.material.mainTexture = normalTexture;
	}
}
