using System.IO;
using UnityEngine;

public class PassthroughManager
{
	private static OVRPassthroughLayer _passthroughLayer;
	private static Camera _centerEyeCamera;

	public static void SetPassthroughLayer(OVRPassthroughLayer oVRPassthroughLayer)
	{
		_passthroughLayer = oVRPassthroughLayer;
	}

	public static void SetCamera(Camera camera)
	{
		_centerEyeCamera = camera;
	}

	public static void EnablePassthrough()
	{
		_passthroughLayer.enabled = true;
		_centerEyeCamera.clearFlags = CameraClearFlags.SolidColor;

		DataManager.gameSettings.immersiveSettings.isImmersive = false;
		DataManager.SaveSettings();
	}

	public static void DisablePassthrough()
	{
		_passthroughLayer.enabled = false;
		_centerEyeCamera.clearFlags = CameraClearFlags.Skybox;

		DataManager.gameSettings.immersiveSettings.isImmersive = true;
		DataManager.SaveSettings();
	}
}