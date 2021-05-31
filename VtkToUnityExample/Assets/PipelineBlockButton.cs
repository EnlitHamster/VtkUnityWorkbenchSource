using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipelineBlockButton : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        GetComponent<Image>().color = Color.gray;
    }

    public void OnTriggerExit(Collider other)
    {
        GetComponent<Image>().color = Color.white;
    }
}
