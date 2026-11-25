using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

// CI tarafindan cagrilir: -executeMethod BuildScript.Build
public static class BuildScript
{
	// Unity Build Automation 'pre-export method' olarak da bunu kullanir.
	public static void Configure()
	{
		var target = NamedBuildTarget.Android;

		PlayerSettings.companyName = "Local";
		PlayerSettings.productName = "Extreme Car Driving";
		PlayerSettings.SetApplicationIdentifier(target, "com.local.extremecar");
		PlayerSettings.bundleVersion = "1.0.0";
		PlayerSettings.Android.bundleVersionCode = 1;

		// 64-bit (arm64) + IL2CPP
		PlayerSettings.SetScriptingBackend(target, ScriptingImplementation.IL2CPP);
		PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
		PlayerSettings.SetApiCompatibilityLevel(target, ApiCompatibilityLevel.NET_Unity_4_8);
		PlayerSettings.SetManagedStrippingLevel(target, ManagedStrippingLevel.Minimal);

		// SDK seviyeleri (36 enum'da yoksa sayisal cast ile)
		PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
		PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersions)36;

		// LAN multiplayer icin internet izni
		PlayerSettings.Android.forceInternetPermission = true;
		PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
		PlayerSettings.SetGraphicsAPIs(BuildTarget.Android,
			new[] { UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3 });
		PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;

		EditorUserBuildSettings.buildAppBundle = false;
		EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
	}

	public static void Build()
	{
		Configure();
		var scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
		Debug.Log("Sahne sayisi: " + scenes.Length);

		var opts = new BuildPlayerOptions
		{
			scenes = scenes,
			locationPathName = "build/Android/ExtremeCar.apk",
			target = BuildTarget.Android,
			options = BuildOptions.None
		};

		BuildReport report = BuildPipeline.BuildPlayer(opts);
		if (report.summary.result != BuildResult.Succeeded)
			throw new Exception("Build basarisiz: " + report.summary.result);
	}
}
