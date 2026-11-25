using UnityEngine;

public class CustomLeaderboardFiledsHolder : MonoBehaviour
{
	public TextMesh rank;

	public TextMesh score;

	public TextMesh playerId;

	public TextMesh playerName;

	public GameObject avatar;

	private void Awake()
	{
		avatar.renderer.material = new Material(avatar.renderer.material);
	}

	public void Disable()
	{
		rank.text = string.Empty;
		score.text = string.Empty;
		playerId.text = string.Empty;
		playerName.text = string.Empty;
		avatar.renderer.enabled = false;
	}
}
