using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerScript : MonoBehaviour
{
    int myLayer;
    int enemyLayer;

    public GameObject towerAttack; //타워의 공격 파티클
    //GameObject[] particlePool = null;
    public MemoryPoolScript towerAttackPool = new MemoryPoolScript(); //(towerAttack, "towerAttack", 10)


    public GameObject spawnUnit; //타워가 소환하는 유닛
    //GameObject[] spawnUnitPool = null;
    public MemoryPoolScript spawnUnitPool = new MemoryPoolScript(); //(spawnUnit, "spawnUnit", 10)


    public float spawnTime;
    float deltaSpawnTime = 0.0f;

    Vector3 spawnPosition;
    int spawnDirection;

    GameObject nowTarget;

    //int poolSize = 10;
    //int ckr = 0;
    //int UnitCkr = 0;

    public int towerDamage = 25;
    Queue<GameObject> attackQ = new Queue<GameObject>();

    public float coolTimeckr = 3.0f;
    float coolTime = 3.0f;

    void Start()
    {
        float x = transform.position.x;
        float z = transform.position.z + spawnDirection;

        spawnPosition = new Vector3(x, 1.0f, z);


        //particlePool = new GameObject[poolSize];
        //
        //for (int i = 0; i < 10; ++i)
        //{
        //    particlePool[i] = Instantiate(towerAttack) as GameObject;
        //    particlePool[i].name = "towerAttack" + i;
        //    particlePool[i].SetActive(false);
        //}

        //spawnUnitPool = new GameObject[poolSize];
        //
        //for (int i = 0; i < poolSize; ++i)
        //{
        //    spawnUnitPool[i] = Instantiate(spawnUnit) as GameObject;
        //    spawnUnitPool[i].name = spawnUnit.name + i;
        //    if (myLayer == 11)
        //    {
        //        spawnUnitPool[i].transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        //
        //        if (spawnUnitPool[i].GetComponent<MageScript>())
        //        {
        //            //Debug.Log("test");
        //            spawnUnitPool[i].GetComponent<MageScript>().target = GameObject.Find("MyWallGate").transform;
        //        }
        //        else if (spawnUnitPool[i].GetComponent<KnightScript>())
        //        {
        //            //Debug.Log("test");
        //            spawnUnitPool[i].GetComponent<KnightScript>().target = GameObject.Find("MyWallGate").transform;
        //        }
        //    }
        //    spawnUnitPool[i].transform.position = spawnPosition;
        //
        //    SetLayerRecursively(spawnUnitPool[i], myLayer);
        //
        //    spawnUnitPool[i].SetActive(false);
        //}
    }

    //레이어 설정
    void Awake()
    {
        //풀셋팅
        towerAttackPool.InitPoolSetting(towerAttack, "towerAttack", 10);
        towerAttackPool.InitObjPool();

        spawnUnitPool.InitPoolSetting(spawnUnit, "spawnUnit", 10);
        spawnUnitPool.InitObjPool();


        myLayer = gameObject.layer;

        //소환 방향 초기화
        if (myLayer == 10)
        {
            spawnDirection = 10;
            enemyLayer = 11;
        }

        else if (myLayer == 11)
        {
            //Debug.Log(myLayer = gameObject.layer); 
            spawnDirection = -10;
            enemyLayer = 10;
        }
    }

    IEnumerator Attack(GameObject target)
    {
        if (!target)
        {
            //return
            yield break;
        }

        if (!target.GetComponent<DamageScript>().isExist())
        { 
            Debug.Log("Unattackable target");
            //nowTarget = attackQ.Dequeue();
            //return;
            yield break;
        }
        
        //공격자 위치
        float x = gameObject.transform.position.x;
        float z = gameObject.transform.position.z;

        Vector3 look = target.gameObject.transform.position - new Vector3(x, 25, z);

        //if (ckr == poolSize)
        //    ckr = 0;
        //
        //particlePool[ckr].SetActive(true);
        //particlePool[ckr].transform.position = new Vector3(x, 25, z);
        //particlePool[ckr].transform.rotation = Quaternion.LookRotation(temp);
        //
        //ckr++;

        GameObject particle = towerAttackPool.GetObj();
        particle.transform.position = new Vector3(x, 25, z);
        particle.transform.rotation = Quaternion.LookRotation(look);

        yield return new WaitForSeconds(3.0f);

        target.GetComponent<DamageScript>().Hit(towerDamage);
        towerAttackPool.Free(particle);
    }

    //유닛이 충돌 체크 범위 안에 들어왔을때 공격 큐에 넣음.
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision Enter" + gameObject.name + other.name);

        if (other.gameObject.layer == enemyLayer)
        {
            Debug.Log("mylayer " + gameObject.name + " "  + gameObject.layer);
            Debug.Log("otherlayer " + other.name + " " + other.gameObject.layer);
            attackQ.Enqueue(other.gameObject);
        }
    }

    void Update()
    {
        coolTime += Time.deltaTime;
        deltaSpawnTime += Time.deltaTime;

        //타워 공격
        if (attackQ.Count != 0)
        {
            if (!nowTarget)
                nowTarget = attackQ.Dequeue();

            if (coolTime >= coolTimeckr)
            {
                Debug.Log("attack");
                coolTime = 0.0f;
                Attack(nowTarget);
            }
        }

        //유닛 스폰
        if (deltaSpawnTime > spawnTime)
        {
            deltaSpawnTime = 0.0f;

            //if (UnitCkr == poolSize)
            //    UnitCkr = 0;
            //spawnUnitPool[UnitCkr].SetActive(true);
            //UnitCkr++;

            GameObject unit = spawnUnitPool.GetObj();

            //소환 위치 결정, 애너미 소환시 타겟 설정 
            if (myLayer == 11) //소환하는 타워의 레이어가 enemy레이어면.
            {
                unit.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

                if (unit.GetComponent<MageScript>()) //유닛이 메이지일경우
                {
                    //Debug.Log("test");
                    unit.GetComponent<MageScript>().target = GameObject.Find("MyWallGate").transform;
                }
                else if (unit.GetComponent<KnightScript>()) //유닛이 나이트일경우
                {
                    //Debug.Log("test");
                    unit.GetComponent<KnightScript>().target = GameObject.Find("MyWallGate").transform;
                }
            }

            float x = transform.position.x;
            float z = transform.position.z + spawnDirection;
            unit.transform.position = new Vector3(x, 1.0f, z);
            
            SetLayerRecursively(unit, myLayer);
        }

        //if(UnitCkr == poolSize)
        //{
        //    for(int i = 0; i < poolSize; i++)
        //    {
        //        if( !spawnUnitPool[i] )
        //        {
        //            spawnUnitPool[i] = Instantiate(spawnUnit) as GameObject;
        //            spawnUnitPool[i].name = spawnUnit.name + i;
        //
        //            상대 타워 소환시 타겟 설정
        //            if (myLayer == 11)
        //            {
        //                spawnUnitPool[i].transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        //
        //                if (spawnUnitPool[i].GetComponent<MageScript>())
        //                {
        //                    //Debug.Log("test");
        //                    spawnUnitPool[i].GetComponent<MageScript>().target = GameObject.Find("MyWallGate").transform;
        //                }
        //                else if (spawnUnitPool[i].GetComponent<KnightScript>())
        //                {
        //                    //Debug.Log("test");
        //                    spawnUnitPool[i].GetComponent<KnightScript>().target = GameObject.Find("MyWallGate").transform;
        //                }
        //            }
        //            float x = transform.position.x;
        //            float z = transform.position.z + spawnDirection;
        //            spawnUnitPool[i].transform.position = new Vector3(x, 1.0f, z);
        //
        //            SetLayerRecursively(spawnUnitPool[i], myLayer);
        //
        //            spawnUnitPool[i].SetActive(false);
        //        }
        //
        //    }
        //}
        //
    }

    

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        //배열 안 상속 포함 오브젝트 레이어 전부 변경
        if (null == obj)
        {
            return;
        }
    
        obj.layer = newLayer;
    
        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
