using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MemoryPoolScript// : MonoBehaviour
{
    public MemoryPoolScript(){}

    public MemoryPoolScript(GameObject obj, string name, int size)
    {
        this.poolObj = obj;
        this.nameObj = name;
        this.poolMax = size;
    }

    public GameObject poolObj;
    public string nameObj;
    public int poolMax;
    public int counter;
    public GameObject folderObj;

    
    public List<GameObject> pool; //This is memory pool

    public void InitPoolSetting(GameObject obj, string name, int size)
    {
        this.poolObj = obj;
        this.nameObj = name;
        this.poolMax = size;
    }

    public IEnumerator InitObjPool()
    {
        for (counter = 0; counter < poolMax; ++counter)
        {
            GameObject inst = GameObject.Instantiate(poolObj) as GameObject;
            inst.name = nameObj + counter;
            inst.SetActive(false);
            inst.transform.parent = folderObj.transform;

            yield return new WaitForEndOfFrame(); //풀 생성시 부하 줄이기 위해 코루틴 사용
        }
    }

    public GameObject GetObj()
    {
        if(pool.Count > 0)
        {
            GameObject obj = pool[0];
            pool.RemoveAt(0);
            obj.SetActive(true);

            return obj;
        }
        else
        {
            ++counter;
            GameObject obj = GameObject.Instantiate(poolObj) as GameObject;
            obj.name = nameObj + counter;
            obj.transform.parent = folderObj.transform;
            return obj;
        }
    }

    public void Free(GameObject obj)
    {
        obj.SetActive(false);
        pool.Add(obj);
    }
}
