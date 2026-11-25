using System.Collections.Generic;
using UnityEngine;

public class LightmapsConfigurator : MonoBehaviour
{
	public bool dontNeedLightmapConfiguration;

	public Texture OriginDayLightmap;

	public Texture OriginNighLightmap;

	public Texture tempDayLightmap;

	public Texture tempNightLightmap;

	public string directory;

	public string sceneName;

	public List<Material> sceneMaterials;

	public List<Material> commonMaterials;

	public List<Renderer> modifiedRenderers;

	public new RealTimeLight light;

	public List<Material> allMaterials;

	private void Start()
	{
		light = Object.FindObjectOfType<RealTimeLight>();
		ReplaceMaterials();
		AddDNMaterials();
		light.SetDayNight(light.time);
	}

	private void OnDestroy()
	{
		RemoveDNMaterials();
	}

	public void ReplaceMaterials()
	{
		foreach (Renderer modifiedRenderer in modifiedRenderers)
		{
			string empty = string.Empty;
			string path = string.Empty;
			int num = modifiedRenderer.sharedMaterials.Length;
			Material[] array = new Material[num];
			for (int i = 0; i < num; i++)
			{
				if (modifiedRenderer.sharedMaterials[i].HasProperty("_Lightmap1") && modifiedRenderer.sharedMaterials[i].HasProperty("_Lightmap2"))
				{
					empty = modifiedRenderer.sharedMaterials[i].name;
					path = sceneName + "_Materials/" + empty + "_" + sceneName;
				}
				else if (modifiedRenderer.sharedMaterials[i].HasProperty("_DayNight"))
				{
					empty = modifiedRenderer.sharedMaterials[i].name;
					path = "Common_Materials/" + empty + "_Common";
				}
				Material material = (Material)Resources.Load(path);
				array[i] = material;
			}
			modifiedRenderer.sharedMaterials = array;
		}
	}

	public void AddDNMaterials()
	{
		light.AddDNMaterials(sceneMaterials, commonMaterials);
	}

	public void RemoveDNMaterials()
	{
		light.RemoveDNMaterials(sceneMaterials);
	}
}
