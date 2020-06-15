using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Valve.VR.Extras;
using UnityEngine.SceneManagement;

public class PlayerHandHandler : MonoBehaviour
{
    public GameObject curClick;
    public GameObject beforeClick;

    public SteamVR_LaserPointer leftPointer;
    public SteamVR_LaserPointer rightPointer;

    private ExhibitionTable table;

    // Start is called before the first frame update
    void Start()
    {
        leftPointer.PointerIn += PointerInside;
        leftPointer.PointerOut += PointerOutside;
        leftPointer.PointerClick += PointerClick;

        rightPointer.PointerIn += PointerInside;
        rightPointer.PointerOut += PointerOutside;
        rightPointer.PointerClick += PointerClick;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        beforeClick = curClick;
        curClick = e.target.gameObject;

        if (beforeClick != null && curClick != null)
        {
            // 도자기 삭제
            if (curClick.tag == "Interactor" && beforeClick.name == "delete")
            {
                table.DeleteDojagi(curClick);
            }

            // 도자기 위치 변경
            if (curClick.tag == "Interactor" && beforeClick.tag == "Interactor")
            {
                table.ChangeLocation(beforeClick, curClick);
            }

        }
        else if (curClick != null)
        {
            // 갤러리 퇴장
            if (curClick.name == "exit")
            {
                SceneManager.LoadScene(sceneName: "StartScene");
            }

            
        } else if (beforeClick != null)
        {
            // 삭제 취소 ( 아무곳이나 클릭 )
            if (beforeClick.name == "delete")
            {
                beforeClick = null;
            }
        }

    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        Debug.Log("pointer exited: " + e.target.name);
    }
}
