using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveControllerToolBase : MonoBehaviour {

    public GameObject DefaultIconPrefab;

    protected SteamVR_TrackedObject _trackedObj;

    protected GameObject _iconParent;
    protected GameObject _icon;
	protected int _iconCloneIndex = 0;

	protected bool _active;

	public virtual bool Busy()
	{
		return false;
	}

    protected SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)_trackedObj.index); }
    }

    void Awake()
    {
        _trackedObj = GetComponent<SteamVR_TrackedObject>();
        var iconParentTransform = _trackedObj.transform.Find("IconParent");
        if (iconParentTransform)
        {
            _iconParent = iconParentTransform.gameObject;
        }

        Activate();
    }

    // to be overridden by derived classes
    public virtual void Activate()
    {
        DeleteAllIconChildren();
        _icon = AddIconPrefab(DefaultIconPrefab);
		_icon.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        gameObject.layer = LayerMask.NameToLayer("Physics Controller");

		_active = true;
	}

    public virtual void DeActivate()
	{
		_active = false;
	}

    protected void DeleteAllIconChildren()
    {
        // destroy any existing children
        foreach (Transform child in _iconParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    protected GameObject AddIconPrefab(GameObject iconPrefab)
    {
        //DeleteAllIconChildren();

        if (_iconParent && iconPrefab)
        {
            var icon = Instantiate(iconPrefab);
            icon.transform.parent = _iconParent.transform;
            icon.transform.localPosition = Vector3.zero;

			icon.name = icon.name + " " + _iconCloneIndex.ToString();
			_iconCloneIndex += 1;

			return icon;
        }

        return null;
    }

	//   // Use this for initialization
	//   void Start () {

	//}

	public void OnTriggerEnter(Collider other)
	{
		if (!_active)
		{
			return;
		}

		OnTriggerEnterImpl(other);
	}

	protected virtual void OnTriggerEnterImpl(Collider other)
	{

	}

	public void OnTriggerStay(Collider other)
	{
		if (!_active)
		{
			return;
		}

		OnTriggerStayImpl(other);
	}

	protected virtual void OnTriggerStayImpl(Collider other)
	{

	}

	public void OnTriggerExit(Collider other)
	{
        // this is commented out because of #146
		//if (!_active)
		//{
		//	return;
		//}

		OnTriggerExitImpl(other);
	}

	protected virtual void OnTriggerExitImpl(Collider other)
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (!_active)
		{
			return;
		}

		UpdateImpl();
	}

	protected virtual void UpdateImpl()
	{

	}
}
