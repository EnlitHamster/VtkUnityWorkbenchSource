using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransferFunctionCounterText : MonoBehaviour
{
	private Text _textUi;
    // Start is called before the first frame update
    void Start()
    {
		_textUi = GetComponent<Text>();
		string dataStr = (1).ToString() + "/" + (1).ToString();
		_textUi.text = dataStr;
		//UpdateDataText();
    }

	// Update is called once per frame
	//void Update()
	//{

	//}

	public void SetCount(int i, int n)
	{
		string dataStr = (i + 1).ToString() + "/" + n.ToString();
		_textUi.text = dataStr;
	}

}
