using UnityEngine;

public class SA_BackButton : DefaultPreviewButton
{
	public static string firstLevel = string.Empty;

	private void Start()
	{
		if (firstLevel != string.Empty)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		Object.DontDestroyOnLoad(base.gameObject);
		if (firstLevel == string.Empty)
		{
			firstLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
		}
	}

	private void FixedUpdate()
	{
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(firstLevel))
		{
			base.GetComponent<Renderer>().enabled = false;
			base.GetComponent<Collider>().enabled = false;
		}
		else
		{
			base.GetComponent<Renderer>().enabled = true;
			base.GetComponent<Collider>().enabled = true;
		}
	}

	protected override void OnClick()
	{
		base.OnClick();
		GoBack();
	}

	private void GoBack()
	{
		Singleton<SALevelLoader>.instance.LoadLevel(firstLevel);
	}
}
