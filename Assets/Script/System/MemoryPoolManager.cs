using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MemoryPool
{
    public GameObject source; //object
    public int poolMax;
    public GameObject folderObj;

    public List<GameObject> unusedList = new List<GameObject>(); //This is memory pool
}

public class MemoryPoolManager : MonoBehaviour // : Singleton
{ 
    public static MemoryPoolManager _instance = null;

    public static MemoryPoolManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Singleton == null");

            return _instance;
        }
    }

    void Awake()
    {
        Debug.Log("Memory Pool Manager Init");
        _instance = this;
    }

    public int defaultSize = 10;
    public GameObject[] poolList;
    public int[] poolAmount;


    public Dictionary<string, MemoryPool> memorypoolList = new Dictionary<string, MemoryPool>();


    public void InitObjPool() //IEnumerator
    {
        Debug.Log("InitObjPool Start");

        for (int i = 0; i < poolList.Length; ++i)
        {
            MemoryPool memoryPool = new MemoryPool();
            memoryPool.source = poolList[i];
            memorypoolList[poolList[i].name] = memoryPool;
            
            Debug.Log("InitObjPool : " + poolList[i].name);

            //Hierarchy에 추가
            GameObject folder = new GameObject();
            folder.name = poolList[i].name;
            folder.transform.parent = this.transform;
            memoryPool.folderObj = folder;

            int amount = defaultSize;
            if (poolAmount.Length > i && poolAmount[i] != 0)
                amount = poolAmount[i];

            for(int j = 0; j < amount; ++j)
            {
                GameObject inst = Instantiate(memoryPool.source) as GameObject;
                inst.SetActive(false);
                inst.transform.parent = folder.transform;
                memoryPool.unusedList.Add(inst);

                //yield return new WaitForEndOfFrame(); //풀 생성시 부하 줄이기 위해 코루틴 사용 ... 풀 생성이 안되는 문제 있음
            }

            memoryPool.poolMax = amount;
        }
    }

    public GameObject Get(string key)
    {
        if( !memorypoolList.ContainsKey(key) )
        {
            Debug.Log("MemoryPoolManager Get error : " + key);
            return null;
        }

        MemoryPool pool = memorypoolList[key];

        if (pool.unusedList.Count > 0)
        {
            GameObject obj = pool.unusedList[0];
            pool.unusedList.RemoveAt(0);
            obj.SetActive(true);

            return obj;
        }
        else
        {
            GameObject obj = Instantiate(pool.source);
            obj.transform.parent = pool.folderObj.transform;
            return obj;
        }
    }

    public void Free(GameObject obj)
    {
        string key = obj.transform.parent.name;

        if( !memorypoolList.ContainsKey(key) )
        {
            Debug.Log("MemoryPoolManager Free error : " + key);
            obj.SetActive(false);
            return;
        }

        MemoryPool pool = memorypoolList[key];

        obj.SetActive(false);
        pool.unusedList.Add(obj);
    }
}
