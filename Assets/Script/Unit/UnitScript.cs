using UnityEngine;
using System.Collections;

//Unit관련 클래스의 베이스를 정의합니다.

public class UnitScript : MonoBehaviour
{
    public Transform target = null;

    float moveSpeed;         //이동 속도
    float rotationSpeed;     //회전 속도
    float attackableRange;   //공격 가능한 거리
    int normalDamage;           //평타 데미지


    //유닛 터치
    bool isTouch = false;

    public void Touching()
    {
        isTouch = true;
    }

    public void UnTouching()
    {
        isTouch = false;
    }

    public bool isTouching()
    {
        return isTouch ? true : false;
    }

    void Update()
    {
        if (isTouch == true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Right Button Clicked");

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitObj;

                if (Physics.Raycast(ray, out hitObj, 200.0f))
                {
                    // if (hitObj.transform.gameObject.layer == 5)
                    //Debug.Log(hitObj.transform.gameObject.name);

                    if (hitObj.transform.gameObject.layer == 11)
                    {
                        //Debug.Log(hitObj.transform.gameObject.name);

                        if (gameObject.GetComponent<MageScript>())
                        {
                            gameObject.GetComponent<MageScript>().target = hitObj.transform;
                        }
                        else if (gameObject.GetComponent<KnightScript>())
                        {
                            gameObject.GetComponent<KnightScript>().target = hitObj.transform;
                        }

                        //target = hitObj.transform;
                    }
                }
            }
        }
    }
}
