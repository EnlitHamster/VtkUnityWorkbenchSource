using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataCounterText : MonoBehaviour
{
	private Text _textUi;
    // Start is called before the first frame update
    void Start()
    {
		_textUi = GetComponent<Text>();
		UpdateCounterString();

	}

	// Update is called once per frame
	//void Update()
	//{

	//}

	public void UpdateCounterString()
	{
		string dataStr = (DataStore.Instance.IDataFolder + 1).ToString() +
			"/" + DataStore.Instance.NDataFolders.ToString();
		_textUi.text = dataStr;
	}
}
