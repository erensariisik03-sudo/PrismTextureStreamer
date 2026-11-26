using UnityEngine;

public class SALevelLoader : Singleton<SALevelLoader>
{
	private Texture2D bg;

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void LoadLevel(string name)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(name);
	}

	public void Restart()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
	}
}
