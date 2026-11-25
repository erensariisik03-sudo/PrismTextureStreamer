using UnityEngine;

public class UIColorRenderer : MonoBehaviour
{
	public void OnColorChange()
	{
		base.renderer.sharedMaterial.color = UIColorPicker.current.value;
	}
}
