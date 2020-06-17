using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.Extras;

public class BakeDialog : MonoBehaviour
{
    public MeshFilter pottery;
    public SteamVR_LaserPointer leftPointer;
    public SteamVR_LaserPointer rightPointer;
    public SteamVR_Action_Boolean touchPadAction;
    private int tick;

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

            //BakeDialog 출력 - MoldingScene에서 오른손 입력
            bool touchPadValue = touchPadAction.GetState(SteamVR_Input_Sources.RightHand);
            if (touchPadValue && UIManager.instance.isUIopen == false && SceneManager.GetActiveScene().name == "PotteryMoldingScene")
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
        if (e.target.name == "BakeYesBtn")
        {
            hideMenu();

            //도자기
            GameManager.instance.potteryMesh = pottery.mesh;

            //ColoringScene으로 이동
            SceneControl.instance.ToColoring();

            // 효과음 재생
            EffectManager.instance.Play("button6");
        }
        else if (e.target.name == "BakeNoBtn")
        {
            hideMenu();
        }
    }

    private void showMenu()
    {
        // 메뉴 활성화
        UIManager.instance.isUIopen = true;
        this.transform.localScale = new Vector3(1.0f,1.0f, 1.0f);
        // 컨트롤러 모델 활성화
        leftPointer.gameObject.SetActive(true);
        rightPointer.gameObject.SetActive(true);
        // 효과음 재생
        EffectManager.instance.Play("button4");
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
