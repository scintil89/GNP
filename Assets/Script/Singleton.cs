using UnityEngine;
using System.Collections;

public class Singleton : MonoBehaviour
{
    public static Singleton _instance = null;

    public static Singleton Instance
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
        _instance = this;
    }
}