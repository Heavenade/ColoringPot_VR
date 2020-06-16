using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public bool isUIopen;
    public bool isMolding;
    public bool isColoring;

    #region Singleton

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        isUIopen = false;
        isMolding = false;
        isColoring = false;
    }
    #endregion Singleton

   

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
