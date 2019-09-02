using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ViveControllerToolGuiPresser : ViveControllerToolBase {

	private GameObject _collidingObject;

	public GuiPointerInOut PointerInOut = new GuiPointerInOut();

	private void SetCollidingObject(Collider col, bool fromOnEnter = false)
    {
        GameObject colGameObject = col.gameObject;

        if (_collidingObject == colGameObject)
        {
            return;
        }

        // if we already have an object we've collided with
        if (_collidingObject)
        {
            // and we're not colliding with one of it's children
            if (!colGameObject.transform.IsChildOf(_collidingObject.transform))
            {
                return;
            }

			if (fromOnEnter)
			{
				SteamVR_Controller.Input((int)_trackedObj.index).TriggerHapticPulse(500);
			}
		}

		_collidingObject = colGameObject;

		GuiPointerInOut.PointerEventArgs argsIn = new GuiPointerInOut.PointerEventArgs();
		if (Controller != null)
		{
			argsIn.controllerIndex = Controller.index;
		}
		//argsIn.distance = hit.distance;
		argsIn.flags = 0;
		argsIn.distance = 0.0f;
		argsIn.target = _collidingObject.transform;
		PointerInOut.OnPointerIn(argsIn);

		if (fromOnEnter)
		{
			SteamVR_Controller.Input((int)_trackedObj.index).TriggerHapticPulse(500);
		}
	}

	protected override void OnTriggerEnterImpl(Collider other)
    {
        SetCollidingObject(other, true);
    }

	protected override void OnTriggerStayImpl(Collider other)
    {
        SetCollidingObject(other);
    }

    protected override void OnTriggerExitImpl(Collider other)
    {
        if (!_collidingObject)
        {
            return;
        }

		//if (!_objectInHand)
		{
			SteamVR_Controller.Input((int)_trackedObj.index).TriggerHapticPulse(500);
		}

		_collidingObject = null;

		GuiPointerInOut.PointerEventArgs argsOut = new GuiPointerInOut.PointerEventArgs();
		if (Controller != null)
		{
			argsOut.controllerIndex = Controller.index;
		}
		//argsIn.distance = hit.distance;
		argsOut.flags = 0;
		argsOut.distance = 0.0f;
		argsOut.target = null;
		PointerInOut.OnPointerOut(argsOut);
	}


	// Update is called once per frame
	//void Update()
 //   {

 //   }

}
