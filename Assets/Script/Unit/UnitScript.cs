using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Unit관련 클래스의 베이스를 정의합니다.

abstract public class UnitScript : MonoBehaviour
{
    //클릭한 목표
    public GameObject target;
    //진행방향
    public Vector3 dst;

    public int enemyLayer;
    
    //유닛 터치
    public bool isTouch { get; set; }

    void Start()
    { 

    }

    void Awake()
    {
        //오브잭트 재스폰시 타겟초기화
        target = null;
        
        if (gameObject.layer == Common.Config.playerLayer)
        {
            enemyLayer = Common.Config.enemyLayer;
        }

        else if (gameObject.layer == Common.Config.enemyLayer)
        {
            enemyLayer = Common.Config.playerLayer;
        }
    }

    public GameObject RayCastFront(float distance)
    {
        RaycastHit hit;

        if( Physics.Raycast(transform.position, transform.forward, out hit, distance) )
        {
            //Debug.Log("ray");
            Debug.DrawLine(transform.position, hit.point, Color.magenta);//

            var isUnit = hit.collider.gameObject.GetComponent<UnitScript>();

            if( isUnit != null )
            {
                return hit.transform.gameObject;
            }
        }
        return null;
    }

    public void Targeting(GameObject t)
    {
        target = t;

        gameObject.GetComponent<WayPoint>().CalcWay();
        dst = gameObject.GetComponent<WayPoint>().way.Pop();
    }
}
