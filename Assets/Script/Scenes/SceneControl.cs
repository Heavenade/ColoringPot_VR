using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public static SceneControl instance;

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
    }
    #endregion Singleton

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ToColoring()
    {
        //페이드

        //컬러링으로 이동
        SceneManager.LoadScene(sceneName: "ColoringScene");

        Transform player = GameObject.Find("Player").transform;

    }

    public void ToGallery()
    {
        //페이드
        
        //갤러리로 이동
        SceneManager.LoadScene(sceneName: "GalleryScene");
    }


    public void ToMolding()
    {
        //페이드

        //몰딩으로 이동
        SceneManager.LoadScene(sceneName: "PotteryMoldingScene");
    }


}
