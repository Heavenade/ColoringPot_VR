using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.Extras;

public class SaveDialog : MonoBehaviour
{
    public static SaveDialog instance;
    //from규리 : 저장할 도자기 해당 스크립트에서 받아와야하나? 도와줘요 성범!

    public SteamVR_LaserPointer leftPointer;
    public SteamVR_LaserPointer rightPointer;
    public SteamVR_Action_Boolean touchPadAction;
    private int tick;

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
        this.transform.localScale = new Vector3(0, 0, 0);
        tick = System.Environment.TickCount;

        leftPointer.PointerClick += PointerClick;
        rightPointer.PointerClick += PointerClick;
    }

    void Update()
    {
        int curTick = System.Environment.TickCount;
        if (curTick - tick >= 200)
        {
            tick = curTick;

            //왼손입력 - Save는 수정해야함
            bool touchPadValue = touchPadAction.GetState(SteamVR_Input_Sources.LeftHand);
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
        if (e.target.name == "SaveYesBtn")
        {
            //Hide SaveDialog
            hideMenu();

            //Save Pottery
            //from규리 : 도자기 저장 코드 연결 부탁합니다.

            //도자기 저장 알림창
            //from규리 : 리소스 + 코드 추가 필요

            //PotteryMoldingScene으로 이동
            SceneManager.LoadScene(sceneName: "PotteryMoldingScene");
        }
        else if (e.target.name == "SaveNoBtn")
        {
            //Hide SaveDialog
            hideMenu();
            //PotteryMoldingScene으로 이동
            SceneManager.LoadScene(sceneName: "PotteryMoldingScene");
        }
    }

    public void showMenu()
    {
        // 메뉴 활성화
        this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
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
