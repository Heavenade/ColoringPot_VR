using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.Extras;

public class DeleteDialog : MonoBehaviour
{
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

            //Delete Dialog 출력 - 오른손 입력- 어케할까 고민중
            //bool touchPadValue = touchPadAction.GetState(SteamVR_Input_Sources.RightHand);
            //if (touchPadValue)
            //{
            //    if (this.transform.localScale.Equals(new Vector3(0, 0, 0)))
            //    {
            //        showMenu();
            //    }
            //    else
            //    {
            //        hideMenu();
            //    }
            //}
        }
    }

    private void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.name == "CompleteYesBtn")
        {
            //Hide DeleteDialog
            hideMenu();

            // 효과음 재생
            EffectManager.instance.Play("button6");

            //선택한 도자기 삭제 연결

        }
        else if (e.target.name == "CompleteNoBtn")
        {
            //Hide CompleteDialog
            hideMenu();

            // 효과음 재생
            EffectManager.instance.Play("button6");
        }
    }

    private void showMenu()
    {
        // 메뉴 활성화
        UIManager.instance.isUIopen = true;
        this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
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
