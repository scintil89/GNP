using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Unit관련 클래스의 베이스를 정의합니다.

abstract public class UnitScript : MonoBehaviour
{
    public Transform target;

    //float moveSpeed;         //이동 속도
    //float rotationSpeed;     //회전 속도
    //float attackableRange;   //공격 가능한 거리
    //int normalDamage;           //평타 데미지

    public int enemyLayer;
    
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

    void Start()
    {
        //오브잭트 재스폰시 타겟초기화
        Debug.Log("eeeeee");
        target = null;
    }

    //void Update()
    //{
    //    Debug.Log("eeeeee");
    //    
    //    if (isTouch == true)
    //    {
    //        if (Input.GetMouseButtonDown(1))
    //        {
    //            Debug.Log("Right Button Clicked");
    //    
    //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //            RaycastHit hitObj;
    //    
    //            if (Physics.Raycast(ray, out hitObj, 200.0f))
    //            {
    //                // if (hitObj.transform.gameObject.layer == 5)
    //                //Debug.Log(hitObj.transform.gameObject.name);
    //    
    //                
    //                if (hitObj.transform.gameObject.layer == 11)
    //                {
    //                    //Debug.Log(hitObj.transform.gameObject.name);
    //    
    //                    if (gameObject.GetComponent<MageScript>())
    //                    {
    //                        gameObject.GetComponent<MageScript>().target = hitObj.transform;
    //                    }
    //                    else if (gameObject.GetComponent<KnightScript>())
    //                    {
    //                        gameObject.GetComponent<KnightScript>().target = hitObj.transform;
    //                    }
    //    
    //                    //target = hitObj.transform;
    //                }
    //            }
    //        }
    //    }
    //}

    //void OnCollisionEnter(Collision collision)
    //{
    //    //충돌이 발생하면 충돌한 물체를 타겟으로 변경
    //    if (collision.gameObject.layer == enemyLayer)
    //    {
    //        target = collision.gameObject.transform;
    //    }
    //}
}
