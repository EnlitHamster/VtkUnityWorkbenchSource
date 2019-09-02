using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LaserEvents : MonoBehaviour {

    private SteamVR_TrackedObject _trackedObj;
    private SteamVR_LaserPointer laserPointer;
    private SteamVR_TrackedController trackedController;

    // Use this for initialization
    void Awake()
    {
        _trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnEnable()
    {
        laserPointer = GetComponent<SteamVR_LaserPointer>();
        laserPointer.PointerIn -= HandlePointerIn;
        laserPointer.PointerIn += HandlePointerIn;
        laserPointer.PointerOut -= HandlePointerOut;
        laserPointer.PointerOut += HandlePointerOut;

        trackedController = GetComponent<SteamVR_TrackedController>();
        if (trackedController == null)
        {
            trackedController = GetComponentInParent<SteamVR_TrackedController>();
            trackedController.controllerIndex = (uint)_trackedObj.index;
        }
        trackedController.TriggerClicked -= HandleTriggerClicked;
        trackedController.TriggerClicked += HandleTriggerClicked;

        trackedController.TriggerUnclicked -= HandleTriggerUnclicked;
        trackedController.TriggerUnclicked += HandleTriggerUnclicked;
    }


    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            ExecuteEvents.Execute(
				EventSystem.current.currentSelectedGameObject, 
				new PointerEventData(EventSystem.current), 
				ExecuteEvents.submitHandler);

            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void HandleTriggerUnclicked(object sender, ClickedEventArgs e)
    {
        ExecuteEvents.Execute(
			EventSystem.current.currentSelectedGameObject, 
			new PointerEventData(EventSystem.current), 
			ExecuteEvents.submitHandler);

        EventSystem.current.SetSelectedGameObject(null);
    }


    private void HandlePointerIn(object sender, PointerEventArgs e)
    {

        if (e.target.gameObject != null)
        {
            //Debug.Log("HandlePointerIn", e.target.gameObject);
            ExecuteEvents.Execute(
				e.target.gameObject, 
				new PointerEventData(EventSystem.current), 
				ExecuteEvents.selectHandler);

            EventSystem.current.SetSelectedGameObject(e.target.gameObject);
        }
    }

    private void HandlePointerOut(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject != null)
        {
            ExecuteEvents.Execute(
				e.target.gameObject, 
				new PointerEventData(EventSystem.current), 
				ExecuteEvents.cancelHandler);

            EventSystem.current.SetSelectedGameObject(null);
            //Debug.Log("HandlePointerOut", e.target.gameObject);
        }
    }
}
