using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerScript : MonoBehaviour
{
    int myLayer;
    int enemyLayer;

    public GameObject towerAttack;
    GameObject[] particlePool = null;

    public GameObject spawnUnit;
    GameObject[] spawnUnitPool = null;

    public float spawnTime;
    float deltaSpawnTime = 0.0f;

    GameObject nowTarget;

    int spawnDirection;

    int poolSize = 10;
    int ckr = 0;
    int UnitCkr = 0;

    public int towerDamage = 25;
    Queue<GameObject> attackQ = new Queue<GameObject>();

    public float coolTimeckr = 3.0f;
    float coolTime = 3.0f;

    void Start()
    {
        particlePool = new GameObject[poolSize];

        for (int i = 0; i < 10; ++i)
        {
            particlePool[i] = Instantiate(towerAttack) as GameObject;
            particlePool[i].name = "towerAttack" + i;
            particlePool[i].SetActive(false);
        }

        spawnUnitPool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; ++i)
        {
            spawnUnitPool[i] = Instantiate(spawnUnit) as GameObject;
            spawnUnitPool[i].name = spawnUnit.name + i;
            if (myLayer == 11)
            {
                spawnUnitPool[i].transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

                if (spawnUnitPool[i].GetComponent<MageScript>())
                {
                    //Debug.Log("test");
                    spawnUnitPool[i].GetComponent<MageScript>().target = GameObject.Find("MyWallGate").transform;
                }
                else if (spawnUnitPool[i].GetComponent<KnightScript>())
                {
                    //Debug.Log("test");
                    spawnUnitPool[i].GetComponent<KnightScript>().target = GameObject.Find("MyWallGate").transform;
                }
            }
               

            float x = transform.position.x;
            float z = transform.position.z + spawnDirection;
            spawnUnitPool[i].transform.position = new Vector3(x, 1.0f, z);

            SetLayerRecursively(spawnUnitPool[i], myLayer);

            spawnUnitPool[i].SetActive(false);
        }
    }

    void Awake()
    {
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
        if (!target)
            return;

        if (!target.GetComponent<DamageScript>().isExist())
        { 
            //Debug.Log("false");
            //nowTarget = attackQ.Dequeue();
            return;
        }
        
        float x = gameObject.transform.position.x;
        float z = gameObject.transform.position.z;

        Vector3 temp = target.gameObject.transform.position - new Vector3(x, 25, z);

        if (ckr == poolSize)
            ckr = 0;

        particlePool[ckr].SetActive(true);
        particlePool[ckr].transform.position = new Vector3(x, 25, z);
        particlePool[ckr].transform.rotation = Quaternion.LookRotation(temp);

        ckr++;

        target.GetComponent<DamageScript>().Hit(towerDamage);
    }

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

        if(UnitCkr == poolSize)
        {
            for(int i = 0; i < poolSize; i++)
            {
                if( !spawnUnitPool[i] )
                {
                    spawnUnitPool[i] = Instantiate(spawnUnit) as GameObject;
                    spawnUnitPool[i].name = spawnUnit.name + i;

                    if (myLayer == 11)
                    {
                        spawnUnitPool[i].transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

                        if (spawnUnitPool[i].GetComponent<MageScript>())
                        {
                            //Debug.Log("test");
                            spawnUnitPool[i].GetComponent<MageScript>().target = GameObject.Find("MyWallGate").transform;
                        }
                        else if (spawnUnitPool[i].GetComponent<KnightScript>())
                        {
                            //Debug.Log("test");
                            spawnUnitPool[i].GetComponent<KnightScript>().target = GameObject.Find("MyWallGate").transform;
                        }
                    }
                    float x = transform.position.x;
                    float z = transform.position.z + spawnDirection;
                    spawnUnitPool[i].transform.position = new Vector3(x, 1.0f, z);

                    SetLayerRecursively(spawnUnitPool[i], myLayer);

                    spawnUnitPool[i].SetActive(false);
                }

            }
        }

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

        if (deltaSpawnTime > spawnTime)
        {
            deltaSpawnTime = 0.0f;

            if (UnitCkr == poolSize)
                UnitCkr = 0;



            spawnUnitPool[UnitCkr].SetActive(true);


            UnitCkr++;
        }
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
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
