using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ThreeDeeHeartPlugins;

public class ViveTouchPadWwwl : MonoBehaviour {

	private VtkVolumeRenderLoadControl _volumeRenderGain;

	[Range(25.0f, 100.0f)]
	public float TouchpadSensitivity = 50.0f;

	private Vector2 _lastTouchPadPosition;
	private bool _touchpadDown = false;

	private SteamVR_TrackedObject trackedObj;

	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	public static float Clamp( float value, float min, float max )
	{
		return (value < min) ? min : (value > max) ? max : value;
	}

	// Use this for initialization
	IEnumerator Start()
	{
		var sceneObject = GameObject.Find("Scene");

		if (null == sceneObject)
		{
			return null;
		}

		_volumeRenderGain = sceneObject.GetComponentInChildren<VtkVolumeRenderLoadControl>();

		return null;
	}

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (null == _volumeRenderGain)
		{
			return;
		}

		if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
		{
			_touchpadDown = true;
			_lastTouchPadPosition = 
				(Controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));
		}
		else if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
		{
			_touchpadDown = false;
		}
		else if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad) && _touchpadDown)
		{
			Vector2 touchpadPosition = (Controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

			_volumeRenderGain.ChangeWindowLevel(
				(touchpadPosition.y - _lastTouchPadPosition.y) * TouchpadSensitivity);
			_volumeRenderGain.ChangeWindowWidth(
				(touchpadPosition.x - _lastTouchPadPosition.x) * TouchpadSensitivity);

			_lastTouchPadPosition = touchpadPosition;
		}

	}

	void OnGUI()
	{
		if (null == _volumeRenderGain)
		{
			return;
		}

		GUI.Label(
			new Rect(0, 0, 200, 50), 
			"WLWW: " + (int)_volumeRenderGain.VolumeWindowLevel + 
			", " + (int)_volumeRenderGain.VolumeWindowWidth);
	}
}
