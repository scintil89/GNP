using UnityEngine;
using System.Collections;

public class KnightShaderCtrlScript : MonoBehaviour 
{
    public GameObject outline;

    void Awake ()
    {
        //외곽선 쉐이더 초기화
        outline.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //OutLine 제어
        if (gameObject.GetComponent<UnitScript>().isTouch)
        {
            outline.SetActive(true);
        }
        else
        {
            outline.SetActive(false);
        }
    }
}
