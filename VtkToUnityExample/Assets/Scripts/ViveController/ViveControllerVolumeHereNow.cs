using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveControllerVolumeHereNow : MonoBehaviour
{
	public  GameObject ObjectOfInterest;
    private SteamVR_TrackedObject trackedObj;


	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	void Start()
	{
		DataStore.Instance.ApplyPositonRotationY(ObjectOfInterest);
	}

	// Update is called once per frame
	void Update () {

		if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
		{
			DataStore.Instance.StorePositionRotation(trackedObj.gameObject);
			DataStore.Instance.ApplyPositonRotationY(ObjectOfInterest);
		}
	}
}
