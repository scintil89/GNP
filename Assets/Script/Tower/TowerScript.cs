using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerScript : MonoBehaviour
{
    int myLayer;
    int enemyLayer;

    //유니티 inspector에서 설정
    //public string towerAttack;
    public string spawnUnit; 

    public GameObject towerAttackParticle; //타워의 공격 파티클

    //float attackRange = 11.0f;

    public float spawnTime;
    float deltaSpawnTime = 5.0f;

    Vector3 spawnPosition;
    int spawnDirection;

    [SerializeField]
    GameObject nowTarget;

    //int poolSize = 10;
    //int ckr = 0;
    //int UnitCkr = 0;

    public int towerDamage = 25;
    List<GameObject> attackQ = new List<GameObject>();

    public float coolTimeckr = 3.0f;
    float coolTime = 3.0f;

    void Start()
    {
        float x = transform.position.x;
        float z = transform.position.z + spawnDirection;

        spawnPosition = new Vector3(x, 1.0f, z);
    }

    //레이어 설정
    void Awake()
    {
        //풀셋팅
        //towerAttackPool.InitPoolSetting(towerAttack, "towerAttack", 10);
        //towerAttackPool.InitObjPool();
        //spawnUnitPool.InitPoolSetting(spawnUnit, "spawnUnit", 10);
        //spawnUnitPool.InitObjPool();

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

    void Attack(GameObject target)
    {
        //명확하지 않은 타겟일 경우 공격을 스킵한다.
        if (!target.GetComponent<DamageScript>().isExist())
        { 
            Debug.Log("Unattackable target");

            nowTarget = null;

            return;
        }
        
        //공격자 위치
        float x = gameObject.transform.position.x;
        float z = gameObject.transform.position.z;

        //if( Mathf.Sqrt( Mathf.Pow((x - target.transform.position.x), 2) + Mathf.Pow((z - target.transform.position.z), 2)) > attackRange)
        //{
        //    nowTarget = attackQ.Dequeue();
        //    return;
        //}

        //공격 파티클 포지션 생성, 포지션 설정
        GameObject particle = Instantiate(towerAttackParticle) as GameObject;         //GameObject particle = MemoryPoolManager.Instance.Get(towerAttack);
        if (!particle)
        {
            Debug.LogError("TowerScript particle instantiate Failed " );

            return;
        }

        Vector3 look = target.gameObject.transform.position - new Vector3(x, 25, z);
        particle.transform.position = new Vector3(x, 25, z);
        particle.transform.rotation = Quaternion.LookRotation(look);

        target.GetComponent<DamageScript>().Hit(towerDamage);

        //yield return new WaitForSeconds(3.0f);

        //MemoryPoolManager.Instance.Free(particle);

        StartCoroutine(ParticleFree(particle));
    }

    IEnumerator ParticleFree(GameObject particle)
    {
        yield return new WaitForSeconds(3.0f);

        //MemoryPoolManager.Instance.Free(particle);
        Destroy(particle);
    }

    void Update()
    {
        coolTime += Time.deltaTime;
        deltaSpawnTime += Time.deltaTime;

        //타워 타겟팅
        if (!nowTarget || !nowTarget.activeSelf)
        {
            if (attackQ.Count > 0)
            {
                nowTarget = attackQ[0];
                attackQ.RemoveAt(0);
            }
        }

        //타워 공격
        if (coolTime >= coolTimeckr && nowTarget)
        {
            //Debug.Log("attack");
            coolTime = 0.0f;
            Attack(nowTarget);
        }

        //유닛 스폰
        if (deltaSpawnTime > spawnTime)
        {
            deltaSpawnTime = 0.0f;

            //if (UnitCkr == poolSize)
            //    UnitCkr = 0;
            //spawnUnitPool[UnitCkr].SetActive(true);
            //UnitCkr++;

            GameObject unit = MemoryPoolManager.Instance.Get(spawnUnit);
            if( !unit )
            {
                Debug.LogError("TowerScript unit Get Failed " + spawnUnit);
                return;
            }

            //소환 위치 결정, 애너미 소환시 타겟 설정 
            if (myLayer == 11) //소환하는 타워의 레이어가 enemy레이어면.
            {
                //180도 회전시켜서 소환
                unit.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            
                if (unit.GetComponent<MageScript>()) //유닛이 메이지일경우
                {
                    //Debug.Log("test");
                    unit.GetComponent<MageScript>().target = GameObject.Find("MyWallGate");
                }
                else if (unit.GetComponent<KnightScript>()) //유닛이 나이트일경우
                {
                    //Debug.Log("test");
                    unit.GetComponent<KnightScript>().target = GameObject.Find("MyWallGate");
                }
            }

            //float x = transform.position.x;
            //float z = transform.position.z + spawnDirection;
            //unit.transform.position = new Vector3(x, 1.0f, z);
            unit.transform.position = spawnPosition;

            SetLayerRecursively(unit, myLayer);
        }
    }

    //유닛이 충돌 체크 범위 안에 들어왔을때 공격 큐에 넣음.
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision Enter " + gameObject.name  + " " + other.name);

        if (other.gameObject.layer == enemyLayer)
        {
            //Debug.Log("mylayer " + gameObject.name + " "  + gameObject.layer);
            //Debug.Log("otherlayer " + other.name + " " + other.gameObject.layer);
            attackQ.Add(other.gameObject);
        }
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
