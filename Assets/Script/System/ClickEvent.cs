using UnityEngine;
using System.Collections;

/**
/// 레이어 정의
/// 5 : UI
/// 10 : MyLayer
/// 11 : EnemyLayer
**/

public class ClickEvent : MonoBehaviour
{
    public GameObject tempObject; //이전 클릭한 오브젝트의 클릭 체크를 해제하기 위해서 임시 저장.

    // Update is called once per frame
    void Update ()
    { 
        if (Input.GetMouseButtonDown(0)) //마우스 왼쪽 클릭
        {
            //Debug.Log("ClickEvent : Left Button Clicked");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitObj;

            if (Physics.Raycast(ray, out hitObj, 200.0f))
            {
                //Debug.Log(hitObj.transform.gameObject.name);

                if(tempObject) //탬프 오브젝트가 있는가?
                {
                    if (tempObject.GetComponent<UnitScript>().isTouch) //오브젝트가 현재 클릭중인가
                    {
                        //클릭 해제
                        tempObject.GetComponent<UnitScript>().isTouch = false;
                    }
                }

                switch (hitObj.transform.gameObject.layer) //레이어로 아군 / 적군 구별
                {
                   // case 5:
                   //     //Debug.Log(hitObj.transform.gameObject.name);
                   //     break;

                    case 10:
                        {
                            if( hitObj.transform.gameObject.GetComponent<UnitScript>() ) //클릭한것이 유닛이면
                            {
                                tempObject = hitObj.transform.gameObject; //새로 클릭한 오브젝트로 대체

                                hitObj.transform.gameObject.GetComponent<UnitScript>().isTouch = true;

                                if (tempObject)
                                {
                                    tempObject.GetComponent<UnitScript>().isTouch = true;
                                    tempObject = hitObj.transform.gameObject;
                                }
                            }
                        } 
                        break;

                    //case 11:
                    //    //Debug.Log(hitObj.transform.gameObject.name);
                    //    break;
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) //마우스 오른쪽 클릭
        {
            //Debug.Log("Right Button Clicked");

            //tempObject가 클릭중인지 검사
            if(tempObject)
                if( tempObject.GetComponent<UnitScript>().isTouch == true)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitObj;

                    if (Physics.Raycast(ray, out hitObj, 200.0f))
                    {
                        //Debug.Log(hitObj.transform.gameObject.name);

                        if (hitObj.collider.gameObject.layer == 11) //클릭한 오브젝트가 애너미인가
                        {
                            //Debug.Log(hitObj.transform.gameObject.name);

                            if (tempObject.GetComponent<UnitScript>())
                            {
                                tempObject.GetComponent<UnitScript>().Targeting(hitObj.transform.gameObject);
                            }
                        }
                    }
                }            
        }
    }
}
