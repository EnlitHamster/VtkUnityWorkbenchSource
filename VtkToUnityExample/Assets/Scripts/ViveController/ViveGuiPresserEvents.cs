using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ViveGuiPresserEvents : MonoBehaviour {

    private SteamVR_TrackedObject _trackedObj;

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
		var guiPresser = GetComponent<ViveControllerToolGuiPresser>();
		GuiPointerInOut pointerInOut = null;
		if (guiPresser != null)
		{
			pointerInOut = guiPresser.PointerInOut;
		}
		//else
		//{
		//	var laserPointer = GetComponent<SteamVR_LaserPointer>();

		//	if (laserPointer != null)
		//	{
		//		pointerInOut = laserPointer.PointerInOut;
		//	}
		//}

		if (pointerInOut != null)
		{
			pointerInOut.PointerIn -= HandlePointerIn;
			pointerInOut.PointerIn += HandlePointerIn;
			pointerInOut.PointerOut -= HandlePointerOut;
			pointerInOut.PointerOut += HandlePointerOut;
		}

		var trackedController = GetComponent<SteamVR_TrackedController>();
        if (trackedController == null)
        {
			trackedController = gameObject.AddComponent<SteamVR_TrackedController>();
		}
		trackedController.TriggerClicked -= HandleTriggerClicked;
        trackedController.TriggerClicked += HandleTriggerClicked;

		//trackedController.TriggerUnclicked -= HandleTriggerUnclicked;
		//trackedController.TriggerUnclicked += HandleTriggerUnclicked;
	}


    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
			ExecuteEvents.Execute(
				EventSystem.current.currentSelectedGameObject,
				new PointerEventData(EventSystem.current),
				ExecuteEvents.submitHandler);

            //EventSystem.current.SetSelectedGameObject(null);
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


	private void HandlePointerIn(object sender, GuiPointerInOut.PointerEventArgs e)
    {

        if (e.target != null && e.target.gameObject != null)
        {
            //Debug.Log("HandlePointerIn", e.target.gameObject);
            ExecuteEvents.Execute(
				e.target.gameObject, 
				new PointerEventData(EventSystem.current), 
				ExecuteEvents.selectHandler);

            EventSystem.current.SetSelectedGameObject(e.target.gameObject);
        }
    }

    private void HandlePointerOut(object sender, GuiPointerInOut.PointerEventArgs e)
    {
        if (e.target != null && e.target.gameObject != null)
        {
            ExecuteEvents.Execute(
				e.target.gameObject, 
				new PointerEventData(EventSystem.current), 
				ExecuteEvents.cancelHandler);

            //Debug.Log("HandlePointerOut", e.target.gameObject);
        }

		EventSystem.current.SetSelectedGameObject(null);
	}
}
