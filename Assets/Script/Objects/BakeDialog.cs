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

    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
        tick = System.Environment.TickCount;

        leftPointer.PointerClick += PointerClick;
        rightPointer.PointerClick += PointerClick;
    }

    // Update is called once per frame
    void Update()
    {
        int curTick = System.Environment.TickCount;
        if (curTick - tick >= 200)
        {
            tick = curTick;

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
        if (e.target.name == "BakeButton")
        {
            GameManager.instance.potteryMesh = pottery.mesh;
            SceneManager.LoadScene(sceneName: "ColoringScene");
        }
        else if (e.target.name == "ResumeButton")
        {
            hideMenu();
        }
    }

    private void showMenu()
    {
        // 메뉴 활성화
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
