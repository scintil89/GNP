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
            Debug.Log("Left Button Clicked");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitObj;

            if (Physics.Raycast(ray, out hitObj, 200.0f))
            {
                //Debug.Log(hitObj.transform.gameObject.name);

                if(tempObject) //탬프 오브젝트가 있는가?
                {
                    if (tempObject.GetComponent<UnitScript>().isTouching() == true) //오브젝트가 현재 클릭중인가
                    {
                        //클릭 해제
                        tempObject.GetComponent<UnitScript>().UnTouching();
                    }
                }

                switch (hitObj.transform.gameObject.layer) //레이어로 아군 / 적군 구별
                {
                   // case 5:
                   //     //Debug.Log(hitObj.transform.gameObject.name);
                   //     break;

                    case 10:
                        {
                            tempObject = hitObj.transform.gameObject; //새로 클릭한 오브젝트로 대체

                            hitObj.transform.gameObject.GetComponent<UnitScript>().Touching();

                            if(tempObject)
                            {
                                tempObject.GetComponent<UnitScript>().Touching();
                                tempObject = hitObj.transform.gameObject;
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
            Debug.Log("Right Button Clicked");

            //tempObject가 클릭중인지 검사
            if( tempObject.GetComponent<UnitScript>().isTouching() == true)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitObj;

                if (Physics.Raycast(ray, out hitObj, 200.0f))
                {
                    //Debug.Log(hitObj.transform.gameObject.name);

                    if (hitObj.transform.gameObject.layer == 11) //클릭한 오브젝트가 애너미인가
                    {
                        //Debug.Log(hitObj.transform.gameObject.name);

                        if (tempObject.GetComponent<UnitScript>())
                        {
                            tempObject.GetComponent<UnitScript>().target = hitObj.transform;
                        }

                        Debug.Log(tempObject.GetComponent<UnitScript>().target);

                        //else if (gameObject.GetComponent<KnightScript>())
                        //{
                        //    gameObject.GetComponent<KnightScript>().target = hitObj.transform;
                        //}
                    }
                }
            }            
        }
    }
}
