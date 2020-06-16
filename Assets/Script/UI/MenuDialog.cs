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
            //DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        //else
        //{
        //    Destroy(this.gameObject);
        //}
    }
    #endregion Singleton

    void Start()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
        tick = System.Environment.TickCount;

        leftPointer.PointerClick += PointerClick;
        rightPointer.PointerClick += PointerClick;
    }


    void Update()
    {
        //메뉴는 게임 시작시 PotteryMoldingScene에서 바로 시작하도록 바꾸기
        int curTick = System.Environment.TickCount;
        if (curTick - tick >= 200)
        {
            tick = curTick;

            //MenuDialog - 게임시작 시 자동 출력
            bool touchPadValue = touchPadAction.GetState(SteamVR_Input_Sources.RightHand);
            if (touchPadValue)
            {
                if (this.transform.localScale.Equals(new Vector3(0, 0, 0)))
                {
                    showMenu();
                }
                else
                {
                    hideMenu();
                }
            }
        }
    }

    private void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.name == "GameStartBtn")
        {
            //MenuDialog 숨기고
             hideMenu();

            //게임 시작

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
        this.transform.localScale = new Vector3(.7f, .7f, .7f);
        // 컨트롤러 모델 활성화
        leftPointer.gameObject.SetActive(true);
        rightPointer.gameObject.SetActive(true);
    }

    private void hideMenu()
    {
        // 메뉴 비활성화
        this.transform.localScale = new Vector3(0, 0, 0);
        // 컨트롤러 모델 비활성화
        leftPointer.gameObject.SetActive(false);
        rightPointer.gameObject.SetActive(false);
    }
}
