using UnityEngine;

public class UIColorRenderer : MonoBehaviour
{
	public void OnColorChange()
	{
		base.GetComponent<Renderer>().sharedMaterial.color = UIColorPicker.current.value;
	}
}
