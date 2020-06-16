using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.Extras;

public class MenuDialog : MonoBehaviour
{
    public static MenuDialog instance;

    public SteamVR_LaserPointer leftPointer;
    public SteamVR_LaserPointer rightPointer;
    public SteamVR_Action_Boolean touchPadAction;
    private int tick;

    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
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
        this.transform.localScale = new Vector3(0, 0, 0);
        tick = System.Environment.TickCount;

        leftPointer.PointerClick += PointerClick;
        rightPointer.PointerClick += PointerClick;

        //MenuDialog - 게임시작 시 (해당 씬 실행 시) 자동 출력
        showMenu();
    }


    void Update()
    {
 
    }

    private void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.name == "GameStartBtn")
        {
            //MenuDialog 숨기고
             hideMenu();

            //게임 시작 - 그냥 UI가 열렸는지로 확인해도 될 거 같은데
            //UIManager.instance.isMolding = true;

        }
        else if (e.target.name == "ToGalleryBtn")
        {
            //MenuDialog 숨기고
            hideMenu();        
        }
        else if (e.target.name == "QuitGameBtn")
        {
            //MenuDialog 숨기고
            hideMenu();

            //게임 종료
            Application.Quit();                
        }
    }

    public void showMenu()
    {
        UIManager.instance.isUIopen = true;
        this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        // 컨트롤러 모델 활성화
        leftPointer.gameObject.SetActive(true);
        rightPointer.gameObject.SetActive(true);
    }

    private void hideMenu()
    {
        // 메뉴 비활성화
        UIManager.instance.isUIopen = false;
        this.transform.localScale = new Vector3(0, 0, 0);
        // 컨트롤러 모델 비활성화
        leftPointer.gameObject.SetActive(false);
        rightPointer.gameObject.SetActive(false);
    }
}
