using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameCounterText : MonoBehaviour
{
	private Text _textUi;
	private FrameAnimationControl _frameAnimationControl;
	private string _frameFormat = "00";

    // Start is called before the first frame update
    void Start()
    {
		_textUi = GetComponent<Text>();
		_frameAnimationControl = GetComponentInParent<FrameAnimationControl>();
		if (_frameAnimationControl.NFrames < 10)
		{
			_frameFormat = "0";
		}
		else if (_frameAnimationControl.NFrames > 99)
		{
			_frameFormat = "000";
		}
    }

	// Update is called once per frame
	void Update()
	{
		// because there is a delay between selecting and index and it being used,
		// I'm going to (expensively) update this every frame for now
		string dataStr =
			(_frameAnimationControl.IFrameSet + 1).ToString(_frameFormat) + "/" +
			_frameAnimationControl.NFrames.ToString(_frameFormat);
		_textUi.text = dataStr;
	}
}
