using UnityEngine;

// Unity 6'da GUITexture yok; yalnizca derleme icin.
public class GUITexture : MonoBehaviour
{
	public Texture texture;
	public Color color = Color.white;
	public Rect pixelInset;
}
