using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Mesh potteryMesh;

    private static GameManager localInstance;
    public static GameManager instance {
        get {
            if (localInstance == null)
            {
                GameManager obj = FindObjectOfType<GameManager>();
                if (obj != null)
                {
                    localInstance = obj;
                }
                else
                {
                    GameManager newObj = new GameObject("GameManager Object").AddComponent<GameManager>();
                    localInstance = newObj;
                }
            }
            return localInstance;
        }
        private set {
            localInstance = value;
        }
    }

    private void Awake()
    {
        GameManager[] objs = FindObjectsOfType<GameManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
