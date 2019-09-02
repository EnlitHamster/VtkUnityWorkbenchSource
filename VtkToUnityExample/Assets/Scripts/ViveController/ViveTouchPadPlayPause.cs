using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ThreeDeeHeartPlugins;

public class ViveTouchPadPlayPause : MonoBehaviour {

	private VtkVolumeRenderLoadControl _volumeRenderAnimation;

	private SteamVR_TrackedObject trackedObj;

    public GameObject ToggleButton;

	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	// Use this for initialization
	IEnumerator Start ()
	{
		var sceneObject = GameObject.Find("Scene");

		if (null == sceneObject)
		{
			return null;
		}

		_volumeRenderAnimation = sceneObject.GetComponentInChildren<VtkVolumeRenderLoadControl>();

		return null;
	}

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
		{
            if (ToggleButton && ToggleButton.GetComponent<Toggle>())
            {
                ToggleButton.GetComponent<Toggle>().isOn = !ToggleButton.GetComponent<Toggle>().isOn;
            }
        }
	}
}
