using UnityEngine;
using System.Collections;

public class MageShaderCtrlScript : MonoBehaviour
{
    public GameObject outline1; //유닛 자체의 아웃라인
    public GameObject outline2; //유닛이 들고 있는 지팡이의 아웃라인

    void Awake()
    {
        //외곽선 쉐이더 초기화
        outline1.SetActive(false);
        outline2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //OutLine 제어
        if (gameObject.GetComponent<UnitScript>().isTouch)
        {
            outline1.SetActive(true);
            outline2.SetActive(true);
        }
        else
        {
            outline1.SetActive(false);
            outline2.SetActive(false);
        }
    }
}
